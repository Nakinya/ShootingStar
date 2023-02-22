using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class SceneLoader : PersistanSingleton<SceneLoader>
{
    [SerializeField] Image transitionImage;//转场过渡用图片
    [SerializeField] float fadeTime = 3.5f;//转场过渡时间
    Color color;//用于修改转场图片的颜色
    const string GAMEPLAY = "GamePlay";
    const string MAIN_MENU = "MainMenu";
    const string SCORING = "Scoring";
   
    IEnumerator LoadingCoroutine(string sceneName)//加载场景
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);//异步加载场景而不影响主线程,返回的Asyncoperation异步操作类可以用来判断场景是否加载完成
        loadingOperation.allowSceneActivation = false;//设置加载好的场景是否为激活状态
        transitionImage.gameObject.SetActive(true);
        while (color.a < 1f)//更改alpha的值实现淡出
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime/fadeTime);//将浮点数的值限制在0和1之间
            transitionImage.color = color;
            yield return null;
        }

        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);//表明场景已经加载好等待被激活，当场景被激活时progress才为1，防止因场景加载时间过长而导致问题
        loadingOperation.allowSceneActivation = true;//设置加载好的场景是否为激活状态

        while (color.a > 0f)//更改alpha的值实现淡入
         {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);//将浮点数的值限制在0和1之间
            transitionImage.color = color;
            yield return null;
         }
        transitionImage.gameObject.SetActive(false);
    }

    internal void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }

    public void LoadGamePlayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENU));
    }
}
