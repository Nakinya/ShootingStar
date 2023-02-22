using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;//�ӵ�ʱ��̶�
    float defaultFixedDeltaTime;
    float t;
    float timeScaleBeforePause;

    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }
    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }
    public void Unpause()
    {
        //Time.timeScale = 1f;
        Time.timeScale = timeScaleBeforePause;//�����ͣ�ص�����״̬ʱ��һ�µ�����
    }
    public void BulletTime(float duration)//����
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }
    public void BulletTime(float inDuration,float outDuration)//���ٺ����
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)//���ٺ󱣳��ټ���
    {
        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, keepingDuration, outDuration));
    }
    public void SlowIn(float duration)//����
    {
        StartCoroutine(SlowInCoroutine(duration));
    }
    public void SlowOut(float duration)//����
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration , float outDuration)//���ٺ���������
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    IEnumerator SlowInKeepAndOutCoroutine(float inDuration, float keepingDuration, float outDuration)//����һ�μ���ʱ��
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    IEnumerator SlowOutCoroutine(float duration)//�ӵ�ʱ�����Э��
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)//���⿪������ģʽʱ������ͣ��Ϸ
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
    IEnumerator SlowInCoroutine(float duration)//�ӵ�ʱ�����Э��
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
}
