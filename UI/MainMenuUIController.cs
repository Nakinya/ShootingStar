using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class MainMenuUIController : MonoBehaviour
{
    [Header("-----Canvas-----")]
    [SerializeField] Canvas mainMenuCanvas;

    [Header("-----Button-----")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonQuit;

    private void Start()
    {
        Time.timeScale = 1f;//�������ͣ�˵��ص����˵�����Ҫ��timeScale��Ϊ1
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }
    private void OnEnable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClicked);//ע�ᰴť���ܱ�(������)
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClicked);
    }
    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }

    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;//��ʼ��Ϸʱ�����˵��ر�
        SceneLoader.Instance.LoadGamePlayScene();
    }
    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }
    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();//ֻ��Build�����Ч
#endif
    }
}
