using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject UIBG;
    public GameObject PlayerController;
    public GameObject Dot;
    public Transform globalInventoryPanel;
    public Transform inventoryPanel;
    private Camera mainCamera;
    public GameObject mainCameraGo;
    public GameObject inventoryCamera;
    public GameObject currentWeapon;
    public GameObject _vMeleeCombatInput;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;
    private float reachDistance = 7f;
    GameObject pickUp;


    private void Awake()
    {
        UIBG.SetActive(true);
        inventoryPanel.gameObject.SetActive(false);
        globalInventoryPanel.gameObject.SetActive(false);
    }

    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        UIBG.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpened = !isOpened;
            if (isOpened)
            {
                mainCameraGo.SetActive(false);
                inventoryCamera.SetActive(true);
                UIBG.SetActive(true);
                globalInventoryPanel.gameObject.SetActive(true);
                _vMeleeCombatInput.GetComponent<vMeleeCombatInput>().enabled = false;

                inventoryPanel.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Dot.SetActive(false);
                //mainCamera.GetComponent<CameraController>().enabled = false;
            }
            else
            {
                mainCameraGo.SetActive(true);
                inventoryCamera.SetActive(false);
                UIBG.SetActive(false);
                globalInventoryPanel.gameObject.SetActive(false);
                _vMeleeCombatInput.GetComponent<vMeleeCombatInput>().enabled = true;
                inventoryPanel.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Dot.SetActive(true);
                //mainCamera.GetComponent<CameraController>().enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    // Metoda pentru a verifica dacă inventarul este plin
    private bool IsInventoryFull()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty) // Dacă există un slot gol, inventarul nu este plin
            {
                return false;
            }
        }
        return true; // Toate sloturile sunt ocupate
    }

    public void PickUp()
    {
        if (IsInventoryFull())
        {
            Debug.Log("Inventarul este plin! Nu mai poți lua obiecte.");
            return; // Oprește procesul de adăugare a obiectului
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            Item itemComponent = hit.collider.gameObject.GetComponent<Item>();
            if (itemComponent != null)
            {
                AddItem(itemComponent.item, itemComponent.amount);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmountText.text = _amount.ToString();
                break;
            }
        }
    }
}
