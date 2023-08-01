using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretLTPointsBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateLTPointsBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}
