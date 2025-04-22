/*********************************************************					
* ExerciseInterfaces.cs					
* 작성자 : modeunkang					
* 작성일 : 2022.12.21 오후 1:19					
**********************************************************/
using Dev_System;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Unit					
{
    public class ExerciseIdle : UnitInterface<UnitExercise>
    {
        private UnitExercise CurExercise;

        public void Enter(UnitExercise unit)
        {
            CurExercise = unit;
            unit.Idle();
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

    public class ExercisePrepare : UnitInterface<UnitExercise>
    {
        private UnitExercise CurExercise;
        private bool accurate = true;       // 값이 나누어 떨어지면
        private int remainPlusOne;

        public void Enter(UnitExercise unit)
        {
            CurExercise = unit;
            int curObjectNum = Managers.Exercise.ExerciseObjectNums[Managers.Unit.GetCurTargetExerciseUnit.GetComponent<UnitExercise>().ExerciseStage];
            Managers.Exercise.curExercisingNum++;
            CurExercise.ExerciseCount = (int)Managers.Gesture.pCurGestureMaxCount / curObjectNum;
            accurate = (int)Managers.Gesture.pCurGestureMaxCount % curObjectNum == 0 ? true : false;
            // 나누기가 맞아떨어지나 안맞아떨어지나 따로구현
            if (accurate)
            {
                Accurate();
            }
            else
            {
                if (CurExercise.FirstExercise)
                {
                    Managers.Exercise.RemainCount = (int)Managers.Gesture.pCurGestureMaxCount % curObjectNum;
                    CurExercise.FirstExercise = false;
                }

                NoAccuarate();
            }
        }

        void Accurate()
        {
            // 만약 운동횟수가 5회일 경우 바로바로 Shoot을 호출해준다.
            if (CurExercise.ExerciseCount == 1)
            {
                CurExercise.ChangeStateMachine(ExerciseState.Shoot);
                return;
            }

            // 운동 횟수가 10회 이상일 경우
            if (Managers.Exercise.curExercisingNum % CurExercise.ExerciseCount == 0)
            {
                CurExercise.ChangeStateMachine(ExerciseState.Shoot);
            }
            else
            {
                CurExercise.PrePare();
                CurExercise.PrePareEvent?.Invoke();
                CurExercise.ChangeStateMachine(ExerciseState.Idle);
            }
        }

        void NoAccuarate()
        {
            remainPlusOne = Managers.Exercise.RemainCount > 0 ? 1 : 0;

            // 운동 횟수가 10회 이상일 경우
            if (Managers.Exercise.curExercisingNum % (CurExercise.ExerciseCount + remainPlusOne) == 0)
            {
                Managers.Exercise.RemainCount--;
                CurExercise.ChangeStateMachine(ExerciseState.Shoot);
            }
            else
            {
                CurExercise.PrePare();
                CurExercise.PrePareEvent?.Invoke();
                CurExercise.ChangeStateMachine(ExerciseState.Idle);
            }

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

    public class ExerciseShoot : UnitInterface<UnitExercise>
    {
        private UnitExercise CurExercise;

        public void Enter(UnitExercise unit)
        {
            CurExercise = unit;
            int curObjectNum = Managers.Exercise.ExerciseObjectNums[Managers.Unit.GetCurTargetExerciseUnit.GetComponent<UnitExercise>().ExerciseStage];
            Managers.Exercise.curExercisingNum = 0;

            // 각 마지막 운동의 Shoot이라면 (마지막 Index)
            if (Managers.Unit.GetCurTargetExerciseUnit.lastExercise)
            {
                Managers.Exercise.LastExerciseShoot();
            }

            CurExercise.Shoot();
            CurExercise.ShootEvent?.Invoke();

            Managers.Unit.CurTurnExerciseUnitIndex++;

            // 현재 진행된 오브젝트 갯수를 넘을경우 호출
            if (Managers.Unit.CurTurnExerciseUnitIndex >= curObjectNum)
            {
                Managers.Unit.CurTurnExerciseUnitIndex = 0;
                Managers.Exercise.InitExercise();
            }

            CurExercise.ChangeStateMachine(ExerciseState.Idle);
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