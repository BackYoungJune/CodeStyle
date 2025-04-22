/*********************************************************					
* CettiBayScene_Controller.cs					
* 작성자 : skjo					
* 작성일 : 2024.09.19 오후 3:38					
**********************************************************/
using Dev_System;
using Dev_Unit;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// **************Namespace를 반드시 수정하세요**************					
namespace Dev_SceneControl
{
    public class CettiBayScene_Controller : AdventureSceneController
    {

        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public override void Mission1_진입()
        {
            base.Mission1_진입();
            base.Phase_전투1_Init();
        }

        public override void Mission1_운동시작()
        {
            base.Mission1_운동시작();
            base.Phase_전투1();
        }

        public override void Mission2_진입()
        {
            base.Mission2_진입();
            base.Phase_미션진입();
        }

        public override void Mission2_운동시작()
        {
            base.Mission2_운동시작();
            base.Phase_미션시작();
        }

        public override void Mission3_진입()
        {
            base.Mission3_진입();
            base.Phase_전투2_Init();
        }

        public override void Mission3_운동시작()
        {
            base.Mission3_운동시작();
            base.Phase_전투2();
        }

        protected override void Phase_전투2_Init()
        {
            base.turnState = TurnState.EnemyTurn;
            base.Phase_전투2_Init();
        }

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					

        [SerializeField] private MeshRenderer Gazebo;
        [SerializeField] private Material AengduCutscne_Material;
        [SerializeField] private Material AengduBattle_Material;
        [SerializeField] private Volume Volume;

        Coroutine getEnemerator;

        public void AmbientColorChange()
        {
            //StopCoroutine("RenderColorChange");
            //StartCoroutine("RenderColorChange");
            //RenderSettings.skybox = DarkMaterial;
            //Char_Light.intensity = 0;

            if (getEnemerator != null)
            {
                StopCoroutine(getEnemerator);
            }
            getEnemerator = StartCoroutine("PostExposure");

        }

        IEnumerator PostExposure()
        {
            //ColorGrading colorGrading;
            //Volume.profile.TryGet(out colorGrading);
            Volume.profile.TryGet(out ColorAdjustments colorAdjustments);
            Volume.profile.TryGet(out Bloom bloom);

            float duration = 2f;
            float elapsedTime = 0;
            float desValue = -1f;

            float durationPercent = 1 / duration;

            bloom.active = true;

            while (elapsedTime < duration)
            {
                float t = elapsedTime * durationPercent;
                colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, desValue, t);
                colorAdjustments.contrast.value = Mathf.Lerp(colorAdjustments.contrast.value, 40f, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            colorAdjustments.postExposure.value = desValue;
            colorAdjustments.contrast.value = 40f;
            
        }

        IEnumerator RenderColorChange()
        {
            Color ambientColor = RenderSettings.ambientLight;
            Color targetColor = new Color(0, 0, 0);
            //Debug.LogError("Ambient Color : " + ambientColor);

            float duration = 2f;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                ambientColor = Color.Lerp(ambientColor, targetColor, t);
                RenderSettings.ambientLight = ambientColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            RenderSettings.ambientLight = targetColor;
        }

        public void AengduCutScene()
        {
            Gazebo.material = AengduCutscne_Material;
        }

        public void AengduBattle()
        {
            Gazebo.material = AengduBattle_Material;
        }

        public void NPCSplineLeftMove()
        {
            StopCoroutine("NPCLeftMove");
            StartCoroutine("NPCLeftMove");
            //base.FollowNPC.Follower.motion._offset.x = -0.56f;
        }

        IEnumerator NPCLeftMove()
        {
            float duration = 1f;
            float elapsedTime = 0;

            float desOffset = -0.56f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                base.FollowNPC.Follower.motion._offset.x = Mathf.Lerp(base.FollowNPC.Follower.motion._offset.x, desOffset, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            base.FollowNPC.Follower.motion._offset.x = desOffset;
        }

        protected override void CreateMission()
        {
            foreach (var genpos in GenPos_MissionObjects)
            {
                Managers.Unit.CreateUnit(UnitUniqueID.Exercise_Fishing, UnitState.None, genpos, true, Vector3.one);
            }
        }

        protected override void Start()
        {
            base.Start();
        }


        protected override void Update()
        {
            base.Update();
        }



    }//end of class					


}//end of namespace					