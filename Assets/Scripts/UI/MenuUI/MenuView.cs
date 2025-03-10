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
        _startColor = new Color(0,0,0,0.4f);
        _startPosition = _buttonsContainer.position;
    }

    public void FadeBackgroundOut(TweenCallback onComplete) => _backgroundImage.DOFade(0, 0.2f).SetId("UI").OnComplete(onComplete);
    public void FadeBackgroundIn() => _backgroundImage.DOFade(_startColor.a, 0.2f).SetId("UI");
    
    public void MoveButtonsOut(TweenCallback onComplete) => _buttonsContainer.DOMoveY(_startPosition.y, 0.2f).SetId("UI").OnComplete(onComplete);
    public void MoveButtonsIn(TweenCallback onComplete) => _buttonsContainer.DOMoveY(720, 0.2f).SetId("UI").OnComplete(onComplete);
}
