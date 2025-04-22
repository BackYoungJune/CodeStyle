/*********************************************************					
* ExerciseManager.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.01.16 오후 2:13					
**********************************************************/
using Dev_System;
using Dev_UI;
using System.Collections;					
using System.Collections.Generic;					
using UnityEngine;					
					
namespace Dev_Unit					
{					
	public class ExerciseManager : MonoBehaviour
	{					
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------		
		public void InitExercise()
        {
            StartCoroutine(InitExerciseCor());
        }

        IEnumerator InitExerciseCor()
        {
            ExerciseObjectNums.Clear();
            objectCount = 0;
            PreExerciseState = ExerciseStage.None;


            lastCount.Clear();
            // 오브젝트 종류가 여러개일경우 Dictionary에 각각 Exercise 종류별로 집어넣는다
            foreach (var exercise in Managers.Unit.UnitExerciseList)
            {
                objectCount++;
                ExerciseStage curExerciseStage = exercise.GetComponent<UnitExercise>().ExerciseStage;
                if (ExerciseObjectNums.ContainsKey(curExerciseStage))
                {
                    ExerciseObjectNums[curExerciseStage] = objectCount;
                }
                else
                {
                    objectCount = 1;
                    ExerciseObjectNums[curExerciseStage] = objectCount;
                }
            }

            // 각 마지막 오브젝트는 bool값 true를 넣어준다
            int firstObjectNum;
            int lastObjectNum = 0;
            foreach (var dic in ExerciseObjectNums)
            {
                lastObjectNum += dic.Value;
                firstObjectNum = lastObjectNum - dic.Value;
                Managers.Unit.UnitExerciseList[lastObjectNum - 1].GetComponent<UnitExercise>().lastExercise = true;
                Managers.Unit.UnitExerciseList[firstObjectNum].GetComponent<UnitExercise>().FirstExercise = true;
            }

            curExercisingNum = 0;

            yield return new WaitUntil(() => Managers.UIManager.GetUIModule("Exercise") != null);
            UIModule_Exercise exericse = Managers.UIManager.GetUIModule("Exercise") as UIModule_Exercise;
            if (exericse != null)
            {
                exericse.Action_Skip -= ExerciseSkip;
                exericse.Action_Skip = null;
                exericse.Action_Skip += ExerciseSkip;
            }
        }


        public void LastExerciseShoot()
        {
            curExercisingNum = 0;
        }

        public void ExerciseSkip()
        {
            if (Managers.Unit.UnitExerciseList.Count <= 0) return;
            int startNum = Managers.Unit.CurTurnExerciseUnitIndex;
            int endNum = Managers.Unit.UnitExerciseList.Count;
            if (startNum < 0)
            {
                startNum = 0;
                Managers.Unit.CurTurnExerciseUnitIndex = 0;
            }
            UnitExercise startExercise = Managers.Unit.UnitExerciseList[startNum].GetComponent<UnitExercise>();
            List<UnitExercise> skipExercise = new List<UnitExercise>();

            for(int i = startNum; i < endNum; i++)
            {
                UnitExercise curExercise = Managers.Unit.UnitExerciseList[i].GetComponent<UnitExercise>();
                if (startExercise.ExerciseStage == curExercise.ExerciseStage)
                {
                    skipExercise.Add(curExercise);
                }

                if (curExercise.lastExercise) break;
            }

            foreach(var exercise in skipExercise)
            {
                exercise.ChangeStateMachine(ExerciseState.Shoot);
            }
        }

        public int curExercisingNum { get; set; }       // 현재 운동 횟수
        public int RemainCount { get; set; }            // 마지막 운동동작 남은 운동횟수
        public Dictionary<ExerciseStage, int> ExerciseObjectNums = new Dictionary<ExerciseStage, int>();
        public static ExerciseManager Instance { get { return instance; } }
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        private int objectCount;                        // ExerciseObjectNums Dictionary에 넣을 int값
        private ExerciseStage PreExerciseState = ExerciseStage.None;    // 이전 오브젝트 ExerciseStage 상태
        private List<int> lastCount = new List<int>();
        private static ExerciseManager instance;


    }//end of class					
}//end of namespace					