/*********************************************************					
* UnitExercise_FishMove.cs					
* 작성자 : Hmit					
* 작성일 : 2024.08.08 오후 1:19					
**********************************************************/
using Dev_System;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Unit
{					
public class UnitExercise_FishMove : UnitExercise
    {

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private Transform FollowCube;              // 따라가는 큐브
        [SerializeField] private List<Transform> Pos_FishCube;      // 큐브 위치 5곳
        [SerializeField] private List<Transform> Pos_Bait;          // 미끼 뿌리는 위치 5곳
        [SerializeField] private GameObject[] Bait_Group;
        [SerializeField] private GameObject[] BaitParticles;

        private int curProceedsNum = 0;
        private Vector3 followPosition = Vector3.zero;
        private int curCubeNum;

        float timeElapsed = 0.0f;
        bool isBaitActive = false;

        protected override void Awake()
        {
            base.Awake();
            base.pUniqueID = UnitUniqueID.Exercise_FishMove;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
            curProceedsNum = 0;
            this.transform.localScale = Vector3.one;
            RandMovePos();
        }

        protected override void Update()
        {
            base.Update();

            if (base.StateMachine.CurState != null)
            {
                base.StateMachine.CurState.UpdateExcute();
            }

            if (isBaitActive)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed > 1f)
                {
                    InvokeBait();
                }
            }

            FollowCube.position = Vector3.Lerp(FollowCube.position, followPosition, Time.smoothDeltaTime);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            timeElapsed = 0.0f;
            isBaitActive = false;
        }


        //--------------------------------------------------------					
        // 추상함수 구현		
        //--------------------------------------------------------	

        public override void Idle()
        {

        }

        public override void PrePare()
        {
            if (PrePareFeedbacks.Count <= curProceedsNum) return;
            RandMovePos();
            Bait();
            base.PrePareFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.PrePareFeedbacks[curProceedsNum]?.PlayFeedbacks();
        }

        public override void Shoot()
        {
            if (ShootFeedbacks.Count <= curProceedsNum) return;
            RandMovePos();
            Bait();
            base.ShootFeedbacks[curProceedsNum]?.StopFeedbacks();
            base.ShootFeedbacks[curProceedsNum]?.PlayFeedbacks();

            curProceedsNum++;

            //마지막 연출 feedback 
            if (ShootFeedbacks.Count <= curProceedsNum)
            {
                StartCoroutine(Active(this.gameObject, false, 1.5f));
            }
        }

        IEnumerator Active(GameObject obj, bool active, float time)
        {
            yield return new WaitForSeconds(time);
            //if (obj == this.gameObject)
            //{
            //    base.DisableFeedback?.PlayFeedbacks();

            //    yield return new WaitUntil(() => base.DisableFeedback.IsPlaying == false);
            //}

            obj.SetActive(active);
        }

        void RandMovePos()
        {
            int curNum = Random.Range(0, Pos_FishCube.Count);
            if (curCubeNum == curNum || Mathf.Abs(curCubeNum - curNum) < 2)
            {
                RandMovePos();
                return;
            }
            curCubeNum = curNum;
            followPosition = Pos_FishCube[curCubeNum].position;
        }

        void Bait()
        {
            foreach (var bat in Bait_Group)
            {
                bat.SetActive(false);       // 애니메이션 때문에 한번 껐다 켜줌
                bat.SetActive(true);
                Vector3 pos = Pos_Bait[curCubeNum].position;
                Vector3 randomVector3 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                bat.transform.position = pos + randomVector3;
                bat.GetComponent<Animator>().SetTrigger("Bait");
            }

            BaitParticles[curCubeNum].SetActive(true);
            //Invoke("InvokeBait", 1.0f);
        }


        void InvokeBait()
        {
            foreach (var bat in Bait_Group)
            {
                bat.SetActive(false);
            }
            isBaitActive = false;
            timeElapsed = 0.0f;
        }



    }//end of class					


}//end of namespace					