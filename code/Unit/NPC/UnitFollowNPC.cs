/*********************************************************					
* UnitFollowNPC.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.09.19 오후 2:05					
**********************************************************/
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using PixelCrushers.DialogueSystem;
using Dev_Spline;
using Dev_System;

namespace Dev_Unit
{					
	public abstract class UnitFollowNPC : UnitNPC
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        protected override void InitUnit(UnitState state)
        {
            base.InitUnit(state);
            //NPC 상태 캐싱
            NPCStateCache.Add(UnitState.None, null);
            NPCStateCache.Add(UnitState.CutScene, new FollowNPCCutScene());
            NPCStateCache.Add(UnitState.MoveSpline, new FollowNPCFollowSpline());
            NPCStateCache.Add(UnitState.LookAt, new FollowNPCFollowSplineLookAt());
            NPCStateCache.Add(UnitState.BattleIdle, new FollowNPCBattleIdle());
            NPCStateCache.Add(UnitState.BattleEnd, new FollowNPCBattleEnd());
            NPCStateCache.Add(UnitState.Climb, new FollowNPCClimb());
            NPCStateCache.Add(UnitState.OtherTransport, new FollowNPCOtherTransport());
            NPCStateCache.Add(UnitState.FastSpline, new FollowNPCFastFollowSpline());
            NPCStateCache.Add(UnitState.MoveSpline_NoLookAt, new FollowNPCMoveSplineNoLookAt());
            StateMachine = new UnitStateMachine<UnitFollowNPC>(this, NPCStateCache[UnitState.None]);
            ChangeStateMachine(state);

            follower.motion._offset.y = 0;
        }

        public void ChangeStateMachine(UnitState state)
        {
            if (StateMachine == NPCStateCache[state])
            {
                Debug.LogError("[UnitNPC] 동일한 State로 변경 불가!");
                return;
            }

            StateMachine.InitState(NPCStateCache[state]);
            base.curState = state;
        }

        public void NPCRotation(Vector3 rot)
        {
            transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }

        public void FastRun(float _time)
        {
            Invoke("FastRunState", _time);
        }

        public void SplineSetting(SplineStage splineStage, SplineComputer splineComputer)
        {
            follower.spline = splineComputer;
            if (splineStage == SplineStage.Stage)
            {
                follower.Restart(1.0f);
                follower.motion._offset.x = -1.3f;// = new Vector2(-0.81f, 0);
                follower.motion._offset.y = 0.0f;// = new Vector2(0.1f, 0);
            }
            else if (splineStage == SplineStage.Mission)
            {
                follower.Restart(0.1f);
                follower.motion._offset.x = -0.81f;// = new Vector2(-0.81f, 0);
                follower.motion._offset.y = 0.0f;// = new Vector2(0.1f, 0);
            }
            else if (splineStage == SplineStage.RunStraight)
            {
                follower.Restart(0.1f);
                follower.motion._offset.x = 0.0f;// = new Vector2(-0.81f, 0);
                follower.motion._offset.y = 0.0f;// = new Vector2(0.1f, 0);
            }
            else
            {
                follower.Restart(0.1f);
                follower.motion._offset.x = -0.56f;// = new Vector2(-0.81f, 0);
                follower.motion._offset.y = 0.0f;// = new Vector2(0.1f, 0);
            }
        }

        public void BarkDialogue()
        {
            string barkText = "";
            if (Managers.Game.pCurLanguage == Dev_System.DevSystemLanguage.ko_KR)
            {
                int rand = Random.Range(0, BarkString_KR.Length);
                barkText = BarkString_KR[rand];
            }
            else if (Managers.Game.pCurLanguage == Dev_System.DevSystemLanguage.en_US)
            {
                int rand = Random.Range(0, BarkString_EN.Length);
                barkText = BarkString_EN[rand];
            }

            DialogueSystemTrigger.barkText = barkText;
            DialogueSystemTrigger.OnUse();
        }



        public SplineFollower Follower { get { return follower; } set { follower = value; } }
        public AnimationCurve Curve { get { return curve; } }
        public DialogueSystemTrigger DialogueSystemTrigger { get { return dialogueSystemTrigger; } }

        public string[] BarkString_KR = { "힘내요!", "빨리 빨리!", "서둘러요!" };
        public string[] BarkString_EN = { "Hurry Up!" };
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------			
        [Header("---------- NPC Settings ----------")]
        [SerializeField] protected DialogueSystemTrigger dialogueSystemTrigger;
        [SerializeField] protected AnimationCurve curve;

        protected UnitStateMachine<UnitFollowNPC> StateMachine;
        protected Dictionary<UnitState, UnitInterface<UnitFollowNPC>> NPCStateCache = new();
        protected SplineFollower follower;

        protected override void AssignAnimatorIDs()
        {
            base.AssignAnimatorIDs();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            if (StateMachine != null)
            {
                if (StateMachine.CurState != null)
                {
                    StateMachine.CurState.UpdateExcute();
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }


        void FastRunState()
        {
            ChangeStateMachine(UnitState.FastSpline);
        }

        //--------------------------------------------------------					
        // 추상함수			
        //--------------------------------------------------------		

        public abstract void Move();        //이동 - 어드벤쳐
        public abstract void LookAt();      //룩엣 - 어드벤쳐


    }//end of class					
}//end of namespace					