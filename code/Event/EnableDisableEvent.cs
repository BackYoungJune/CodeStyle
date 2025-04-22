/*********************************************************
* EnableDisableEvent.cs
* 작성자 : SeoJin
* 작성일 : 2022.04.07 AM 8:23
**********************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Dev_Events
{
    public class EnableDisableEvent : MonoBehaviour
    {
        //--------------------------------------------------------
        //          외부 참조 함수 & 프로퍼티
        //--------------------------------------------------------


        //--------------------------------------------------------
        //          내부 필드 변수
        //--------------------------------------------------------
        [SerializeField] private float          EnableWaitTime = 0;
        [SerializeField] private UnityEvent     EnableEvents;
        [SerializeField] private float          StartWaitTime = 0;
        [SerializeField] private UnityEvent     StartEvents;
        [SerializeField] private UnityEvent     DisableEvents;




        void OnEnable()
        {
            StartCoroutine(IOnEnable());
        }



        IEnumerator IOnEnable()
        {
            yield return new WaitForSeconds(EnableWaitTime);
            EnableEvents?.Invoke();
        }
        
        
        
        IEnumerator Start()
        {
            yield return new WaitForSeconds(StartWaitTime);
            StartEvents?.Invoke();
        }



        void OnDisable()
        {
            if(DisableEvents != null)
            {
                DisableEvents?.Invoke();
            }
        }

        


    }//end of class


}//end of namespace