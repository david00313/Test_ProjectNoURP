using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
/// IPointerDownHandler - ������ �� ��������� ����� �� ������� �� ������� ����� ���� ������
/// IPointerUpHandler - ������ �� ����������� ����� �� ������� �� ������� ����� ���� ������
/// IDragHandler - ������ �� ��� �� ����� �� �� ������� ����� �� �������
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
        //��������� ��� "PLAYER" �� ������� ���������!
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // ������� ������ InventorySlot � ����� � ��������
        oldSlot = transform.GetComponentInParent<InventorySlot>();

    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On drag");
        // Daca slotul va fi gol nu facem ceea ce e mai jos de return
        if (oldSlot.isEmpty)
            return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        if (oldSlot.isEmpty)
            return;
        //������ �������� ����������
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.9f);
        // ������ ��� ����� ������� ������ �� ������������ ��� ��������
        GetComponentInChildren<Image>().raycastTarget = false;
        // ������ ��� DraggableObject �������� InventoryPanel ����� DraggableObject ��� ��� ������� ������� ���������
        transform.SetParent(transform.parent.parent.parent);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        if (oldSlot.isEmpty)
            return;

        // � ����� ����� ����� ����� �� ������
        GetComponentInChildren<Image>().raycastTarget = true;

        //��������� DraggableObject ������� � ���� ������ ����
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
        //���� ����� �������� ��� �������� �� ����� UIPanel, ��...
        if (eventData.pointerCurrentRaycast.gameObject.name == "UIBG") // renamed to UIBG
        {
            // ������ �������� �� ��������� - ������� ������ ������ ����� ����������
            itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);

            // ������������� ���������� �������� ����� ����� ���� � �����
            itemObject.GetComponent<Item>().amount = oldSlot.amount;

            // ������� �������� InventorySlot

            NullifySlotData();

        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent == null)
        {
            return;
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            //���������� ������ �� ������ ����� � ������
            ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
        }

    }
    public void NullifySlotData() // made public 
    {
        Debug.Log("NullifySlotData");

        // ������� �������� InventorySlot
        oldSlot.item = null;
        oldSlot.amount = 0;
        oldSlot.isEmpty = true;
        oldSlot.iconGO.GetComponent<Image>().color = new Color(100, 100, 100, 0);
        oldSlot.iconGO.GetComponent<Image>().sprite = null;
        oldSlot.itemAmountText.text = "";

    }


    void ExchangeSlotData(InventorySlot newSlot)
    {
        Debug.Log("ExchangeSlotData"+ newSlot);

        // �������� ������ ������ newSlot � ��������� ����������
        ItemScriptableObject item = newSlot.item;
        int amount = newSlot.amount;
        bool isEmpty = newSlot.isEmpty;
        GameObject iconGO = newSlot.iconGO;
        TMP_Text itemAmountText = newSlot.itemAmountText;

        // �������� �������� newSlot �� �������� oldSlot
        newSlot.item = oldSlot.item;
        newSlot.amount = oldSlot.amount;
        if (oldSlot.isEmpty == false)
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

        // �������� �������� oldSlot �� �������� newSlot ����������� � ����������
        oldSlot.item = item;
        oldSlot.amount = amount;
        if (isEmpty == false)
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
