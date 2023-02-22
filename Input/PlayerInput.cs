using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

///<summary>
///
///</summary>
[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions,InputActions.IPauseMenuActions,InputActions.IGameOverScreenActions
{
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };
    public event UnityAction onDodge = delegate { };
    public event UnityAction onOverdrive = delegate { };
    public event UnityAction onPause = delegate { };
    public event UnityAction onUnpause = delegate { };
    public event UnityAction onLaunchMissile = delegate { };
    public event UnityAction onConfirmGameOver = delegate { };
    InputActions inputActions;
    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlay.SetCallbacks(this);//�Ǽǻص�����
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }
    private void OnDisable()
    {
        DisableAllInputs();
    }

    void SwitchActionMap(InputActionMap actionMap, bool IsUIIput)
    {
        inputActions.Disable();//����InputActions�����ж�����
        actionMap.Enable();//����Ŀ�궯����

        if (IsUIIput)//�����ui���������ù��
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;//�������
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //�л�InputSystem����ģʽ����̬����ģʽ��FixedUpdate(�̶�)����ģʽ�ܵ�time.deltaTime��Ӱ��
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInputs() => inputActions.Disable();//������������
    public void EnableGameplayInput() => SwitchActionMap(inputActions.GamePlay, false);//����Gameplay������
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);//����PauseMenu������
    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, false);//����GameOverScreen������

    public void OnMove(InputAction.CallbackContext context)
    {
                     
        if (context.phase == InputActionPhase.Performed)//��������յ��ź��ǰ��£�������ס,�൱��input.getkey
        {
            if (onMove != null)
                onMove.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled)//����������Ϊ�ж�����
        {
            if (onStopMove != null)
                onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)//��������յ��ź��ǰ��£�������ס,�൱��input.getkey
        {
            onFire?.Invoke();
        }
        if (context.canceled)
        {
            onStopFire?.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge?.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause?.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnpause?.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissile?.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver?.Invoke();
        }
    }
}
