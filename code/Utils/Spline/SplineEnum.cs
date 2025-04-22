/*********************************************************					
* SplineEnum.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/					
using UnityEngine;

namespace Dev_Spline
{
    public enum SplineStage
    {
        None, Run, ROM, Stage, Mission, RunStraight,
    }
    public class SplineEnum : MonoBehaviour
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        [SerializeField] public SplineStage SplineStage;

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					




    }//end of class					
}//end of namespace								