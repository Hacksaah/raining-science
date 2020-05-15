using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Text ammoInClipText;
    public Text maxAmmoText;

    static AmmoUI instance;
    public static AmmoUI Instance
    {
        get
        {
            if (instance == null)
                new AmmoUI();
            return instance;
        }
    }

    AmmoUI() { instance = this; }

    public void UpdateText(int ammoInClip, int maxAmmo)
    {
        ammoInClipText.text = ammoInClip.ToString();
        if (maxAmmo < 0)
            maxAmmoText.text = "∞";
        else
            maxAmmoText.text = maxAmmo.ToString();
    }
}
