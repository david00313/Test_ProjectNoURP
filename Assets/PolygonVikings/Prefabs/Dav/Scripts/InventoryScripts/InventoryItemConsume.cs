using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class InventoryItemConsume : MonoBehaviour, IPointerClickHandler
{
    public InventoryManager inventoryManager;
    public InventorySlot inventorySlot; // referință către slotul în care se află itemul
    public TMP_Text healthText;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Verificăm dacă a fost un click dreapta
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Verificăm dacă itemul este consumabil și nu este în inventar
            if (inventorySlot.item != null && inventorySlot.item.isConsumeable)
            {
                UseItem();
            }
        }
    }

    private void UseItem()
    {
        // Modificăm indicatorul de sănătate
        if (Indicators.healthAmount + inventorySlot.item.changeHealth <= 100)
        {
            Indicators.healthAmount += inventorySlot.item.changeHealth; // creștem sănătatea
        }
        else
        {
            Indicators.healthAmount = 100; // setăm sănătatea la maxim
        }

        // Dacă itemul este consumat complet, golim slotul
        if (inventorySlot.amount <= 1)
        {
            inventorySlot.ClearSlot(); // Golim slotul
        }
        else
        {
            inventorySlot.amount--;
            inventorySlot.itemAmountText.text = inventorySlot.amount.ToString(); // actualizăm cantitatea
        }

        // Actualizăm textul de sănătate
        healthText.text = "Health: " + Indicators.healthAmount;
    }

}
