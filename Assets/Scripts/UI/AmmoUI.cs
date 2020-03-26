using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public VarInt ammoInClip;
    public VarInt maxAmmo;

    public Text ammoInClipText;
    public Text maxAmmoText;

    public void UpdateText()
    {
        ammoInClipText.text = ammoInClip.value.ToString();
        if (maxAmmo.value < 0)
            maxAmmoText.text = "∞";
        else
            maxAmmoText.text = maxAmmo.value.ToString();
    }
}
