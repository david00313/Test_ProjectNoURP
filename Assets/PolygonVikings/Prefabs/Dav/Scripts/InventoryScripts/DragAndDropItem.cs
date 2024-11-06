using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    public InventorySlot oldSlot;
    private Transform player;
    GameObject itemObject;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        oldSlot = transform.GetComponentInParent<InventorySlot>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.9f);
        GetComponentInChildren<Image>().raycastTarget = false;
        transform.SetParent(transform.parent.parent.parent);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        GetComponentInChildren<Image>().raycastTarget = true;
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;

        // Verificăm dacă a fost click dreapta și itemul este consumabil
        if (eventData.button == PointerEventData.InputButton.Right && oldSlot.item != null && oldSlot.item.isConsumeable)
        {
            UseConsumableItem();
        }

        // Muta obiectul dintr-un slot în altul sau îl elimină dacă este aruncat
        if (eventData.pointerCurrentRaycast.gameObject.name == "UIBG")
        {
            itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
            itemObject.GetComponent<Item>().amount = oldSlot.amount;
            NullifySlotData();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent != null && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
        }
        ArmorMiniInventory.instance.CheckArmorSlots();
        MiniInventory.Instance.CheckItemHand();
    }

    private void UseConsumableItem()
    {
        if (oldSlot.item != null && oldSlot.item.isConsumeable)
        {
            // Consumă itemul și crește sănătatea (poți adăuga și alte efecte aici)
            Indicators.healthAmount += oldSlot.item.changeHealth;

            // Dacă sănătatea depășește 100, setează-o la 100
            if (Indicators.healthAmount > 100)
                Indicators.healthAmount = 100;

            // Scade din cantitatea de item
            if (oldSlot.amount <= 1)
            {
                NullifySlotData(); // Golim slotul dacă itemul a fost consumat complet
            }
            else
            {
                oldSlot.amount--;
                oldSlot.itemAmountText.text = oldSlot.amount.ToString();
            }
        }
    }

    public void NullifySlotData()
    {
        oldSlot.item = null;
        oldSlot.amount = 0;
        oldSlot.isEmpty = true;
        oldSlot.iconGO.GetComponent<Image>().color = new Color(100, 100, 100, 0);
        oldSlot.iconGO.GetComponent<Image>().sprite = null;
        oldSlot.itemAmountText.text = "";
    }

    void ExchangeSlotData(InventorySlot newSlot)
    {
        ItemScriptableObject item = newSlot.item;
        int amount = newSlot.amount;
        bool isEmpty = newSlot.isEmpty;
        GameObject iconGO = newSlot.iconGO;
        TMP_Text itemAmountText = newSlot.itemAmountText;

        newSlot.item = oldSlot.item;
        newSlot.amount = oldSlot.amount;
        if (!oldSlot.isEmpty)
        {
            newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
            newSlot.itemAmountText.text = oldSlot.amount.ToString();
        }
        else
        {
            newSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            newSlot.iconGO.GetComponent<Image>().sprite = null;
            newSlot.itemAmountText.text = "";
        }

        newSlot.isEmpty = oldSlot.isEmpty;

        oldSlot.item = item;
        oldSlot.amount = amount;
        if (!isEmpty)
        {
            oldSlot.SetIcon(item.icon);
            oldSlot.itemAmountText.text = amount.ToString();
        }
        else
        {
            oldSlot.iconGO.GetComponent<Image>().color = new Color(123, 21, 111, 0);
            oldSlot.iconGO.GetComponent<Image>().sprite = null;
            oldSlot.itemAmountText.text = "";
        }

        oldSlot.isEmpty = isEmpty;
    }
}
