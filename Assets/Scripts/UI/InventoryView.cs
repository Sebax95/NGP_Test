using System;
using DG.Tweening;
using UnityEngine;

public class InventoryView: MonoBehaviour
{
    
    [SerializeField]
    private Transform inventoryContainer;
    [SerializeField] 
    private ItemDescription _itemDescription;

    private Vector3 _startPositionDescription;
    private Vector3 _startPositionInventory;
    private void Start()
    {
        _startPositionInventory = inventoryContainer.position;
    }

    public void AnimationDescription(Action onComplete) => _itemDescription.transform
        .DOMoveX(_startPositionDescription.x + 550, .2f)
        .SetEase(Ease.InOutSine)
        .OnComplete(() => onComplete?.Invoke());
    
    public void OpenInventory(Action onComplete)
    {
        inventoryContainer.DOMoveX(_startPositionInventory.x + 2365,.2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            _startPositionDescription = _itemDescription.transform.position;
            onComplete?.Invoke();
        });
    }
    public void CloseInventory(Action onComplete)
    {
        inventoryContainer.DOMoveX(_startPositionInventory.x, .2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            _itemDescription.transform.position = new Vector3(_startPositionDescription.x - 2365, _startPositionDescription.y, _startPositionDescription.z); 
            onComplete?.Invoke();
        });

    }
}