using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text itemAmountText;

    private void Start()
    {
        iconGO = transform.GetChild(0).GetChild(0).gameObject;//ne adresam la child 0 (adica Icon) din Dragobject(adica 0) si de acolo avem nevoie de tot gameObject
        itemAmountText = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();//fix asa doar ca child 1 si alegem componentul TMP_Text

    }
    public void ClearSlot()
    {
        // Resetează itemul din acest slot
        item = null;
        amount = 0;
        itemAmountText.text = ""; // Actualizează textul cantității
                                  // Dacă ai alte acțiuni specifice, le poți adăuga aici
    }

    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);//culoarea alba default
        iconGO.GetComponent<Image>().sprite = icon;
    }
}
