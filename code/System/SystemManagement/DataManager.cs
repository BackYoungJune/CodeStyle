/*********************************************************					
* DataManager.cs					
* 작성자 : #AUTHOR#					
* 작성일 : #DATE#					
**********************************************************/									
using System.Collections.Generic;					
using UnityEngine;
using System;

namespace Dev_Data				
{			
	public interface ILoader<Key, Value>
	{
		Dictionary<Key, Value> MakeDict();
	}

    public class DataManager : MonoBehaviour				
	{
		//--------------------------------------------------------					
		// 외부 참조 함수 & 프로퍼티					
		//--------------------------------------------------------					
		
		public Dictionary<int, SoundData> SoundDic { get; private set; } = new Dictionary<int, SoundData>();
        public Dictionary<int, SkillData> SkillDic { get; private set; } = new Dictionary<int, SkillData>();

        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					


        void Start()
        {
            Init();
        }

        void Init()
        {
            SoundDic = LoadCSV<SoundData>("SoundData");
            SkillDic = LoadCSV<SkillData>("SkillData");
        }

        Dictionary<int, T>LoadCSV<T>(string fileName) where T : class, new()
        {
            var allDatas = CSVReader.Read(fileName);
            Dictionary<int, T> dict = new Dictionary<int, T>();

            foreach (var data in allDatas)
            {
                T classData = new T();

                // T 형식의 프로퍼티와 CSV 데이터 매핑
                foreach (var property in typeof(T).GetFields())
                {
                    if (data.TryGetValue(property.Name, out object value))
                    {
                        property.SetValue(classData, Convert.ChangeType(value, property.FieldType));
                    }
                }

                // DataID 가져오기
                if (data.TryGetValue("DataID", out object dataIDObj) && dataIDObj is int dataID)
                {
                    dict.Add(dataID, classData);
                }
                else
                {
                    Debug.LogError("[DataManger] : DataID 다시 셋팅하시오");
                }
            }

            return dict;
        }
    }//end of class					
}//end of namespace					