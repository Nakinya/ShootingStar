using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class StatsBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    [SerializeField] bool willDelayFill = true;//是否延迟填充
    [SerializeField] float delayFillTime = 0.5f;//延迟填充时间
    [SerializeField] float fillSpeed = 0.1f;//填充速度
    float currentFillAmount;
    protected float targetFillAmount;
    float t;
    WaitForSeconds waitForDelayFill;
    Coroutine bufferFillingCoroutine;
    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(delayFillTime);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStats(float currentValue,float maxValue)
    {
        targetFillAmount = currentValue / maxValue;
        if (bufferFillingCoroutine != null)
        {
            StopCoroutine(bufferFillingCoroutine);
        }
        //状态减少时，前面图片的填充值=目标填充值，后面图片的填充值慢慢减少
        if (currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
            bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageBack));
        }
        //状态增加时，后面图片的填充值=目标填充值，前面图片的填充值慢慢增加
        else if (currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageFront));
        }
    }

    protected virtual IEnumerator BufferFillingCoroutine(Image image)//缓冲填充协程
    {
        if (willDelayFill)
        {
            yield return waitForDelayFill;
        }
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}
