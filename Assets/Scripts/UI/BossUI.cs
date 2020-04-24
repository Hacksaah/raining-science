using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public VarInt bossMaxHP;
    public VarInt bossCurrHP;

    public static BossUI Instance;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ReadyUI()
    {
        slider.maxValue = bossMaxHP.value;
        slider.value = bossCurrHP.value;
        gameObject.SetActive(true);
    }

    public void UpdateHealthBar()
    {
        slider.value = bossCurrHP.value;
        if (bossCurrHP.value <= 0)
            gameObject.SetActive(false);
    }
}
