using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class StatsBar_HUD : StatsBar
{
    [SerializeField] protected Text percentText;
    protected virtual void SetPercentText()
    {
        //percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";//返回最接近的int
        percentText.text = targetFillAmount.ToString("P0");
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }

    protected override IEnumerator BufferFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BufferFillingCoroutine(image);
    }
}
