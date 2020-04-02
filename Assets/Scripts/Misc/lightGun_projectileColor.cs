using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightGun_projectileColor
{
    private static lightGun_projectileColor instance;
    private lightGun_projectileColor()
    {
        if (instance != null)
        {
            return;
        }
        colors = new Color[12];
        colors[0] = new Color(1, 0.65f, 0.65f, 0.15f);
        colors[1] = new Color(1, 0, 0.0342f);
        colors[2] = new Color(1, 0.71f, 0.59f, 0.15f);
        colors[3] = new Color(1, 0.3f, 0);
        colors[4] = new Color(1, 1, 0.85f, 0.15f);
        colors[5] = new Color(1, 1, 0);
        colors[6] = new Color(0.75f, 0.9f, 0.75f, 0.15f);
        colors[7] = new Color(0.121f, 0.83f, 0.135f);
        colors[8] = new Color(0.64f, 0.73f, 1, 0.15f);
        colors[9] = new Color(0.163f, 0.38f, 1);
        colors[10] = new Color(0.9f, 0.68f,1f, 0.15f);
        colors[11] = new Color(0.546f, 0.051f, 0.766f);
        index = -1;

        instance = this;
    }
    public static lightGun_projectileColor Instance
    {
        get
        {
            if (instance == null)
            {
                new lightGun_projectileColor();
            }
            return instance;
        }
    }

    private int index;
    private Color[] colors;

    public Color GetColor()
    {
        index++;
        if (index == 12)
            index = 0;
        return colors[index];
    }
}
