using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;//子弹时间刻度
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
        Time.timeScale = timeScaleBeforePause;//解决暂停回到爆发状态时顿一下的问题
    }
    public void BulletTime(float duration)//减速
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }
    public void BulletTime(float inDuration,float outDuration)//减速后加速
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)//减速后保持再加速
    {
        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, keepingDuration, outDuration));
    }
    public void SlowIn(float duration)//减速
    {
        StartCoroutine(SlowInCoroutine(duration));
    }
    public void SlowOut(float duration)//加速
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration , float outDuration)//减速后立即加速
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    IEnumerator SlowInKeepAndOutCoroutine(float inDuration, float keepingDuration, float outDuration)//保持一段减速时间
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    IEnumerator SlowOutCoroutine(float duration)//子弹时间加速协程
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)//避免开启爆发模式时不能暂停游戏
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
    IEnumerator SlowInCoroutine(float duration)//子弹时间减速协程
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
