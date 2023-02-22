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
    [SerializeField] bool willDelayFill = true;//�Ƿ��ӳ����
    [SerializeField] float delayFillTime = 0.5f;//�ӳ����ʱ��
    [SerializeField] float fillSpeed = 0.1f;//����ٶ�
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
        //״̬����ʱ��ǰ��ͼƬ�����ֵ=Ŀ�����ֵ������ͼƬ�����ֵ��������
        if (currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
            bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageBack));
        }
        //״̬����ʱ������ͼƬ�����ֵ=Ŀ�����ֵ��ǰ��ͼƬ�����ֵ��������
        else if (currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageFront));
        }
    }

    protected virtual IEnumerator BufferFillingCoroutine(Image image)//�������Э��
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
