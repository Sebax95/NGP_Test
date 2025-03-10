using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PickableObject: BaseMonoBehaviour, IPickeable
{
    public int id;
    public string name;
    public string description;
    public int quantity;
    public ItemType type;
    public Sprite sprite;
    
    private Vector3 _startPosition;

    
    public Item Item => new Item(id, name, description, quantity, type, sprite);

    protected override void Start()
    {
        _startPosition = transform.position;
        float randomDelay = Random.Range(0.5f, 2f);

        DOVirtual.DelayedCall(randomDelay, AnimateMovement);
    }
    private void AnimateMovement()
    {
        transform.DOMoveY(_startPosition.y + 0.5f, 1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        transform.DORotate(new Vector3(0, 360f, 0), 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
    

    public void Pick()
    {
        InventoryController.Instance.AddItemToInventory(Item);
        Destroy(gameObject);
    }

}