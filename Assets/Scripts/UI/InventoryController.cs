using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    private InventoryView _view;
    
    [SerializeField]
    private Transform inventoryContainer;

    [SerializeField] 
    private ItemDescription _itemDescription;
    [SerializeField]
    private GameObject slotPrefab;
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

    private void Start()
    {
        FillEmptySlots();
        _view = GetComponent<InventoryView>();
    }

    public void Open()
    {
        _view.OpenInventory(UpdateManager.TogglePause);
        Cursor.lockState = CursorLockMode.None;
    }
    public void Close()
    {
        _view.CloseInventory(UpdateManager.TogglePause);
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void FillEmptySlots()
    {
        slots = new List<SlotIcon>();
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            var obj = Instantiate(slotPrefab, slotsContainer);
            var slot = obj.GetComponentInChildren<SlotIcon>();
            slot.SetInventoryView(this);
            slot.SetIndex(i);
            slot.EmptySlot();
            slot.SetItem(null);
            slots.Add(slot);
        }
    }


    public void SelectItem(SlotIcon slot)
    {
        _selectedSlot = slot;
        if(!_itemDescription.gameObject.activeSelf)
            _itemDescription.gameObject.SetActive(true);
        _view.AnimationDescription(()=>_itemDescription.SetItem(_selectedSlot.item));
        
    }
    public Inventory SetInventory(Inventory newInventory)
    {
        _inventory = newInventory;
        UpdateInventory();
        return _inventory;
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

    public void SwapItems(SlotIcon slotA, SlotIcon slotB)
    {
        if (slotA == null || slotB == null)
            return;

        int indexA = slotA.GetIndex();
        int indexB = slotB.GetIndex();
        
        if (indexA < 0 || indexB < 0 || indexA >= _inventory.items.Count || indexB >= _inventory.items.Count)
            return;

        (_inventory.items[indexA], _inventory.items[indexB]) = (_inventory.items[indexB], _inventory.items[indexA]);

        slotA.SetItem(_inventory.items[indexA]);
        slotB.SetItem(_inventory.items[indexB]);
    }

}
