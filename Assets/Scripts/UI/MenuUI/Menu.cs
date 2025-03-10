using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Button SaveButton;
    [SerializeField]
    public Button LoadButton;
    [SerializeField]
    public Button ExitButton;
    
    private InventorySave _inventorySave;
    private MenuView _menuView;
    public static bool isMenuOpened;
    [SerializeField] 
    private GameObject _menuButtons;
    [SerializeField] 
    private GameObject _menuBackground;
    private void Start()
    {
        _inventorySave = new InventorySave();
        _menuView = GetComponent<MenuView>();
        isMenuOpened = false;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpened)
                Close();
            else
                Open();
        }
    }

    private void Open()
    {
        InventoryController.Instance.Close();
        _menuButtons.SetActive(true);
        _menuBackground.SetActive(true);
        _menuView.MoveButtonsIn(()=>UpdateManager.SetPause(true));
        _menuView.FadeBackgroundIn();
        isMenuOpened = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Close()
    {
        _menuView.MoveButtonsOut(()=> _menuButtons.SetActive(false));
        _menuView.FadeBackgroundOut(()=> _menuBackground.SetActive(false));
        isMenuOpened = false;
        Cursor.lockState = CursorLockMode.Locked;
        UpdateManager.SetPause(false);
    }
    
    public void Save() => _inventorySave.SaveInventory();
    
    public void Load() => _inventorySave.LoadInventory();
    
    public void Exit() => Application.Quit();
}
