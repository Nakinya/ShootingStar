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
        Time.timeScale = 1f;//如果是暂停菜单回到主菜单，则要把timeScale设为1
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }
    private void OnEnable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClicked);//注册按钮功能表(动画机)
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClicked);
    }
    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }

    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;//开始游戏时将主菜单关闭
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
        Application.Quit();//只有Build后才有效
#endif
    }
}
