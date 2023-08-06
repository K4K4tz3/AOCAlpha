using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class PanelUIUpdater : MonoBehaviour
{
    private Image qImg;
    private Image wImg;
    private Image eImg;

    [SerializeField] GameObject qPanel;
    [SerializeField] GameObject wPanel;
    [SerializeField] GameObject ePanel;

    private void Awake()
    {
        qImg = qPanel.GetComponent<Image>();
        wImg = wPanel.GetComponent<Image>();
        eImg = ePanel.GetComponent<Image>();
    }

    public void UpdateQImgFill(float currentValue, float maxValue)
    {
        qImg.fillAmount = currentValue / maxValue;
    }
    public void UpdateWImgFill(float currentValue, float maxValue)
    {
        wImg.fillAmount = currentValue / maxValue;
    }
    public void UpdateEImgFill(float currentValue, float maxValue)
    {
        eImg.fillAmount = currentValue / maxValue;
    }

}
