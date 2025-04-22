/*********************************************************					
* DLCDataTable.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.07.16 오후 1:42					
**********************************************************/
using System;
using UnityEngine;					
					
namespace Dev_DLC
{



	[Serializable]
	public class DLCInfo
	{
		public string discription;
        public DLCSkuID skuID;
	}


    [CreateAssetMenu(fileName = "DLC_", menuName = "#ScriptableObject/DLC")]
    public class DLCDataTable : ScriptableObject
    {
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					

        public DLCInfo[] pDLCInfos { get { return dLCInfos; } }
		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
        [SerializeField] private DLCInfo[] dLCInfos;
		

    }//end of class					
}//end of namespace					