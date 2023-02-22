using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class GamePlayUIController : MonoBehaviour
{
    [Header("-----Player Input-----")]
    [SerializeField] PlayerInput playerInput;

    [Header("-----Canvas-----")]
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menuCanvas;

    [Header("-----Button-----")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mainMenuButton;

    [Header("-----Audio Data-----")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unpauseSFX;

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;
        
        //交给动画机管理
        //resumeButton.onClick.AddListener(OnResumeButtonClick);//添加监听
        //optionsButton.onClick.AddListener(OnOptionsButtonClick);//添加监听
        //mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);//添加监听
        //动画机
        ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }
    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        //resumeButton.onClick?.RemoveAllListeners();
        //optionsButton.onClick?.RemoveAllListeners();
        //mainMenuButton.onClick?.RemoveAllListeners();
        ButtonPressedBehaviour.buttonFunctionTable.Clear();//切换场景时清除功能函数
    }
    void Pause()
    {
        //Time.timeScale = 0f;//当玩家开始爆发模式时暂停，游戏不会暂停
        hUDCanvas.enabled = false;//关闭hud画布
        menuCanvas.enabled = true;//开启menu画布
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();//切换到动态更新模式，当time.deltaTime = 0时，InputSystem的固定更新模式会停止更新，会卡在菜单页面
        UIInput.Instance.SelectUI(resumeButton);//默认打开菜单时选中resumeButton
        AudioManager.Instance.PlaySFX(pauseSFX);
    }
    void Unpause()
    {
        resumeButton.Select();//先选中再播放动画
        resumeButton.animator.SetTrigger("Pressed");
        //OnResumeButtonClick(); 交给动画机管理
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }

    void OnResumeButtonClick()
    {
        //Time.timeScale = 1f;
        hUDCanvas.enabled = true;
        menuCanvas.enabled = false;
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.Unpause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionsButtonClick()
    {
        // TODO
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }
    
    void OnMainMenuButtonClick()
    {
        menuCanvas.enabled = false;
        //加载mainMenu场景
        SceneLoader.Instance.LoadMainMenuScene();
    }

}
