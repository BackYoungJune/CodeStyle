/*********************************************************					
* NeutralNPC.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dreamteck.Splines;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev_UI;
using Dev_System;

namespace Dev_Unit
{					
    public struct BarkText
    {
        public string EngText;
        public string KorText;
    }

	public class NeutralNPC : MonoBehaviour					
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public void Interaction()
        {
            if (this.gameObject.activeInHierarchy)
            {
                StopCoroutine("InteractionCor");
                StartCoroutine("InteractionCor");
            }
        }

        IEnumerator InteractionCor()
        {
            yield return new WaitForSeconds(InteractionTime);
            if (InteractionAnim != string.Empty)
            {
                Animatorbase.SetTrigger(InteractionAnim);
            }
            InteractionEvent?.Invoke();
        }

        // 애니메이션 Trigger 후 이전 애니메이션 재생
        public void InteractionAfterPrevAnimation()
        {
            Invoke("InvokeAfter", 1.0f);
        }

        void InvokeAfter()
        {
            prevAnimation = true;
        }

        public void SplineRotationInit()
        {
            StartCoroutine("SplineRotationInitCor");
        }

        IEnumerator SplineRotationInitCor()
        {
            SplineFollower follower = GetComponent<SplineFollower>();
            float duration = 2.0f;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                follower.motion.rotationOffset = Vector3.Lerp(follower.motion.rotationOffset, Vector3.zero, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public void BarkAnimation(string key)
        {
            if (curBarkUI != null)
            {
                curBarkUI.BarkPlay(NeutralBarkType.Animation, key);
            }
        }

        public void BarkText(BarkText key)
        {
            if (Managers.Game != null)
            {
                Managers.Game.LanguageModeTrigger(
                    korea: () =>
                    {
                        if (curBarkUI != null)
                        {
                            curBarkUI.BarkPlay(NeutralBarkType.Text, key.KorText);
                        }
                    },
                    english: () =>
                    {
                        if (curBarkUI != null)
                        {
                            curBarkUI.BarkPlay(NeutralBarkType.Text, key.EngText);
                        }
                    });
            }
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [SerializeField] private float StartTime;
        [SerializeField] private string StartAnim;
        [SerializeField] private float EnalbeTime;
        [SerializeField] private string EnableAnim;
        [SerializeField] private string InteractionAnim;
        [SerializeField] private float InteractionTime;
        [SerializeField] private UnityEvent InteractionEvent;
        [SerializeField] private float distance;
        [SerializeField] private UnityEvent DistanceEvent;

        [Space(2.0f), Header("----------Animation-----------")]
        [SerializeField] private float AnimationSpeed = 1.0f;

        [Space(2.0f), Header("----------Bark-----------")]
        [SerializeField] private NeutralNPCBarkUI BarkUI;
        [SerializeField] private Vector3 BarkUIOffset = new Vector3(0, 6.0f, 0);

        private Animator Animatorbase;
        private bool distanceBool = true;
        private bool prevAnimation = false;
        private NeutralNPCBarkUI curBarkUI = null;

        void Awake()
        {
            Animatorbase = GetComponent<Animator>();
        }

        void Start()
        {
            StartCoroutine(StartCor());

            if (BarkUI != null)
            {
                GameObject BarkParent = new GameObject("#BarkParent");
                BarkParent.transform.SetParent(this.transform);
                BarkParent.transform.localPosition = BarkUIOffset;
                BarkParent.transform.localRotation = Quaternion.identity;
                BarkParent.transform.localScale = Vector3.one;
                curBarkUI = Instantiate(BarkUI);
                curBarkUI.transform.SetParent(BarkParent.transform);
                curBarkUI.transform.localPosition = Vector3.zero;
                curBarkUI.transform.localRotation = Quaternion.identity;
                //curBarkUI.transform.localScale = Vector3.one;
            }
        }

        IEnumerator StartCor()
        {
            yield return new WaitForSeconds(StartTime);

            if (StartAnim != string.Empty)
            {
                Animatorbase.SetTrigger(StartAnim);
            }
            if (AnimationSpeed == 0) AnimationSpeed = 1.0f;
            Animatorbase.speed = AnimationSpeed;
        }

        void OnEnable()
        {
            StartCoroutine(EnableCor());
        }

        IEnumerator EnableCor()
        {
            yield return new WaitForSeconds(EnalbeTime);

            if (EnableAnim != string.Empty)
            {
                Animatorbase.SetTrigger(EnableAnim);
            }
        }

        void Update()
        {
            if (Managers.AllCreateManagers == false) return;
            // 애니메이션 Trigger 후 이전 애니메이션 재생 구현
            if (prevAnimation)
            {
                AnimatorStateInfo stateInfo = Animatorbase.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.normalizedTime >= 1.0f) // normalizedTime이 1.0 이면 애니메이션이 종료됨
                {
                    Invoke("InvokeAnimTrigger", StartTime);
                    //Animatorbase.SetTrigger(StartAnim);
                    //prevAnimation = false;
                }
            }

            // Distance 이벤트
            if (distanceBool)
            {
                if (Managers.Unit.GetCurTurnUnit != null)
                {
                    float Distance = Vector3.Distance(this.transform.position, Managers.Unit.GetCurTurnUnit.transform.position);
                    if (Distance < distance && distanceBool)
                    {
                        DistanceEvent?.Invoke();
                        distanceBool = false;
                    }
                }
            }
        }

        void InvokeAnimTrigger()
        {
            Animatorbase.SetTrigger(StartAnim);
            prevAnimation = false;
        }

    }//end of class									
}//end of namespace					