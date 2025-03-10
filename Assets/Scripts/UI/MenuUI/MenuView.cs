using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField]
    private Transform _buttonsContainer;
    [SerializeField]
    private Image _backgroundImage;
    private Color _startColor;
    private Vector3 _startPosition;
    private void Start()
    {
        _startColor = _backgroundImage.color;
        _startPosition = _buttonsContainer.position;
    }

    public void FadeBackgroundOut(TweenCallback onComplete) => _backgroundImage.DOFade(0, 0.5f).SetId("UI").OnComplete(onComplete);
    public void FadeBackgroundIn() => _backgroundImage.DOFade(_startColor.a, 0.5f).SetId("UI");
    
    public void MoveButtonsOut(TweenCallback onComplete) => _buttonsContainer.DOMoveY(-1000, 0.5f).SetId("UI").OnComplete(onComplete);
    public void MoveButtonsIn() => _buttonsContainer.DOMoveY(_startPosition.y, 0.5f).SetId("UI");
}
