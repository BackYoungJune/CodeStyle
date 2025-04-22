/*********************************************************					
* SimplePoolEditor.cs					
* 작성자 : modeunkang					
* 작성일 : 2023.07.26 오후 5:21					
**********************************************************/
using UnityEditor;
					
namespace Dev_System					
{
	[CustomEditor(typeof(ObjectPoolData))]
	public class PoolEditor : Editor					
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					


		//--------------------------------------------------------					
		// 내부 필드 변수					
		//--------------------------------------------------------					
		SerializedProperty PoolType;
        SerializedProperty PoolData;
		SerializedProperty AutoSetting_EnumPool;
		SerializedProperty AutoSetting_Initailizing;

        void OnEnable()
        {
			PoolType = serializedObject.FindProperty("poolType");
            PoolData = serializedObject.FindProperty("poolData");
            AutoSetting_EnumPool = serializedObject.FindProperty("AutoSetting_EnumPool");
            AutoSetting_Initailizing = serializedObject.FindProperty("AutoSetting_Initailizing");
        }

        public override void OnInspectorGUI()
        {
			serializedObject.Update();
			DrawProperties();
            serializedObject.ApplyModifiedProperties();
        }

		void DrawProperties()
		{
			var target_cs = (ObjectPoolData)target;
            EditorGUILayout.PropertyField(PoolType);
			EditorGUILayout.PropertyField(AutoSetting_EnumPool);
            EditorGUILayout.PropertyField(AutoSetting_Initailizing);

            switch ((PoolObjectType)PoolType.enumValueIndex)
			{
				case PoolObjectType.Pool:
					{
						if (target_cs.PoolData == null)
							target_cs.PoolData = new();

						EditorGUILayout.PropertyField(PoolData);

                        if (target_cs.AutoSetting_Initailizing)
                        {
                            PoolUniqueID[] allPool = (PoolUniqueID[])System.Enum.GetValues(typeof(PoolUniqueID));
                            target_cs.PoolData.Clear();

                            for (int i = 0; i < allPool.Length; i++)
                            {
                                PoolDataTable poolTable = new PoolDataTable();
                                if (allPool[i] == PoolUniqueID.None) continue;
                                poolTable.enumKey = allPool[i];
                                poolTable.discription = poolTable.enumKey.ToString();
                                //poolTable.poolSize = 20;

                                target_cs.PoolData.Add(poolTable);
                            }
                        }

                        if (target_cs.AutoSetting_EnumPool)
                        {
                            foreach (var data in target_cs.PoolData)
                            {
                                data.discription = data.enumKey.ToString();
                                //data.poolSize = 20;
                            }
                        }

                        break;
					}
			}
		}


    }//end of class					
}//end of namespace					