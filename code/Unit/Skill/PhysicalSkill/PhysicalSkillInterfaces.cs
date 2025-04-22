/*********************************************************					
* PhysicalSkillInterfaces.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.12.19 오후 7:36					
**********************************************************/					
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;
using Dev_Unit;

namespace Dev_UnitSkill
{


    public class PhysicalFixed : SkillInterface<PhysicalSkill>
    {
        private PhysicalSkill CurPhysicalSkill;

        public void Enter(PhysicalSkill skill)
        {
            CurPhysicalSkill = skill;
        }

        public void UpdateExcute()
        {

        }

        public void Exit()
        {
            
        }

        public IEnumerator Hit()
        {


            CurPhysicalSkill.OnHit();

            yield return null;   
        }

        public void OnTriggerEnter(Collider col)
        {
           
        }
     

    }//end of class











}//end of namespace					