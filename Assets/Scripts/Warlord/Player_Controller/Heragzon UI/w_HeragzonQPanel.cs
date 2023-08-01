using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class w_HeragzonQPanel : MonoBehaviour
{
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void UpdateImageFill(float currentValue, float maxValue)
    {
        img.fillAmount = currentValue / maxValue;
    }

}
