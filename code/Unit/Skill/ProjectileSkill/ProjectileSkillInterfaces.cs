/*********************************************************					
* ProjectileSkillInterfaces.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.19 오후 7:36					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using Dev_Unit;
using Dev_System;
using Unity.VisualScripting;

namespace Dev_UnitSkill
{
    /// <summary>
    /// 가만히 있는 발사체
    /// </summary>
    public class ProjectileIdle : SkillInterface<ProjectileSkill>
    {
        private ProjectileSkill CurProjectile;

        public void Enter(ProjectileSkill skill)
        {
            CurProjectile = skill;
        }

        public void UpdateExcute()
        {
        }

        public void Exit()
        {
            //isHit = false;
        }

        public IEnumerator Hit()
        {
            yield return null;
        }

        public void OnTriggerEnter(Collider col)
        {

        }


    }//end of class


    /// <summary>
    /// 직선 이동형 발사체
    /// </summary>
    public class ProjectileStraight : SkillInterface<ProjectileSkill>
    {
        private ProjectileSkill CurProjectile;
        private float moveSpeed;
        private float rotSpeed;


        public void Enter(ProjectileSkill skill)
        {
            CurProjectile = skill;
            moveSpeed = CurProjectile.pMoveSpeed;
            rotSpeed = CurProjectile.pRotSpeed;
        }

        public void UpdateExcute()
        {
            if (moveSpeed > 0 && CurProjectile.pTargetUnit != null)
            {
                //이동방향
                Vector3 dir = (CurProjectile.pTargetUnit.transform.position + Vector3.up * CurProjectile.pTargetUpPercent - CurProjectile.transform.position);
                //이동
                CurProjectile.transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
                //회전
                CurProjectile.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);


                //test 1
                //float angle = UnityEngine.Random.Range(60.0f, 120.0f);
                //Vector3 newForce = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f);
                //newForce *= UnityEngine.Random.Range(6.0f, 8.0f);
                //CurProjectile.pRigidBody.AddForceAtPosition(newForce, CurProjectile.transform.position);

                //test 2
                //Vector3 dir = (CurProjectile.pTargetUnit.transform.position - CurProjectile.transform.position);
                //CurProjectile.pRigidBody.AddForce(dir * moveSpeed);


            }
          

        }

        public void Exit()
        {
            //isHit = false;
        }

        public IEnumerator Hit()
        {
            CurProjectile.DisableModels();

            //발사체 자기자신 충돌처리
            CurProjectile.OnHit();

            yield return new WaitForSeconds(1.5f);

            //Debug.LogError(string.Format("<color=#FF0000><b>{0}</b></color> Projectile Straight Destroy", CurProjectile.name));
            if(Managers.ObjectPool != null)
            {
                Managers.ObjectPool.ReturnObjectPool(PoolObjectType.Pool, CurProjectile.gameObject);
            }
            //GameObject.Destroy(CurProjectile.gameObject);
        }

        public void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(CurProjectile.pTargetUnitTag))
            {
                CurProjectile.StartCoroutine(Hit());
            }
          
        }


    }//end of class

    /// <summary>
    /// 회전 이동형 발사체
    /// </summary>
    public class ProjectileRotate : SkillInterface<ProjectileSkill>
    {
        private ProjectileSkill CurProjectile;
        private float moveSpeed;
        private float rotSpeed;


        public void Enter(ProjectileSkill skill)
        {
            CurProjectile = skill;
            moveSpeed = CurProjectile.pMoveSpeed;
            rotSpeed = CurProjectile.pRotSpeed;
        }

        public void UpdateExcute()
        {
            if (moveSpeed > 0 && CurProjectile.pTargetUnit != null)
            {
                //이동방향
                Vector3 dir = ((CurProjectile.pTargetUnit.transform.position + Vector3.up * CurProjectile.pTargetUpPercent)  - CurProjectile.transform.position);
                //이동
                CurProjectile.transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
                //회전
                CurProjectile.transform.Rotate(CurProjectile.pRotateVector * rotSpeed * Time.deltaTime, Space.World);

            }


        }

        public void Exit()
        {
            //isHit = false;
        }

        public IEnumerator Hit()
        {
            CurProjectile.DisableModels();

            //발사체 자기자신 충돌처리
            CurProjectile.OnHit();

            yield return new WaitForSeconds(1.5f);

            //Debug.LogError(string.Format("<color=#FF0000><b>{0}</b></color> Projectile Straight Destroy", CurProjectile.name));
            if (Managers.ObjectPool != null)
            {
                Managers.ObjectPool.ReturnObjectPool(PoolObjectType.Pool, CurProjectile.gameObject);
            }
            //GameObject.Destroy(CurProjectile.gameObject);
        }

        public void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(CurProjectile.pTargetUnitTag))
            {
                CurProjectile.StartCoroutine(Hit());
            }

        }


    }//end of class

    /// <summary>
    /// 포물선 발사체
    /// </summary>
    public class ProjectileArc : SkillInterface<ProjectileSkill>
    {
        private ProjectileSkill CurProjectile;
        private float moveSpeed;
        private float rotSpeed;

        private float angle = 45f;
        private float gravity = 9.81f;
        private float time = 0;
        private float distance = 0;

        public void Enter(ProjectileSkill skill)
        {
            CurProjectile = skill;
            moveSpeed = CurProjectile.pMoveSpeed;
            rotSpeed = CurProjectile.pRotSpeed;
            distance = Vector3.Distance(CurProjectile.transform.position, CurProjectile.pTargetUnit.transform.position);
        }

        public void UpdateExcute()
        {
            if (moveSpeed > 0 && CurProjectile.pTargetUnit != null)
            {
                Vector3 startPos = CurProjectile.transform.position;
                Vector3 endPos = CurProjectile.pTargetUnit.transform.position;

                Vector3 center = (startPos + endPos) * 0.5f;
                center.y -= 3.0f;
                Vector3 riseRelCenter = startPos - center;
                Vector3 setRelCenter = endPos - center;
                time = Time.smoothDeltaTime * moveSpeed;
                CurProjectile.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, time);
                CurProjectile.transform.position += center;

                Vector3 dir = CurProjectile.pTargetUnit.transform.position - CurProjectile.transform.position;
                dir.y = 0;
                dir.Normalize();
                CurProjectile.transform.rotation = Quaternion.Slerp(CurProjectile.transform.rotation, Quaternion.LookRotation(dir), Time.smoothDeltaTime);
                distance = Vector3.Distance(CurProjectile.transform.position, endPos);
            }


        }

        public void Exit()
        {
            //isHit = false;
        }

        public IEnumerator Hit()
        {
            CurProjectile.DisableModels();

            //발사체 자기자신 충돌처리
            CurProjectile.OnHit();

            yield return new WaitForSeconds(1.5f);

            //Debug.LogError(string.Format("<color=#FF0000><b>{0}</b></color> Projectile Straight Destroy", CurProjectile.name));
            if (Managers.ObjectPool != null)
            {
                Managers.ObjectPool.ReturnObjectPool(PoolObjectType.Pool, CurProjectile.gameObject);
            }   
            //GameObject.Destroy(CurProjectile.gameObject);
        }

        public void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(CurProjectile.pTargetUnitTag))
            {
                CurProjectile.StartCoroutine(Hit());
            }

        }


    }//end of class

}//end of namespace					