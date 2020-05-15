using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public VarInt BossCount;

    static BossUI instance;
    public static BossUI Instance
    {
        get
        {
            if (instance == null)
                new BossUI();
            return instance;
        }
    }

    Slider slider;
    Text bossCount_text;
    GameObject bossCount_Panel;

    BossUI() { instance = this; }

    private void Awake()
    {
        slider = transform.GetChild(1).GetComponent<Slider>();
        bossCount_Panel = transform.GetChild(0).gameObject;
        bossCount_text = bossCount_Panel.transform.GetChild(1).GetComponent<Text>();
        slider.gameObject.SetActive(false);
    }

    private void Start()
    {
        new WaitForEndOfFrame();
        UpdateBossCount();
    }

    public void ReadyHealthBar(int maxHp)
    {
        slider.maxValue = maxHp;
        slider.value = maxHp;
        slider.gameObject.SetActive(true);

        bossCount_Panel.SetActive(false);
    }

    public void UpdateHealthBar(int incHp)
    {
        slider.value = incHp;
        if (incHp <= 0)
        {
            slider.gameObject.SetActive(false);
            if (BossCount.value > 0)
            {
                UpdateBossCount();
                bossCount_Panel.SetActive(true);
            }                
        }        
    }

    public void UpdateBossCount()
    {
        bossCount_text.text = BossCount.value.ToString();
    }
}
