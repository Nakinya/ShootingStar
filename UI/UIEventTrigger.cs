using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

///<summary>
///
///</summary>
public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, ISelectHandler, ISubmitHandler//处理EventSystem所广播的事件
{
    [SerializeField] AudioData selectSFX;
    [SerializeField] AudioData submitSFX;


    public void OnPointerEnter(PointerEventData eventData)//检测到鼠标悬停在脚本所挂载的对象上时就会调用
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
