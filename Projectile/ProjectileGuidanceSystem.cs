using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] float minBallisticAngle = 50f;
    [SerializeField] float maxBallisticAngle = 75f;
    float ballisticAngel;//弹道角度随机值
    Vector3 targetPosition;
    public IEnumerator HomingCoroutine(GameObject target)//归巢(追踪)协程
    {
        ballisticAngel = Random.Range(minBallisticAngle, maxBallisticAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {//朝着目标移动
                targetPosition = target.transform.position - this.transform.position;
                //朝向目标旋转
                //var angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
                this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg, Vector3.forward);
                this.transform.rotation *= Quaternion.Euler(0f,0f,ballisticAngel);//让子弹弹道角度大一些
                //移动
                projectile.MoveOnOwn();
            }
            else
            {//朝着子弹前方移动移动
                projectile.MoveOnOwn();
            }
            yield return null;
        }
    }
}
