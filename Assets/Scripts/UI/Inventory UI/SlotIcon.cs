using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SlotIcon : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IDropHandler
{
    public Item item;
    public Image imageIcon; 
    public GameObject frameQuantity;
    public TextMeshProUGUI textQuantity;

    private InventoryController _inventoryController;
    private Transform originalParent; 
    private CanvasGroup canvasGroup; 
    private Canvas canvas; 

    private int index; 

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        item = new Item();
    }

    public void SetInventoryController(InventoryController inventoryController) => _inventoryController = inventoryController;

    public void SetIndex(int id)
    {
        if (id < 0)
            return;
        index = id;
    }
    public int GetIndex()
    {
        if (index < 0)
            return -1;
        return index;
    }

    public void SetItem(Item itemValue)
    {
        item = itemValue;
        SetSprite();
        SetQuantity();
    }

    public void SetSprite()
    {
        if (item.type != ItemType.Empty)
        {
            imageIcon.sprite = _inventoryController.GetSpriteByID(item.id);
            imageIcon.color = Color.white;
        }
        else
            imageIcon.color = new Color(0, 0, 0, 0);
    }

    public void SetQuantity()
    { 
        if (item.type != ItemType.Empty && item.quantity > 1)
        {
            frameQuantity.SetActive(true);
            textQuantity.text = item.quantity.ToString();
        }
        else
        {
            frameQuantity.SetActive(false);
            textQuantity.text = "";
        }
    }

    public void EmptySlot()
    {
        item = null;
        imageIcon.sprite = null;
        imageIcon.color = new Color(0, 0, 0, 0);
        frameQuantity.SetActive(false);
        textQuantity.text = "";
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(item == null)
            return;
        if(item.type == ItemType.Empty)
            return;
        
        _inventoryController.SelectItem(this);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null)
            return;
        if (item.type == ItemType.Empty)
            return;

        originalParent = transform.parent;

        if (canvas == null)
            return;

        transform.SetParent(canvas.transform, true); 
        canvasGroup.blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (item == null)
            return;
        if (item.type == ItemType.Empty)
            return;

        transform.position = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent); 
            transform.localPosition = Vector3.zero;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlotGO = eventData.pointerDrag;
        if (draggedSlotGO == null)
            return;

        var draggedSlot = draggedSlotGO.GetComponent<SlotIcon>();
        if (draggedSlot == null)
            return;

        if (draggedSlot == this)
            return;
        
        _inventoryController.SwapItems(this, draggedSlot);
    }
}