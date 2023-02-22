using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///对象池模式
///</summary>
[System.Serializable]//Pool类没有继承monobehaviour，所以需要这个将里面的序列化字段暴露出来
public class Pool
{
    public GameObject Prefab => prefab;//get的lambda表达式
    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;//默认队列长度
    Transform parent;//复制出来的预制体的父物体，用于管理

    Queue<GameObject> queue;

    public void Initialize(Transform parent)//初始化队列
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());//入列，相当于List的add
        }
    }
    GameObject Copy()//复制预制体
    {
        var copy = GameObject.Instantiate(prefab,parent);
        copy.SetActive(false);
        return copy;
    }
    GameObject AvailableObject()//取出可用对象
    {
        GameObject availableObject = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)//队列中只有一个元素时，取出的对象可能正在工作，用peek可用取到第一个元素但不移除，再查看其是否启用
        {
            availableObject = queue.Dequeue();//取出并移除队列里第一个元素
        }
        else
        {
            availableObject = Copy();
        }
        queue.Enqueue(availableObject);//出列的对象需要重新入列
        return availableObject;
    }
    public GameObject PreparedObject()//启用可用对象
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position)//启用可用对象
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;  

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position,Quaternion rotation)//启用可用对象
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)//启用可用对象
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }
    //public void Return(GameObject gameObject) //对象使用完后需要重新入列
    //{
    //    queue.Enqueue(gameObject);
    //}

}
