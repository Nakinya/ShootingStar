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
    float ballisticAngel;//�����Ƕ����ֵ
    Vector3 targetPosition;
    public IEnumerator HomingCoroutine(GameObject target)//�鳲(׷��)Э��
    {
        ballisticAngel = Random.Range(minBallisticAngle, maxBallisticAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {//����Ŀ���ƶ�
                targetPosition = target.transform.position - this.transform.position;
                //����Ŀ����ת
                //var angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
                this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg, Vector3.forward);
                this.transform.rotation *= Quaternion.Euler(0f,0f,ballisticAngel);//���ӵ������Ƕȴ�һЩ
                //�ƶ�
                projectile.MoveOnOwn();
            }
            else
            {//�����ӵ�ǰ���ƶ��ƶ�
                projectile.MoveOnOwn();
            }
            yield return null;
        }
    }
}
