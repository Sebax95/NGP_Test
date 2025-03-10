using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDescription: MonoBehaviour
{
    public Item item;
    public Image imageSprite;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public Button useButton;
    public Button dropButton;

    private void Start() => EmptySlot();

    public void SetItem(Item itemValue)
    {
        item = itemValue;
        UpdateInfo();
    }
    
    private void SetSprite()
    {
        imageSprite.sprite = InventoryController.Instance.GetSpriteByID(item.id);
        imageSprite.color = Color.white;
    }
    private void SetText() => textName.text = item.name;
    private void SetDescription() => textDescription.text = item.description;

    public void EmptySlot()
    {
        item = null;
        imageSprite.sprite = null;
        imageSprite.color = new Color(0,0,0,0);
        textName.text = "";
        textDescription.text = "";
    }
    private void UpdateInfo()
    {
        SetSprite();
        SetText();
        SetDescription();
    }

    
}