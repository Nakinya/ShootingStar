using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class LootItem : MonoBehaviour
{
    [SerializeField] float minSpeed = 5f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] protected AudioData defaultPickUpSFX; 

    protected Player player;
    protected AudioData pickUpSFX;
    protected Text lootMessage;

    Animator animator;
    int pickUpStateID = Animator.StringToHash("PickUp");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<Player>();
        pickUpSFX = defaultPickUpSFX;
        lootMessage = GetComponentInChildren<Text>(true);//ע��lootMessageһ��ʼ�ǽ���״̬�ģ��Ӹ�����true���Ի�ý���״̬���
    }
    private void OnEnable()
    {
        StartCoroutine(MoveCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
        animator.Play(pickUpStateID);
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);
    }
    protected virtual void PickUp()
    {
        StopAllCoroutines();
    }
    IEnumerator MoveCoroutine()//��ս��Ʒ�Զ��������
    {
        float speed = Random.Range(minSpeed, maxSpeed);
        Vector3 diretion = Vector3.left;
        while (true)
        {
            if (player.isActiveAndEnabled)
            {
                diretion = (player.transform.position - transform.position).normalized;
            }
            transform.Translate(diretion*speed*Time.deltaTime);
            yield return null;
        }

    }
}
