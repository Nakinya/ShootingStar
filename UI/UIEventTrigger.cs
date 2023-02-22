using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

///<summary>
///
///</summary>
public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, ISelectHandler, ISubmitHandler//����EventSystem���㲥���¼�
{
    [SerializeField] AudioData selectSFX;
    [SerializeField] AudioData submitSFX;


    public void OnPointerEnter(PointerEventData eventData)//��⵽�����ͣ�ڽű������صĶ�����ʱ�ͻ����
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
