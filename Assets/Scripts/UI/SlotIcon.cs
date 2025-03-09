using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SlotIcon: MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Image imageIcon;
    public GameObject frameQuantity;
    public TextMeshProUGUI textQuantity;
    private InventoryController _inventoryController;

    
    private int index;
    public int slotIndex;
    public bool isSelected;
    public bool isHovered;

    public void SetInventoryView(InventoryController inventoryController) => _inventoryController = inventoryController;

    
    public void SetItem(Item itemValue)
    {
        item = itemValue;
        SetSprite();
        SetQuantity();
    }
    
    public void SetSprite()
    {
        imageIcon.sprite = item.sprite;
        imageIcon.color = Color.white;
    }
    public void SetQuantity()
    {
        if (item.quantity < 1)
            frameQuantity.SetActive(false);
        else
        {
            frameQuantity.SetActive(true);
            textQuantity.text = item.quantity.ToString();
        }
        
    }
    public void SetIndex(int id) => index = id;

    public void EmptySlot()
    {
        item = null;
        imageIcon.sprite = null;
        imageIcon.color = new Color(0,0,0,0);
        frameQuantity.SetActive(false);
        textQuantity.text = "";
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(item == null)
            return;
        _inventoryController.SelectItem(this);
    }
}