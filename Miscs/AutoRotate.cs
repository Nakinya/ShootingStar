using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class AutoRotate : MonoBehaviour
{
    [SerializeField] float speed = 360f;
    [SerializeField] Vector3 angle;
    private void Update()
    {
        transform.Rotate(angle * speed * Time.deltaTime);
    }
}
