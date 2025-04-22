/*********************************************************					
* DevTimeEvent.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/
using Dev_System;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using UnityEngine.Events;

namespace Dev_Events				
{					
	public class DevTimeEvent : MonoBehaviour					
	{
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------		

        /// <summary>
        /// 단순히 waitTime 대기 후 종료
        /// </summary>
        public void SimpleTimeEvent()
        {
            StartCoroutine(SimpleType(WaitTime));
        }


        /// <summary>
        /// 
        /// </summary>
        public void RunTimeEvent(/*TextMeshProUGUI tmp = null*/)
        {
            if (TimeType == TimeEventType.Ascending)
                StartCoroutine(AscendingType(WaitTime/*, tmp*/));

            else if (TimeType == TimeEventType.Descending)
                StartCoroutine(DescendingType(WaitTime/*, tmp*/));


        }


        public float GetWaitTime
        {
            get { return WaitTime; }
            set { WaitTime = value; }
        }

        public void MeditationGetTime()
        {
            //Managers.Game.LoginModeTrigger(
            //    debugPlayMode: () =>
            //    {
            //        WaitTime = Managers.Game.MeditationPlayTime * 60;
            //    },
            //    loginPlayMode: () =>
            //    {
            //        WaitTime = Managers.Game.GetCurMeditationSaveTime;
            //    });
            WaitTime = Managers.Game.MeditationPlayTime * 60;
            if (WaitTime <= 0)
            {
                WaitTime = 300;
            }

            Debug.Log(string.Format("<b><color=#FF0000>매디테이션 SaveTime = {0}</color></b>", WaitTime));
        }

        public bool unsclaedTime = false;
        public float ProgressTime { get { return progressTime; } }
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        private enum TimeEventType { [InspectorName("오름차순")] Ascending, [InspectorName("내림차순")] Descending }
        [SerializeField] private TimeEventType TimeType;
        [SerializeField] private float WaitTime;
        [SerializeField] private UnityEvent Events;
        [SerializeField] private float RemainTime = 15.0f;
        [SerializeField] private UnityEvent RemainTimeEvent;
        private WaitForSeconds oneSecWait = new WaitForSeconds(1.0f);

        private float progressTime = 0;
        private bool remainCheck = false;

        void OnDisable()
        {
            StopAllCoroutines();
        }



        IEnumerator SimpleType(float wait)
        {
            yield return new WaitForSeconds(wait);
            Events.Invoke();
        }

        IEnumerator AscendingType(float wait, TMPro.TextMeshProUGUI tmp = null)
        {
            float progress = 0.0f;
            if (tmp != null) tmp.text = progress.ToString();

            yield return oneSecWait;

            while (progress <= wait + 1)
            {
                if (tmp != null)
                    tmp.text = ((int)progress).ToString();

                if (unsclaedTime)
                {
                    progress += Time.unscaledDeltaTime;
                }
                else
                {
                    progress += Time.deltaTime;
                }
                progressTime = progress;
                if (WaitTime - progressTime <= RemainTime && !remainCheck)
                {
                    RemainTimeEvent?.Invoke();
                    remainCheck = true;
                }

                yield return null;
            }
            progressTime = progress;
            Events.Invoke();
        }


        IEnumerator DescendingType(float wait, TMPro.TextMeshProUGUI tmp = null)
        {
            float progress = wait;
            if (tmp != null) tmp.text = wait.ToString();

            yield return oneSecWait;

            while (progress > 0.0f)
            {
                if (tmp != null)
                    tmp.text = ((int)progress).ToString();

                if (unsclaedTime)
                {
                    progress -= Time.unscaledDeltaTime;
                }
                else
                {
                    progress -= Time.deltaTime;
                }

                progressTime = progress;
                if (progressTime <= RemainTime && !remainCheck)
                {
                    RemainTimeEvent?.Invoke();
                    remainCheck = true;
                }
                yield return null;
            }
            progressTime = progress;
            Events.Invoke();
        }


    }//end of class						
}//end of namespace					