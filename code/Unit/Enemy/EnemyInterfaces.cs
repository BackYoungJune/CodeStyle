/*********************************************************					
* EnemyInterfaces.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.29 오후 1:31					
**********************************************************/									
using UnityEngine;					
					
				
namespace Dev_Unit
{
    public class EnemyBattleIdle : UnitInterface<UnitEnemy>
    {
        private UnitEnemy CurEnemy;

        public void Enter(UnitEnemy unit)
        {
            CurEnemy = unit;
        }

        public void UpdateExcute()
        {
            if (CurEnemy.pAnimatorbase.GetBool(CurEnemy.pAnimatorIDTable.AnimID_OnBattle) == false)
            {
                //에너미 전투Idle True
                CurEnemy.pAnimatorbase.SetBool(CurEnemy.pAnimatorIDTable.AnimID_OnBattle, true);
            }

        }

        public void Exit()
        {
            //에너미 전투Idle False
            CurEnemy.pAnimatorbase.SetBool(CurEnemy.pAnimatorIDTable.AnimID_OnBattle, false);
        }

        public void OnTriggerEnter(Collider col)
        {
            //공격받으면 상태변경 - Hit
            if(col.CompareTag(CurEnemy.AttackerTag))
            {
                CurEnemy.ChangeStateMachine(UnitState.BattleHit);
            }


        }

    }//end of class


    public class EnemyBattleAttack : UnitInterface<UnitEnemy>
    {
        private UnitEnemy CurEnemy;


        public void Enter(UnitEnemy unit)
        {
            CurEnemy = unit;

            //에너미 공격
            CurEnemy.pAnimatorbase.SetTrigger(CurEnemy.pAnimatorIDTable.AnimID_AttackThrow);

            //총알발사!
            CurEnemy.OnAttack(CurEnemy.pWeaponPivot);

            //공격연출 끝나면 state변경 -> BattleIdle
            CurEnemy.ChangeStateMachine(UnitState.BattleIdle);
        }

        public void UpdateExcute()
        {

        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider col)
        {

        }

    }//end of class


    public class EnemyBattleHit : UnitInterface<UnitEnemy>
    {
        private UnitEnemy CurEnemy;

        public void Enter(UnitEnemy unit)
        {
            CurEnemy = unit;

            //에너미 히트
            CurEnemy.OnDamage();

            //히트연출 끝나면 state변경 -> BattleIdle
            CurEnemy.ChangeStateMachine(UnitState.BattleIdle);
        }

        public void UpdateExcute()
        {
          
        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider col)
        {


        }

    }//end of class


    public class EnemyBattleDead : UnitInterface<UnitEnemy>
    {
        private UnitEnemy CurEnemy;

        public void Enter(UnitEnemy unit)
        {
            CurEnemy = unit;

            CurEnemy.Dead();
        }

        public void UpdateExcute()
        {

        }

        public void Exit()
        {
           
        }

        public void OnTriggerEnter(Collider col)
        {
           
        }

      
    }//end of class



}//end of namespace					