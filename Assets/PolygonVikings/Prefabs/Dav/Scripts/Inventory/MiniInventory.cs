using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniInventory : MonoBehaviour
{
    public static MiniInventory Instance;
    public Transform[] miniInventorySlots; // Sloturile miniinventarului
    public Transform handPosition; // Poziția în care se va ține obiectul în mână
    public InventorySlot[] mainInventorySlots; // Sloturile inventarului principal
    public GameObject currentItemInHand; // Referință pentru obiectul din mână

    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        // Verifică tastele 1, 2, 3 pentru a activa sloturile din miniinventar
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateMiniInventorySlot(0); // Activare slot 1
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateMiniInventorySlot(1); // Activare slot 2
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateMiniInventorySlot(2); // Activare slot 3
        }
    }

    // Activează un slot din miniinventar și plasează obiectul în mână
    void ActivateMiniInventorySlot(int slotIndex)
    {
        var slot = miniInventorySlots[slotIndex].GetComponent<InventorySlot>();

        // Verifică dacă slotul respectiv conține un item
        if (slot.item != null)
        {
            // Dacă există un obiect în mână, îl distrugem (dacă există)
            if (currentItemInHand != null)
            {
                Destroy(currentItemInHand);
            }

            // Creează obiectul din miniinventar și îl plasează în mână
            currentItemInHand = Instantiate(slot.item.itemPrefab, handPosition.position, slot.item.itemPrefab.transform.rotation);
            currentItemInHand.transform.SetParent(handPosition); // Poziționează-l în mână
            currentItemInHand.transform.localRotation = slot.item.itemPrefab.transform.localRotation;

            PlayerManager.instance.ActivatePlayerInput();
        }
        else
        {
            // Dacă slotul este gol, asigură-te că obiectul din mână este eliminat
            if (currentItemInHand != null)
            {
                Destroy(currentItemInHand);
                currentItemInHand = null;
                PlayerManager.instance.AxeAnimInitDelay();

            }
        }
    }
    public void CheckItemHand()
    {
        if (currentItemInHand != null)
        {
            Destroy(currentItemInHand);
            currentItemInHand = null;
        }
    }
}
