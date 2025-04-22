/*********************************************************					
* UnitSurveFollowNPC.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.09.19 오후 2:05					
**********************************************************/
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Dev_Spline;

namespace Dev_Unit
{					
	public abstract class UnitSubFollowNPC : UnitNPC
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        protected override void InitUnit(UnitState state)
        {
            base.InitUnit(state);

            //NPC 상태 캐싱
            NPCStateCache.Add(UnitState.None, null);
            NPCStateCache.Add(UnitState.CutScene, new SubFollowNPCCutScene());
            NPCStateCache.Add(UnitState.Idle, new SubFollowNPCIdle());
            NPCStateCache.Add(UnitState.MoveSpline, new SubFollowNPCFollowSpline());
            NPCStateCache.Add(UnitState.BattleIdle, new SubFollowNPCBattleIdle());
            NPCStateCache.Add(UnitState.BattleEnd, new SubFollowNPCBattleEnd());
            NPCStateCache.Add(UnitState.LookAt, new SubFollowNPCFollowSplineLookAt());
            StateMachine = new UnitStateMachine<UnitSubFollowNPC>(this, NPCStateCache[UnitState.None]);
            ChangeStateMachine(state);
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

        public void SplineSetting(SplineStage splineStage, SplineComputer splineComputer)
        {
            follower.spline = splineComputer;
            if (splineStage == SplineStage.Stage)
            {
                follower.Restart(0.01f);
                follower.motion._offset.x = 0.8f;
                follower.motion._offset.y = 0.0f;
            }
            else if (splineStage == SplineStage.Mission)
            {
                follower.Restart(0.01f);
                follower.motion._offset.x = 0.8f;
                follower.motion._offset.y = 0.0f;
            }
            else if (splineStage == SplineStage.RunStraight)
            {
                follower.Restart(0.01f);
                follower.motion._offset.x = 0.5f;
                follower.motion._offset.y = 1.7f;
            }
            else
            {
                follower.Restart(0.01f);
                follower.motion._offset.x = 0.5f;
                follower.motion._offset.y = 1.7f;
            }
        }

        public SplineFollower Follower { get { return follower; } set { follower = value; } }
        public AnimationCurve Curve { get { return curve; } }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------			
        [Header("---------- NPC Settings ----------")]
        [SerializeField] protected AnimationCurve curve;

        protected UnitStateMachine<UnitSubFollowNPC> StateMachine;
        protected Dictionary<UnitState, UnitInterface<UnitSubFollowNPC>> NPCStateCache = new();
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

        //--------------------------------------------------------					
        // 추상함수			
        //--------------------------------------------------------		

        public abstract void Move();        //이동 - 어드벤쳐


    }//end of class						
}//end of namespace					