using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

///<summary>
///
///</summary>
public class UIInput : Singleton<UIInput>
{
    [SerializeField] PlayerInput playerInput;
    InputSystemUIInputModule UIInputModule;//EventSystem的组件

    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)//selectable类是所有可选中Unity UI类的基类
    {
        UIObject.Select();//选中这个UI
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    public void DisableAllInputs()//点击按钮后禁用所有输入，避免重复点击
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled = false;
    }
}
