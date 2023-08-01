using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class w_HeragzonChardbar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateChardbar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}
