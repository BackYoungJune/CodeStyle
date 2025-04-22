/*********************************************************					
* UnitTrainer.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_Animation;
using Dev_System;
using Dreamteck.Splines;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;					
					
namespace Dev_Unit
{
    public struct TrainerAnimatorID
    {
        public int AnimID_MotionSpeed;
        public int AnimID_MoveSpeed;
        public int[] AnimID_Pant;
        public int AnimID_OnBattle;          //OnBattle
        public int AnimID_OnFitness;        //OnFintness
        public int AnimID_Dead;             //Dead
        public int AnimID_Direction;        // 방향

        //감정동작
        public int AnimID_Emo_Greeting;
        public int AnimID_Emo_ThumbUp;
        public int AnimID_Emo_ThumbUp2;
        public int AnimID_Emo_Fast;
        public int AnimID_Emo_Clapping2;
        public int AnimID_Emo_HurryUp;

        // Transport
        public int AnimID_Transport;
        public int AnimID_Climb;
    }

    [RequireComponent(typeof(Animator))]
    public abstract class UnitTrainer : UnitBase
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        protected override void InitUnit(UnitState state)
        {
            // 컴포넌트 초기화
            Animatorbase = GetComponent<Animator>();

            // UnitManager 캐싱
            Managers.Unit.AddUnit(this);

            //Player 상태 캐싱
            TrainerStateCache.Add(UnitState.None, null);
            TrainerStateCache.Add(UnitState.CutScene, new TrainerCutScene());
            TrainerStateCache.Add(UnitState.FitnessIdle, new TrainerFitnessIdle());
            TrainerStateCache.Add(UnitState.FitnessAction, new TrainerFitnessAction());
            TrainerStateCache.Add(UnitState.Climb, new TrainerClimb());
            StateMachine = new UnitStateMachine<UnitTrainer>(this, TrainerStateCache[state]);
            ChangeStateMachine(state);
        }
        public void ChangeStateMachine(UnitState state)
        {
            if (StateMachine == TrainerStateCache[state])
            {
                Debug.LogError("[UnitPlayer] 동일한 State로 변경 불가!");
                return;
            }

            StateMachine.InitState(TrainerStateCache[state]);
            curState = state;
        }

        public void PlayerRotation(Vector3 rot)
        {
            transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }

        public Animator pAnimatorbase { get { return Animatorbase; } }

        public TrainerAnimatorID pAnimatorIDTable { get { return AnimatorIDTable; } }
        public UnitState pCurState { get { return curState; } set { curState = value; } }
        public bool pGetureDirection { get; set; }
        public AnimatorStateInfo pAnimatorStateInfo { get { return AnimatorStateInfo; } }
        public SplineFollower Follower { get; set; }
        public float climbPlusPercent { get; set; }
        public void SetTriggerCompare(int animID, bool production = false)
        {
            //현재 애니메이션중인가?

            //애니메이션중이 아닌가?
            if (production)
            {
                Animatorbase.SetTrigger(animID);
            }
            else
            {
                if (Animatorbase.GetCurrentAnimatorClipInfo(0)[0].clip.name == "root|Idle_Battle" || Animatorbase.GetCurrentAnimatorClipInfo(0)[0].clip.name == "root|Idle_Normal" || Animatorbase.GetCurrentAnimatorClipInfo(0)[0].clip.name == "root|Boring" ||
                    Animatorbase.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanIT_Animation_DeepBow_Idle_Female" || Animatorbase.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanIT_Animation_DeepBow_Idle_Male")
                    Animatorbase.SetTrigger(animID);
            }
        }


        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------	
        [Header("---------- State Debug ----------")]
        [SerializeField] UnitState curState;
        [SerializeField] UnitMoveType CurMoveType;

        [Header("---------- Feel Feedback Settings ----------")]
        public MMF_Player FitnessFeedback;

        protected TrainerAnimatorID AnimatorIDTable;
        protected UnitStateMachine<UnitTrainer> StateMachine;
        protected Dictionary<UnitState, UnitInterface<UnitTrainer>> TrainerStateCache = new();
        protected Animator Animatorbase;
        protected AnimatorStateInfo AnimatorStateInfo;

        //--------------------------------------------------------					
        // 추상함수			
        //--------------------------------------------------------	
        public abstract void OnIdle(int animatorID, bool bo, float delay);

        //--------------------------------------------------------					
        // 가상함수
        //--------------------------------------------------------	
        public virtual void FitnessFeedbackEffect()
        {
            FitnessFeedback?.PlayFeedbacks(transform.position);
        }


        protected virtual void Awake()
        {
            //AssignAnimatorIDs();
        }


        protected virtual void OnEnable()
        {

        }


        protected virtual void Start()
        {
        }


        protected virtual void Update()
        {
            if (StateMachine != null)
            {
                if (StateMachine.CurState != null)
                {
                    StateMachine.CurState.UpdateExcute();
                }
            }
        }


        protected virtual void OnDisable()
        {
            StopAllCoroutines();
            Managers.Unit.RemoveUnit(this);
            Disable();
        }


        void Disable()
        {

        }

        protected virtual void OnTriggerEnter(Collider col)
        {
            if (StateMachine.CurState != null)
            {
                StateMachine.CurState.OnTriggerEnter(col);
            }
        }


        protected virtual void AssignAnimatorIDs()
        {
            //Move관련
            AnimatorIDTable.AnimID_MoveSpeed = Animator.StringToHash("MovementSpeed");
            AnimatorIDTable.AnimID_MotionSpeed = Animator.StringToHash("MotionSpeed");
            AnimatorIDTable.AnimID_Pant = new int[2];
            AnimatorIDTable.AnimID_Pant[0] = Animator.StringToHash("Pant1");
            AnimatorIDTable.AnimID_Pant[1] = Animator.StringToHash("Pant2");

            //Battle & Fitness관련
            AnimatorIDTable.AnimID_OnBattle = Animator.StringToHash("OnBattle");
            AnimatorIDTable.AnimID_OnFitness = Animator.StringToHash("OnFitness");
            AnimatorIDTable.AnimID_Dead = Animator.StringToHash("Dead");
            AnimatorIDTable.AnimID_Direction = Animator.StringToHash("Direction");

            //감정동작관련
            AnimatorIDTable.AnimID_Emo_ThumbUp = Animator.StringToHash("Emo_ThumbUp");
            AnimatorIDTable.AnimID_Emo_Greeting = Animator.StringToHash("Emo_Greeting");
            AnimatorIDTable.AnimID_Emo_Fast = Animator.StringToHash("Emo_Fast");
            AnimatorIDTable.AnimID_Emo_ThumbUp2 = Animator.StringToHash("Emo_ThumbUp2");
            AnimatorIDTable.AnimID_Emo_Clapping2 = Animator.StringToHash("Emo_Clapping2");
            AnimatorIDTable.AnimID_Emo_HurryUp = Animator.StringToHash("Emo_HurryUp");

            // Transport 관련
            AnimatorIDTable.AnimID_Transport = Animator.StringToHash("Transport");
            AnimatorIDTable.AnimID_Climb = Animator.StringToHash("Climb");
        }

        protected IEnumerator SelectIdle(int animatorID, bool bo, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            Animatorbase.SetBool(animatorID, bo);
        }




        //--------------------------------------------------------					
        //          Animation Events	
        //--------------------------------------------------------	




    }//end of class					
}//end of namespace					