using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
///
///</summary>
public class PlayerOverdrive : MonoBehaviour
{
    [SerializeField] GameObject triggerVFX;//能量爆发触发时的特效，本身会挂载AutoDetective
    [SerializeField] GameObject engineVFXNormal;//正常状态下玩家机体的特效
    [SerializeField] GameObject engineVFXOverdrive;//overdrive状态下玩家机体的特效

    [SerializeField] AudioData onSFX;//开启时的音效
    [SerializeField] AudioData offSFX;//关闭时的音效

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
