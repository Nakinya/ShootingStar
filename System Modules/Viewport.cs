using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Viewport : Singleton<Viewport>
{
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float middleX;
    public float MaxX => maxX;
    private void Start()
    {
        Camera mainCamera = Camera.main;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f,0f));//�ӿ�����ת������������
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f,1f));

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f,0f,0f)).x;
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)//��������ƶ���Χ
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x,minX+paddingX,maxX-paddingX);
        position.y = Mathf.Clamp(playerPosition.y,minY+paddingY,maxY-paddingY);
        return position;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY,maxY - paddingY);

        return position;
    }

    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(middleX,maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
}