using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///�����ģʽ
///</summary>
[System.Serializable]//Pool��û�м̳�monobehaviour��������Ҫ�������������л��ֶα�¶����
public class Pool
{
    public GameObject Prefab => prefab;//get��lambda���ʽ
    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;//Ĭ�϶��г���
    Transform parent;//���Ƴ�����Ԥ����ĸ����壬���ڹ���

    Queue<GameObject> queue;

    public void Initialize(Transform parent)//��ʼ������
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());//���У��൱��List��add
        }
    }
    GameObject Copy()//����Ԥ����
    {
        var copy = GameObject.Instantiate(prefab,parent);
        copy.SetActive(false);
        return copy;
    }
    GameObject AvailableObject()//ȡ�����ö���
    {
        GameObject availableObject = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)//������ֻ��һ��Ԫ��ʱ��ȡ���Ķ���������ڹ�������peek����ȡ����һ��Ԫ�ص����Ƴ����ٲ鿴���Ƿ�����
        {
            availableObject = queue.Dequeue();//ȡ�����Ƴ��������һ��Ԫ��
        }
        else
        {
            availableObject = Copy();
        }
        queue.Enqueue(availableObject);//���еĶ�����Ҫ��������
        return availableObject;
    }
    public GameObject PreparedObject()//���ÿ��ö���
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position)//���ÿ��ö���
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;  

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position,Quaternion rotation)//���ÿ��ö���
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)//���ÿ��ö���
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }
    //public void Return(GameObject gameObject) //����ʹ�������Ҫ��������
    //{
    //    queue.Enqueue(gameObject);
    //}

}
