using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBarUI : MonoBehaviour
{
    public Slider rageBar;

    public void SetStartRage()
    {
        rageBar.value = 0;
    }

    public void SetRage(float RageXP)
    {
        rageBar.value = RageXP;
    }
}
