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
    InputSystemUIInputModule UIInputModule;//EventSystem�����

    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)//selectable�������п�ѡ��Unity UI��Ļ���
    {
        UIObject.Select();//ѡ�����UI
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    public void DisableAllInputs()//�����ť������������룬�����ظ����
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled = false;
    }
}
