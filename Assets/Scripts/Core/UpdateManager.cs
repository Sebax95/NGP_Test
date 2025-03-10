using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Profiling;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance { get; private set; }
    private List<BaseMonoBehaviour> _updateSubscribers;

    [SerializeField] private bool _isPaused;

    private readonly List<Rigidbody> _nonKinematicRigidbodies = new();
    private readonly Dictionary<Rigidbody, Vector3> _cachedVelocities = new();

    public static event Action OnPause;
    public static event Action OnUnPause;

    public bool IsPaused
    {
        get => _isPaused;
        private set => _isPaused = value;
    }

    private void Awake()
    {
        InitializeSingleton();
        Application.targetFrameRate = 144;

        _updateSubscribers = new List<BaseMonoBehaviour>();
        InitializeRigidbodies();
    }

    public static void Subscribe(BaseMonoBehaviour subscriber) => Instance._updateSubscribers.Add(subscriber);

    public static void Unsubscribe(BaseMonoBehaviour subscriber) => Instance._updateSubscribers.Remove(subscriber);

    public static void TogglePause()
    {
        if (Instance.IsPaused)
            Instance.Resume();
        else
            Instance.Pause();
    }
    public static void SetPause(bool pause)
    {
        if (pause && !Instance.IsPaused)
            Instance.Pause();
        else if (!pause && Instance.IsPaused)
            Instance.Resume();
    }

    private void Update()
    {
        Profiler.BeginSample("Update Manager Update");
        ProcessUpdates(subscriber => subscriber.OnUpdate());
        Profiler.EndSample();
    }

    private void FixedUpdate()
    {
        Profiler.BeginSample("Update Manager FixedUpdate");
        ProcessUpdates(subscriber => subscriber.OnFixedUpdate());
        Profiler.EndSample();
    }

    private void LateUpdate()
    {
        Profiler.BeginSample("Update Manager LateUpdate");
        ProcessUpdates(subscriber => subscriber.OnLateUpdate());
        Profiler.EndSample();
    }

    private void ProcessUpdates(Action<BaseMonoBehaviour> updateAction)
    {
        if (_updateSubscribers == null || IsPaused) return;
        foreach (var subscriber in _updateSubscribers.Where(subscriber => !subscriber.IsIndividuallyPaused))
            updateAction(subscriber);
    }

    private void Pause()
    {
        IsPaused = true;
        DOTween.PauseAll();
        DOTween.Play("UI");
        CacheRigidbodiesVelocities();
        SetRigidbodiesKinematic(true);
        OnPause?.Invoke();
    }

    private void Resume()
    {
        IsPaused = false;
        DOTween.PlayAll();
        RestoreRigidbodiesVelocities();
        SetRigidbodiesKinematic(false);
        OnUnPause?.Invoke();
    }

    private void CacheRigidbodiesVelocities()
    {
        _cachedVelocities.Clear();
        foreach (var rigidbody in _nonKinematicRigidbodies.Where(rigidbody => rigidbody != null))
            _cachedVelocities[rigidbody] = rigidbody.velocity;
    }

    private void RestoreRigidbodiesVelocities()
    {
        foreach (var rigidbody in _nonKinematicRigidbodies)
        {
            if (_cachedVelocities.TryGetValue(rigidbody, out var velocity))
                rigidbody.velocity = velocity;
        }
    }

    private void SetRigidbodiesKinematic(bool isKinematic)
    {
        foreach (var rigidbody in _nonKinematicRigidbodies.Where(rigidbody => rigidbody != null))
            rigidbody.isKinematic = isKinematic;
    }

    private void InitializeRigidbodies()
    {
        _nonKinematicRigidbodies.AddRange(
            FindObjectsOfType<Rigidbody>()
                .Where(rigidbody => rigidbody != null && !rigidbody.isKinematic));
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}