/*********************************************************					
* UnitBase.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.03 오후 4:53					
**********************************************************/								
using UnityEngine;
using System;


namespace Dev_Unit
{

	public abstract class UnitBase : MonoBehaviour					
	{

		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		public void InitUnitBase(UnitClass unitClass, UnitUniqueID unitUniqueID, UnitState state)
		{
			InitUnit(state);
			this.UnitClassType = unitClass;
			this.UniqueID = unitUniqueID;
        }


		public UnitClass pUnitClassType { get { return UnitClassType; } }

		public UnitUniqueID pUniqueID { get { return UniqueID; } set { UniqueID = value; } }
		public Transform pOnDamagePivot { get { return OnDamagePivot; } }
		public Transform pWeaponPivot { get { return WeaponPivot; } }

		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		[SerializeField] private UnitClass UnitClassType;
		[SerializeField] private UnitUniqueID UniqueID;
		
		[Header("---------- OnDamaged Pivot Settings ----------")]
		[SerializeField] private Transform OnDamagePivot;     //데미지받는 총알이 생성될 위치 - 리팩토링 필요 : 일방적인 발사체로 변경
		[SerializeField] private Transform WeaponPivot;     //데미지주는 총알이 생성될 위치 - 리팩토링 필요 : 상동

		protected abstract void InitUnit(UnitState state);

	}//end of class					
					
					
}//end of namespace					