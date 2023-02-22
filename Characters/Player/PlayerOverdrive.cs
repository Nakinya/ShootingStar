using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
///
///</summary>
public class PlayerOverdrive : MonoBehaviour
{
    [SerializeField] GameObject triggerVFX;//������������ʱ����Ч����������AutoDetective
    [SerializeField] GameObject engineVFXNormal;//����״̬����һ������Ч
    [SerializeField] GameObject engineVFXOverdrive;//overdrive״̬����һ������Ч

    [SerializeField] AudioData onSFX;//����ʱ����Ч
    [SerializeField] AudioData offSFX;//�ر�ʱ����Ч

    public static UnityAction on = delegate { };
    public static UnityAction off = delegate { };

    private void Awake()
    {
        on += On;
        off += Off;
    }
    private void OnDestroy()
    {
        on -= On;
        off -= Off;
    }
    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }

    void Off()
    {
        engineVFXNormal.SetActive(true);
        engineVFXOverdrive.SetActive(false);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
