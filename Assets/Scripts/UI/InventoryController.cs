using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    private InventoryView _view;
    
    [SerializeField]
    private Transform inventoryContainer;

    [SerializeField] 
    private ItemDescription ItemDescription;
    [SerializeField]
    private SlotIcon slotPrefab;
    private const int MAX_SLOTS = 60;
    [SerializeField]
    private Transform slotsContainer;
    private List<SlotIcon> slots;
    
    private SlotIcon _selectedSlot;
    
    private Inventory _inventory;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() => FillEmptySlots();

    public void Open()
    {
        inventoryContainer.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        UpdateManager.TogglePause();
    }
    public void Close()
    {
        inventoryContainer.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        UpdateManager.TogglePause();
    }
    private void FillEmptySlots()
    {
        slots = new List<SlotIcon>();
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            var slot = Instantiate(slotPrefab, slotsContainer);
            slot.SetInventoryView(this);
            slot.SetIndex(i);
            slot.EmptySlot();
            slots.Add(slot);
        }
    }

    public void SelectItem(SlotIcon slot)
    {
        _selectedSlot = slot;
        if(!ItemDescription.gameObject.activeSelf)
            ItemDescription.gameObject.SetActive(true);
        ItemDescription.SetItem(_selectedSlot.item);
    }
    public void SetInventory(Inventory newInventory)
    {
        _inventory = newInventory;
        UpdateInventory();
    }
    
    private void UpdateInventory()
    {
        if (_inventory == null || slots == null) return;
        
        foreach (var slot in slots)
            slot.EmptySlot();
        
        
        for (int i = 0; i < _inventory.items.Count; i++)
            if (i < MAX_SLOTS)
                slots[i].SetItem(_inventory.items[i]);
    }

}
