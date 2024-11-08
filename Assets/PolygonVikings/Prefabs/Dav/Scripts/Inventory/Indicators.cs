using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    public Image healthBar, foodBar, waterBar;
    public static float healthAmount = 60;
    public static float foodAmount = 100;
    public static float waterAmount = 100;

    private float secondsToEmptyFood = 180f;
    private float secondsToEmptyWater = 120f;
    private float secondsToEmptyHealth = 360f;

    private void Start()
    {
        healthBar.fillAmount = healthAmount / 100;
        foodBar.fillAmount = foodAmount / 100;
        waterBar.fillAmount = waterAmount / 100;
    }

    private void Update()
    {

        if(healthAmount > 0)
        {
            VerfiyIndicators();
        }
        else
        {
            PlayerManager.instance.PlayerDied();
        }

    }

    private void VerfiyIndicators()
    {

        if (foodAmount > 0)
        {
            foodAmount -= 100 / secondsToEmptyFood * Time.deltaTime;
            foodBar.fillAmount = foodAmount / 100;
        }

        if (waterAmount > 0)
        {
            waterAmount -= 100 / secondsToEmptyWater * Time.deltaTime;
            waterBar.fillAmount = waterAmount / 100;
        }

        if (foodAmount <= 0)
        {
            healthAmount -= 100 / secondsToEmptyHealth * Time.deltaTime;
        }

        if (waterAmount <= 0)
        {
            healthAmount -= 100 / secondsToEmptyWater * Time.deltaTime;
        }
        healthBar.fillAmount = healthAmount / 100;

    }
}
