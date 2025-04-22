/*********************************************************					
* UnitPlayer.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.11.07 오후 2:44					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using Dreamteck.Splines;
using MoreMountains.Feedbacks;
using Dev_System;
using System;
using Dev_Animation;
using Dev_Occulus;
using Dev_Transport;
using static Dev_Unit.PlayerClimb;

namespace Dev_Unit
{			
    public struct PlayerAnimatorID
    {
        public int AnimID_MotionSpeed;
        public int AnimID_MoveSpeed;
        public int[] AnimID_Pant;
        public int AnimID_OnBattle;          //OnBattle
        public int AnimID_OnFitness;        //OnFintness
        public int AnimID_Dead;             //Dead
        public int AnimID_Direction;        // 방향

        //감정동작
        public int AnimID_Emo_No;
        public int AnimID_Emo_Happy1;
        public int AnimID_Emo_Happy2;
        public int AnimID_Emo_Dance1;
        public int AnimID_Emo_Dance2;       //승리모션
        public int AnimID_Emo_Cheerup1;
        public int AnimID_Emo_Cheerup2;
        public int AnimID_Emo_Cheerup3;
        public int AnimID_Emo_Greeting; 
        public int AnimID_Emo_Angry1;        //패배모션
        public int AnimID_Emo_Clap;
        public int AnimID_Emo_ThumbUp;
        public int AnimID_Emo_Victory;
        public int AnimID_Emo_Defeat;
        public int AnimID_Emo_Frustration;  // 좌절모션
        public int AnimID_Emo_MaleGreeting;
        public int AnimID_Emo_Talk;         //대화
    }

    public abstract class UnitPlayer : UnitBase
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------	        


        protected override void InitUnit(UnitState state)
        {
            // 컴포넌트 초기화
            //Animatorbase = GetComponent<Animator>();
            follower = GetComponentInParent<SplineFollower>();
            playerAnimationEvent = GetComponent<PlayerAnimationEvent>();

            // UnitManager 캐싱
            Managers.Unit.AddUnit(this);

            //Player 상태 캐싱
            PlayerStateCache.Add(UnitState.None, null);
            PlayerStateCache.Add(UnitState.CutScene, new PlayerCutScene());
            PlayerStateCache.Add(UnitState.MoveSpline, new PlayerMove());
            PlayerStateCache.Add(UnitState.BattleIdle, new PlayerBattleIdle());
            PlayerStateCache.Add(UnitState.BattleAttack, new PlayerBattleAttack());
            PlayerStateCache.Add(UnitState.BattleGuard, new PlayerBattleGuard());
            PlayerStateCache.Add(UnitState.BattleEnd, new PlayerBattleEnd());
            PlayerStateCache.Add(UnitState.BattleExercise, new PlayerExercise());
            PlayerStateCache.Add(UnitState.FitnessIdle, new PlayerFitnessIdle());
            PlayerStateCache.Add(UnitState.FitnessAction, new PlayerFitnessAction());
            PlayerStateCache.Add(UnitState.Climb, new PlayerClimb());
            PlayerStateCache.Add(UnitState.OtherTransport, new PlayerOtherTransport());
            //PlayerStateCache.Add(UnitState.Profit, new PlayerProfit());
            //PlayerStateCache.Add(UnitState.UpstairSpline, new PlayerUpStairMove());
            StateMachine = new UnitStateMachine<UnitPlayer>(this, PlayerStateCache[state]);
            ChangeStateMachine(state);

            MeterFollower = follower;
        }


        public void ChangeStateMachine(UnitState state)
        {
            if (StateMachine == PlayerStateCache[state])
            {
                Debug.LogError("[UnitPlayer] 동일한 State로 변경 불가!");
                return;
            }

            StateMachine.InitState(PlayerStateCache[state]);
            curState = state;
        }

        public void PlayerRotation(Vector3 rot)
        {
            transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }

        public bool isMoving;
        public void Call_Calculate_WalkRunTime(bool isStart)
        {
            if (isStart)
                StartCoroutine("Calculate_WalkRunTime");
            else
                StopCoroutine("Calculate_WalkRunTime");

        }
        //걷기/뛰기 시간 계산
        IEnumerator Calculate_WalkRunTime()
        {
            Debug.Log("----------Walk & Run 초당 스테미나 계산 시작 ----------");
            while (true)
            {
                //걷는중 아니면 대기
                yield return new WaitUntil(() => isMoving);


                //초당 스테미나 0.2씩 감소
                // Todo : 우선 주석처리
                //GameManager.Instance.Calculate_Stemina(consumePercent: 0.2f, isPlus: false, action: null);
                //Debug.LogError("---------- Walk & Run 초당 스테미나 계산 Update... ----------");

                //1초당 업데이트
                yield return new WaitForSeconds(1.0f);
            }
        }

        public void ChangeSpeed(float speed)
        {
            FollowMaxSpeed= speed;
        }

        //public bool IsCurAnimationEndFrame
        //{
        //    get
        //    {
        //        return Animatorbase.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f ? true : false;
        //    }
        //}

        public void ClimbInput(float speed)
        {
            pAnimationHalfEnd = false;
            ExerciseInputCount++;
            if(ExerciseInputCount > 2)
            {
                ExerciseInputCount = 2;
            }

            TransportMoveSpeed = speed;
        }

        public void OtherTransportInput(float speed)
        {
            pAnimationHalfEnd = false;
            ExerciseInputCount++;
            if (ExerciseInputCount > 2)
            {
                ExerciseInputCount = 2;
            }

            // 임시코드
            TransportMoveSpeed = speed;
        }

        public void FadeTransition(float duration, bool In = true)
        {
            StartCoroutine(FadeTransitionCor(duration, In));
        }

        IEnumerator FadeTransitionCor(float duration, bool In)
        {
            float elpasedTime = 0;
            while(elpasedTime <= duration)
            {
                elpasedTime += Time.deltaTime;
                float t = elpasedTime / duration;



                yield return null;
            }


            yield return null;
        }

        public void RateMoveSpeed(float speed)
        {
            rateSpeed = speed;
        }



        [Range(0, 10)]
        public float FollowMaxSpeed;
        [NonSerialized] public float NormalFollowSpeed;
        public SplineFollower Follower 
        { 
            get 
            { 
                if(follower == null)
                {
                    follower = this.transform.parent?.GetComponent<SplineFollower>();
                }
                return follower; 
            }
            set
            {
                follower = value;
            }
        }
        public SplineFollower MeterFollower { get; set; }
        public UnitState pCurState { get { return curState; } set { curState = value; } }
        public UnitMoveType pCurMoveType { get { return CurMoveType; } set { CurMoveType = value; } }
        public float CurMoveSpeed { get; set; }
        public bool pAnimationHalfEnd { get; set; }
        public int ExerciseInputCount { get; set; }
        public float TransportMoveSpeed { get; set; }
        public PlayerAnimationEvent PlayerAnimationEvent { get { return playerAnimationEvent; } }
        public CameraRigMaster pCurCameraRigMaster {  get {  return CurCameraRigMaster; } }
        public bool pGetureDirection { get; set; }
        public bool ExistFollower { get { return follower != null; } }

        public Transform pStoneUIPos { get { return StoneUIPos; } }  
        public Transform pMeditationGestureUIPos { get { return MeditationGestureUIPos; } }

        public Transport CurTransport { get; set; }
        public float pRateSpeed { get { return rateSpeed; } set { rateSpeed = value; } }
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------	
        [Header("---------- CameraRigMaster Settings ----------")]
        [SerializeField] private CameraRigMaster CurCameraRigMaster;
        [Header("---------- State Debug ----------")]
        [SerializeField] UnitState curState;
        [SerializeField] UnitMoveType CurMoveType;

        [Header("---------- Feel Feedback Settings ----------")]
        public MMF_Player StopFeedback;
        public MMF_Player RunFeedback;
        public MMF_Player Perfect_GuardFeedback;
        public MMF_Player Good_GuardFeedback;
        public MMF_Player FitnessFeedback;


        [Header("---------- GenPos ----------")]
        [SerializeField] private Transform StoneUIPos;
        [SerializeField] private Transform MeditationGestureUIPos;

        //protected PlayerAnimatorID AnimatorIDTable;
        protected SplineFollower follower;
        protected UnitStateMachine<UnitPlayer> StateMachine;
        protected Dictionary<UnitState, UnitInterface<UnitPlayer>> PlayerStateCache = new();
        //protected Animator Animatorbase;
        protected PlayerAnimationEvent playerAnimationEvent;
        protected AnimatorStateInfo AnimatorStateInfo;
        protected float activityExerciseSpeed = 0;
        private Coroutine ActivityCor = null;

        private float rateSpeed = 1.0f;



        //--------------------------------------------------------					
        // 추상함수			
        //--------------------------------------------------------	
        public abstract void OnIdle(int animatorID, bool bo, float delay);
        public abstract void OnAttack(Transform genPos);    //공격
        public abstract void Guard(GestureReferee referee, Transform genPos); //방어
        public abstract void OnExercise();  //비전투 운동
        public abstract void Dead();                        //플레이어 사망

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
            DontDestroyOnLoad(this.gameObject);
        }

        protected virtual void OnEnable()
        {
            
        }


        protected virtual void Start()
        {
            NormalFollowSpeed = FollowMaxSpeed;
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
        }

        protected virtual void OnTriggerEnter(Collider col)
        {
            if (StateMachine.CurState != null)
            {
                StateMachine.CurState.OnTriggerEnter(col);
            }
        }


        protected IEnumerator SelectIdle(int animatorID, bool bo, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
        }




        //--------------------------------------------------------					
        //          Animation Events	
        //--------------------------------------------------------	




    }//end of class		


}//end of namespace					