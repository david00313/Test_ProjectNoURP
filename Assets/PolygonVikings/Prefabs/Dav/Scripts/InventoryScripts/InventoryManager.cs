using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject UIBG;
    public GameObject PlayerController;
    public GameObject Dot;
    public Transform inventoryPanel;
    private Camera mainCamera;
    public GameObject currentWeapon;


    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;
    private float reachDistance = 4f;
    GameObject pickUp;
    private void Awake()
    {
        UIBG.SetActive(true);
        inventoryPanel.gameObject.SetActive(false);
    }
    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < inventoryPanel.childCount; i++)//nr de copii la obiecte
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)//verificam daca la slot este componentul InventorySlot
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        UIBG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpened = !isOpened;
            if (isOpened)
            {
                Debug.Log("Is opened");
                UIBG.SetActive(true);
                inventoryPanel.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Dot.SetActive(false);
                //PlayerController.GetComponent<Rotate>().enabled = true;
                mainCamera.GetComponent<CameraController>().enabled = false;
            }
            else
            {
                UIBG.SetActive(false);
                inventoryPanel.gameObject.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Dot.SetActive(true);
                mainCamera.GetComponent<CameraController>().enabled = true;

                //PlayerController.GetComponent<Rotate>().enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            if (hit.collider.gameObject.GetComponent<Item>() != null)//verific daca in directia ceea este un Item si il adaug in invent
            {
                if (hit.collider.gameObject.GetComponent<Item>().item.isConsumeable)
                {
                    Debug.Log("Pickedup consumeable item");
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item, hit.collider.gameObject.GetComponent<Item>().amount);

                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.GetComponent<Item>().item.isConsumeable == false)
                {
                    Debug.Log("Pickedup no consumeable item");
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item, hit.collider.gameObject.GetComponent<Item>().amount);
                    Destroy(hit.collider.gameObject);

                }
            }
        }
    }

    private void PickUpWeapon(RaycastHit hit)
    {


        //punem arma in mana
        currentWeapon = hit.transform.gameObject;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        currentWeapon.transform.parent = transform;
        currentWeapon.transform.localPosition = new Vector3(0, 180f, 0f);
        currentWeapon.transform.localEulerAngles = new Vector3(0, 180f, 0f);
        currentWeapon.GetComponent<Collider>().enabled = false;
        // currentWeapon.GetComponent<GunFire>().enabled = true;
        Destroy(hit.collider.gameObject);

    }



    private void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)//trecem prin fiecare slot din List de slots
        {
            if (slot.item == _item) //verific daca item se poate de luat
            {
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount; // daca sa putut lua atunci se adauga in invent
                    slot.itemAmountText.text = slot.amount.ToString();
                    return;
                }
                continue;
            }
        }
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
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
