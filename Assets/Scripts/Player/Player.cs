using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Player: BaseMonoBehaviour
{
    [SerializeField] 
    private float _speed;
    private Camera _camera;
    [SerializeField]
    private CinemachineFreeLook _cameraVirtual;
    private IController _controller;
    private PlayerView _playerView;
    private Rigidbody _rigidbody;
    private bool _isPicking;

    public bool canMove;
    protected override void Start()
    {
        base.Start();
        _controller = new PlayerController(this);
        _rigidbody = GetComponent<Rigidbody>();
        _playerView = GetComponent<PlayerView>();
        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        _isPicking = false;
        canMove = true;
        UpdateManager.OnPause += () => CameraPause(true);
        UpdateManager.OnUnPause += () => CameraPause(false);
    }

    private void CameraPause(bool isPaused)
    {
        _cameraVirtual.enabled = !isPaused;
    }
    public override void OnUpdate()
    {
        if(!canMove)
            return;
        _controller.CheckInputs();
    }

    public override void OnFixedUpdate()
    {
        if(!canMove)
            return;
        _controller.CheckMovementInputs();
    }

    public override void OnLateUpdate() => CameraForward();

    private void CameraForward()
    {
        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraDirection = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        
        if (cameraDirection.magnitude > 0.1f)
            transform.forward = cameraDirection;
    }
    
    public void MovePlayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Vector3 moveForward = transform.forward * direction.z; 
            Vector3 moveRight = transform.right * direction.x * 0.2f;    
            
            Vector3 moveDirection = moveForward + moveRight;
            
            if (direction.magnitude > 0.1f) 
                _rigidbody.velocity = moveDirection.normalized * _speed;
            else
                _rigidbody.velocity = Vector3.zero;

        }
        else
            StopPlayer();
        
        _playerView.MoveForward(direction.z);
        _playerView.Rotation(direction.x * 0.2f);
        
    }

    public void PickUpItem()
    {
        if (_isPicking)
            return;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, .5f, -Vector3.up);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out PickableObject pickableObject))
            {
                _isPicking = true;
                canMove = false;
                pickableObject.Pick(); 
                _playerView.PickUp(); 
                StartCoroutine(WaitToMove());
                break;
            }
        }
    }

    public void UseItem(Item item)
    {
        switch (item.type)
        {
            case ItemType.Weapon:
                break;
            case ItemType.Consumable:
                ConsumeItem();
                InventoryController.Instance.RemoveItemFromInventory(item);
                break;
            case ItemType.Quest:
                break;
        }
    }

    private void ConsumeItem()
    {
        canMove = false;
        _playerView.Eat();
        StartCoroutine(WaitToMove());
        //do more things like health the player or boost it
    }

    public void OpenInventory() => InventoryController.Instance.Open();

    private IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(1.7f);
        _isPicking = false;
        canMove = true;
    }

    private void StopPlayer() => _rigidbody.velocity = Vector3.zero;
}