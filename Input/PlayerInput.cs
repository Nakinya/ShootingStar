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
        inputActions.GamePlay.SetCallbacks(this);//登记回调函数
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }
    private void OnDisable()
    {
        DisableAllInputs();
    }

    void SwitchActionMap(InputActionMap actionMap, bool IsUIIput)
    {
        inputActions.Disable();//禁用InputActions里所有动作表
        actionMap.Enable();//启用目标动作表

        if (IsUIIput)//如果是ui输入则启用光标
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;//隐藏鼠标
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //切换InputSystem更新模式到动态更新模式，FixedUpdate(固定)更新模式受到time.deltaTime的影响
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInputs() => inputActions.Disable();//禁用所有输入
    public void EnableGameplayInput() => SwitchActionMap(inputActions.GamePlay, false);//启动Gameplay动作表
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);//启动PauseMenu动作表
    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, false);//启动GameOverScreen动作表

    public void OnMove(InputAction.CallbackContext context)
    {
                     
        if (context.phase == InputActionPhase.Performed)//动作表接收的信号是按下（包括按住,相当于input.getkey
        {
            if (onMove != null)
                onMove.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled)//用属性来作为判断条件
        {
            if (onStopMove != null)
                onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)//动作表接收的信号是按下（包括按住,相当于input.getkey
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
