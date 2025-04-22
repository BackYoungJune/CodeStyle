/*********************************************************					
* Transport.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.02.24 오전 10:54					
**********************************************************/					
using UnityEngine;
using Dev_Unit;
using Dev_Animation;
using Dreamteck.Splines;
using UnityEngine.Events;
using System.Collections;
using Dev_System;

namespace Dev_Transport					
{					
	public class Transport : MonoBehaviour					
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void InitTransport(bool npc = true)
        {
            player = Managers.Unit.GetCurTurnUnit;
            followNPC = Managers.Unit.GetCurFollowNPCUnit;
            Follower = GetComponent<SplineFollower>();
            Follower.useTriggers = true;

            // 플레이어 NPC Transform 조정
            player.transform.SetParent(PlayerPivot);
            player.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            player.transform.localScale = Vector3.one;
            
            if(npc)
            {
                followNPC.transform.SetParent(NPCPivot);
                followNPC.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                followNPC.transform.localScale = Vector3.one * 0.3f;
            }

            player.MeterFollower = Follower;
            player.CurTransport = this;
            Follower.followSpeed = 0;
            Init();
        }


        public void ArriveTransport()
        {
            StartCoroutine(ArriveCoroutine());
        }

        [Range(0.0f, 10.0f)]
        public float FollowMaxSpeed;
        public Transform PlayerPivot;
        public Transform NPCPivot;
        [HideInInspector] public SplineFollower Follower;
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        protected UnitPlayer player;
        protected UnitNPC followNPC;
        
        

        protected virtual void OnEnable()
        {
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            // 끝부분에 도달하면
            if(Follower.result.percent >= 1.0f)
            {
                Follower.followSpeed = 0;
                player.CurMoveSpeed = 0;
            }
        }

        protected virtual void Init() { }

        IEnumerator ArriveCoroutine()
        {
            yield return new WaitForEndOfFrame();

            Managers.ObjectPool.ReturnObjectPool(PoolObjectType.Pool, this.gameObject);
            this.gameObject.SetActive(false);
            player.CurTransport = null;
        }

        protected virtual void OnDisable()
        {
        }

    }//end of class					
}//end of namespace					