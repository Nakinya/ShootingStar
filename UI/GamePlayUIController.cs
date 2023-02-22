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
        
        //��������������
        //resumeButton.onClick.AddListener(OnResumeButtonClick);//��Ӽ���
        //optionsButton.onClick.AddListener(OnOptionsButtonClick);//��Ӽ���
        //mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);//��Ӽ���
        //������
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
        ButtonPressedBehaviour.buttonFunctionTable.Clear();//�л�����ʱ������ܺ���
    }
    void Pause()
    {
        //Time.timeScale = 0f;//����ҿ�ʼ����ģʽʱ��ͣ����Ϸ������ͣ
        hUDCanvas.enabled = false;//�ر�hud����
        menuCanvas.enabled = true;//����menu����
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();//�л�����̬����ģʽ����time.deltaTime = 0ʱ��InputSystem�Ĺ̶�����ģʽ��ֹͣ���£��Ῠ�ڲ˵�ҳ��
        UIInput.Instance.SelectUI(resumeButton);//Ĭ�ϴ򿪲˵�ʱѡ��resumeButton
        AudioManager.Instance.PlaySFX(pauseSFX);
    }
    void Unpause()
    {
        resumeButton.Select();//��ѡ���ٲ��Ŷ���
        resumeButton.animator.SetTrigger("Pressed");
        //OnResumeButtonClick(); ��������������
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
        //����mainMenu����
        SceneLoader.Instance.LoadMainMenuScene();
    }

}
