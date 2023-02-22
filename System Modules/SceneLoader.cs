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
    [SerializeField] Image transitionImage;//ת��������ͼƬ
    [SerializeField] float fadeTime = 3.5f;//ת������ʱ��
    Color color;//�����޸�ת��ͼƬ����ɫ
    const string GAMEPLAY = "GamePlay";
    const string MAIN_MENU = "MainMenu";
    const string SCORING = "Scoring";
   
    IEnumerator LoadingCoroutine(string sceneName)//���س���
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);//�첽���س�������Ӱ�����߳�,���ص�Asyncoperation�첽��������������жϳ����Ƿ�������
        loadingOperation.allowSceneActivation = false;//���ü��غõĳ����Ƿ�Ϊ����״̬
        transitionImage.gameObject.SetActive(true);
        while (color.a < 1f)//����alpha��ֵʵ�ֵ���
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime/fadeTime);//����������ֵ������0��1֮��
            transitionImage.color = color;
            yield return null;
        }

        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);//���������Ѿ����غõȴ������������������ʱprogress��Ϊ1����ֹ�򳡾�����ʱ���������������
        loadingOperation.allowSceneActivation = true;//���ü��غõĳ����Ƿ�Ϊ����״̬

        while (color.a > 0f)//����alpha��ֵʵ�ֵ���
         {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);//����������ֵ������0��1֮��
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
