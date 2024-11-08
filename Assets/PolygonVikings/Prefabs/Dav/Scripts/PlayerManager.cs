using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject axeAnimInit;
    [SerializeField] private GameObject indicatorsGo;
    public GameObject playerDiedUi;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Invoke("AxeAnimInitDelay", 0.5f);

    }

    public void ActivatePlayerInput()
    {
        player.GetComponent<vMeleeCombatInput>().weakAttackInput.useInput = true;
        player.GetComponent<vMeleeCombatInput>().weakAttackInput.useInput = true;
        player.GetComponent<vMeleeCombatInput>().blockInput.useInput = true;
    }
    public void AxeAnimInitDelay()
    {
        player.GetComponent<vMeleeCombatInput>().weakAttackInput.useInput = false;
        player.GetComponent<vMeleeCombatInput>().weakAttackInput.useInput = false;
        player.GetComponent<vMeleeCombatInput>().blockInput.useInput = false;
    }
    public void PlayerDied()
    {
        player.GetComponent<vThirdPersonController>().ChangeHealth(0);
        PlayerDiedUi(true);
    }

    public void PlayerDiedUi(bool isActive)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerDiedUi.SetActive(isActive);
        indicatorsGo.SetActive(!isActive);

    }

}
