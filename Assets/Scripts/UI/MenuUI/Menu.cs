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
    private bool _isOpen;
    [SerializeField] 
    private GameObject _menuButtons;
    [SerializeField] 
    private GameObject _menuBackground;
    private void Start()
    {
        _inventorySave = new InventorySave();
        _menuView = GetComponent<MenuView>();
        _isOpen = false;
        Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isOpen)
                Close();
            else
                Open();
        }
    }

    private void Open()
    {
        _menuButtons.SetActive(true);
        _menuBackground.SetActive(true);
        _menuView.MoveButtonsIn();
        _menuView.FadeBackgroundIn();
        _isOpen = true;
        Cursor.lockState = CursorLockMode.None;
        UpdateManager.SetPause(true);
    }

    private void Close()
    {
        _menuView.MoveButtonsOut(()=> _menuButtons.SetActive(false));
        _menuView.FadeBackgroundOut(()=> _menuBackground.SetActive(false));
        _isOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        UpdateManager.SetPause(false);
    }
    
    public void Save() => _inventorySave.SaveInventory();
    
    public void Load() => _inventorySave.LoadInventory();
    
    public void Exit() => Application.Quit();
}
