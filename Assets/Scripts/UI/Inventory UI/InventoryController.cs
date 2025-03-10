using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [FormerlySerializedAs("_isOpen")] public bool isOpen;
    
    private SlotIcon _selectedSlot;
    
    private Inventory _inventory;
    private Player _player;
    private ItemDatabase _itemDatabase;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _itemDatabase = Resources.Load<ItemDatabase>("Database");
        if (_itemDatabase == null)
            Debug.LogWarning("ItemDatabase not found");
        _inventory = new Inventory(MAX_SLOTS, 0); 
        _view = GetComponent<InventoryView>();
        FillEmptySlots();
        _player = FindObjectOfType<Player>();
        _itemDescription.useButton.onClick.AddListener(() =>
        {
            Close();
            _player.UseItem(_selectedSlot.item);
        });
        _itemDescription.dropButton.onClick.AddListener(() =>
        {
            CloseDescription();
            RemoveItemFromInventory(_selectedSlot.item);
        });
    }

    private void OnDisable()
    {
        _itemDescription.useButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        _itemDescription.useButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isOpen || Menu.isMenuOpened)
            Close();
    }

    public void CloseDescription() => _view.AnimationDescriptionClose(() => _itemDescription.SetItem(_selectedSlot.item));

    public Sprite GetSpriteByID(int id)
    {
        if (_itemDatabase == null)
            return null;
        return _itemDatabase.GetItemSOById(id).sprite;
    }
    
    public void Open()
    {
        _view.OpenInventory(() => UpdateManager.SetPause(true));
        isOpen = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Close()
    {
        if(!isOpen)
            return;
        _view.CloseInventory(() =>
        {
            if (!Menu.isMenuOpened)
                UpdateManager.SetPause(false);
        });
        isOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FillEmptySlots()
    {
        slots = new List<SlotIcon>();
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            var obj = Instantiate(slotPrefab, slotsContainer);
            var slot = obj.GetComponentInChildren<SlotIcon>();
            slot.SetItem(_inventory.items[i]);
            slot.SetInventoryController(this);
            slot.SetIndex(i);
            slot.EmptySlot();
            slots.Add(slot);
        }
    }

    public void SelectItem(SlotIcon slot)
    {
        _selectedSlot = slot;
        if(!_itemDescription.gameObject.activeSelf)
            _itemDescription.gameObject.SetActive(true);
        _view.AnimationDescriptionOpen(()=>_itemDescription.SetItem(_selectedSlot.item));
        
    }

    public void AddItemToInventory(Item item)
    {
        if (_inventory == null)
            return;

        _inventory.AddItem(item);
        UpdateInventory();
    }

    public void RemoveItemFromInventory(Item item)
    {
        if (_inventory == null)
            return;
        _inventory.RemoveItem(item.id, 1);
        UpdateInventory();
    }
    public Inventory GetInventory() => _inventory;
 
    public void SetInventory(Inventory newInventory)
    {
        _inventory = newInventory;
        UpdateInventory();
    }
    
    private void UpdateInventory()
    {
        if (_inventory == null || slots == null) return;

        foreach (var slot in slots.Where(x=> x == null))
            slot.EmptySlot();

        for (int i = 0; i < _inventory.items.Count; i++)
            if (_inventory.items[i] != null && i < MAX_SLOTS)
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
