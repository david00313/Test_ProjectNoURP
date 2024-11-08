using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorMiniInventory : MonoBehaviour
{
    public static ArmorMiniInventory instance;
    // Sloturi pentru fiecare piesă de armură
    public InventorySlot helmetSlot; // Slot pentru Helmet
    public InventorySlot chestplateSlot; // Slot pentru Chestplate
    public InventorySlot pantsSlot; // Slot pentru Pants

    public InventorySlot[] mainInventorySlots; // Sloturile inventarului principal
    public GameObject player;  // Referința la obiectul personajului

    // Referințele la locațiile armurii pe personaj
    public Transform helmetPosition; // Locația pentru helmet
    public Transform chestplatePosition; // Locația pentru chestplate
    public Transform pantsPosition; // Locația pentru pantaloni

    // Referințele la armura echipată (pentru a o distruge dacă este scoasă)
    private GameObject equippedHelmet;
    private GameObject equippedChestplate;
    private GameObject equippedPants;

    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
        // Verifică dacă un item este plasat într-un slot de armură și echipează-l pe personaj
        //CheckArmorSlots();
    }

    public void CheckArmorSlots()
    {
        Debug.Log("Verific");
        // Verificăm sloturile pentru armură și schimbăm echipamentele pe personaj
        EquipArmorToSlot(helmetSlot, ArmorType.Helmet, helmetPosition, ref equippedHelmet);
        EquipArmorToSlot(chestplateSlot, ArmorType.Chestplate, chestplatePosition, ref equippedChestplate);
        EquipArmorToSlot(pantsSlot, ArmorType.Pants, pantsPosition, ref equippedPants);
    }

    // Funcția care echipează armura pe personaj în funcție de tipul de slot
    void EquipArmorToSlot(InventorySlot armorSlot, ArmorType expectedType, Transform equipPosition, ref GameObject equippedArmor)
    {
        if (armorSlot.item != null && armorSlot.item.itemType == ItemType.Armor && armorSlot.item.armorType == expectedType)
        {
            // Dacă armura nu este deja echipată, o instanțiem
            if (equippedArmor == null || equippedArmor.name != armorSlot.item.itemName)
            {
                Destroy(equippedArmor);
                equippedArmor = null;  // Setăm referința la null, pentru a indica că nu mai există armură echipată

                equippedArmor = Instantiate(armorSlot.item.itemPrefab, equipPosition.position, equipPosition.rotation); // Utilizăm rotația locației
                equippedArmor.transform.SetParent(equipPosition); // Poziționează armura sub locația corectă
                equippedArmor.transform.localPosition = Vector3.zero;  // Trebu să poziționezi armura corect sub locația părintelui
                equippedArmor.GetComponent<BoxCollider>().enabled = false;
                equippedArmor.GetComponent<Rigidbody>().isKinematic = true;
                equippedArmor.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        else
        {
            // Dacă nu există itemul corespunzător, distrugem armura echipată
            if (equippedArmor != null)
            {
                Destroy(equippedArmor);
                equippedArmor = null;  // Setăm referința la null, pentru a indica că nu mai există armură echipată
            }
        }
    }

    // Funcție pentru a adăuga iteme din inventar în sloturile corecte
    public void OnItemDropped(ItemScriptableObject item, InventorySlot targetSlot)
    {
        if (item != null && item.itemType == ItemType.Armor)
        {
            // Verificăm dacă itemul este de tipul corect pentru slotul țintă
            switch (item.armorType)
            {
                case ArmorType.Helmet:
                    if (targetSlot == helmetSlot)
                    {
                        helmetSlot.item = item;
                        EquipArmorToSlot(helmetSlot, ArmorType.Helmet, helmetPosition, ref equippedHelmet);  // Echipează automat
                    }
                    break;
                case ArmorType.Chestplate:
                    if (targetSlot == chestplateSlot)
                    {
                        chestplateSlot.item = item;
                        EquipArmorToSlot(chestplateSlot, ArmorType.Chestplate, chestplatePosition, ref equippedChestplate);  // Echipează automat
                    }
                    break;
                case ArmorType.Pants:
                    if (targetSlot == pantsSlot)
                    {
                        pantsSlot.item = item;
                        EquipArmorToSlot(pantsSlot, ArmorType.Pants, pantsPosition, ref equippedPants);  // Echipează automat
                    }
                    break;
                default:
                    Debug.Log("Itemul nu se potrivește cu niciun slot.");
                    break;
            }
        }
    }

}
