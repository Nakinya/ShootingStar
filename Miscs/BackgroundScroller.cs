using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Vector2 scrollVelocity;//±³¾°ÒÆ¶¯ËÙ¶È
    Material material;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        if(GameManager.GameState != GameState.GameOver)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
        }
    }
}
