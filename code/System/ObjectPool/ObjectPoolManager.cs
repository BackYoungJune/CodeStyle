/*********************************************************					
* SimpleObjectPoolManager.cs					
* 작성자 : SeoJin					
* 작성일 : 2022.11.30 오후 10:57					
**********************************************************/
using Dev_UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Dev_System
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public struct UIInfo
        {
            public UISceneTypes type;
            public string uniqueID;
        }


        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------					
        public GameObject GetObjectFromPool(PoolObjectType _type, PoolUniqueID _uniqueID, Transform _parent = null)
        {
            var data = System.Array.Find(ObjectPoolData, x => x.PoolType == _type);
            if (data == null)
            {
                Debug.LogError("[PoolManager] : 해당 type의 data가 검색되지 않았습니다.");
                return null;
            }

            var obj = data.PoolData.Find(x => x.enumKey == _uniqueID);
            if (obj == null)
            {
                Debug.LogError("[PoolManager] : 해당 uniqueID의 table이 검색되지 않았습니다.");
                return null;
            }

            GameObject select = null;

            if (!uniquePoolDic.ContainsKey(_uniqueID))
            {
                uniquePoolDic.Add(_uniqueID, new Queue<GameObject>());
            }

            foreach (var item in uniquePoolDic[_uniqueID])
            {
                if(item == null)
                {
                    select = Instantiate(obj.prefab, _parent);
                    break;
                }

                if (!item.activeSelf)
                {
                    select = item;
                    break;
                }
            }

            if (select == null)
            {
                select = Instantiate(obj.prefab, _parent);
                uniquePoolDic[_uniqueID].Enqueue(select);
            }

            if (parentPoolDic.ContainsKey(_type) && _parent == null)
            {
                select.transform.SetParent(parentPoolDic[_type].transform);
            }
            else if (_parent)
            {
                select.transform.SetParent(_parent);
            }

            select.SetActive(true);
            return select;
        }

        public GameObject GetUIFromPool(UISceneTypes type, string uniqueID, Vector3 genpos = default, Transform _parent = null)
        {
            UIModuleInfo enableUI = null;
            UIModuleSO so = Array.Find(Managers.UIManager.pUIModuleSOs, element =>
            {
                enableUI = element.GetUIModule(type, uniqueID);
                return enableUI != null;
            });

            if (enableUI == null)
            {
                Debug.LogError("[PoolManager] : 해당하는 UI Info가 없습니다.");
                return null;
            }

            GameObject select = null;
            UIInfo info = new UIInfo();
            info.type = type;
            info.uniqueID = uniqueID;

            if (!UIPoolDic.ContainsKey(info))
            {
                UIPoolDic.Add(info, new Queue<GameObject>());
            }

            foreach (var item in UIPoolDic[info])
            {
                if (item == null)
                {
                    select = Instantiate(enableUI.pUIModulePrefab.gameObject, _parent);
                    break;
                }

                if (!item.activeSelf)
                {
                    select = item;
                    break;
                }
            }

            if (select == null)
            {
                select = Instantiate(enableUI.pUIModulePrefab.gameObject, _parent);
                UIPoolDic[info].Enqueue(select);
            }

            if (parentPoolDic.ContainsKey(PoolObjectType.UI) && _parent == null)
            {
                select.transform.SetParent(parentPoolDic[PoolObjectType.UI].transform);
                select.transform.SetLocalPositionAndRotation(genpos, Quaternion.identity);
            }
            else if (_parent)
            {
                select.transform.SetParent(_parent);
                select.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            select.SetActive(true);
            return select;
        }

        public void ReturnObjectPool(PoolObjectType _objectID, GameObject obj)
        {
            if (!parentPoolDic.ContainsKey(_objectID))
            {
                Debug.LogError("[PoolManager] : 해당하는 Object Parnet가 없습니다.");
                return;
            }

            obj.SetActive(false);
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            //obj.transform.localScale = Vector3.one;
            obj.transform.SetParent(parentPoolDic[_objectID].transform);
        }

        public IEnumerator AllPoolInit()
        {
            if(uniquePoolDic != null)
            {
                foreach (var pool in uniquePoolDic)
                {
                    foreach (var obj in pool.Value)
                    {
                        if (obj != null)
                        {
                            obj.gameObject.SetActive(false);
                            obj.transform.SetParent(parentPoolDic[PoolObjectType.Pool].transform);
                        }
                    }
                }
            }

            if(UIPoolDic != null)
            {
                yield return StartCoroutine(Managers.UIManager.AllDisableUIModule());
                foreach (var pool in UIPoolDic)
                {
                    foreach(var obj in pool.Value)
                    {
                        if (obj != null)
                        {
                            obj.gameObject.SetActive(false);
                            obj.transform.SetParent(parentPoolDic[PoolObjectType.UI].transform);
                        }
                    }
                }    
            }

            AllclearEvent?.Invoke();
        }

        public Queue<GameObject> ReturnUIPoolObj(UIInfo info)
        {
            return UIPoolDic[info] == null ? null : UIPoolDic[info];
        }

        public GameObject ReturnUIPoolObject(UISceneTypes uiSceneType, string uniqueID)
        {
            UIInfo uiInfo = new UIInfo();
            uiInfo.type = uiSceneType;
            uiInfo.uniqueID = uniqueID;

            return Managers.ObjectPool.ReturnUIPoolObj(uiInfo).Dequeue();
        }

        public UnityAction AllclearEvent;
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------					
        [Header("-------------Pool Data Setting-------------")]
        [SerializeField] private ObjectPoolData[]         ObjectPoolData;
        private Dictionary<PoolObjectType, GameObject>          parentPoolDic = new();
        private Dictionary<PoolUniqueID, Queue<GameObject>>     uniquePoolDic = new();
        private Dictionary<UIInfo, Queue<GameObject>>           UIPoolDic = new();

        void Awake()
        {
            InitPoolSetting();
        }

        void InitPoolSetting()
        {
            foreach (var data in ObjectPoolData)
            {
                if(data.PoolType == PoolObjectType.None) 
                    continue;

                GameObject obj = new GameObject(string.Format("#{0}", data.PoolType.ToString()));
                obj.transform.SetParent(this.transform);

                parentPoolDic.Add(data.PoolType, obj);
            }

            // UI는 PoolData에 안넣고 외부에서 호출
            GameObject obj2 = new GameObject(string.Format("#{0}", PoolObjectType.UI.ToString()));
            obj2.transform.SetParent(this.transform);

            parentPoolDic.Add(PoolObjectType.UI, obj2);
        }

        void AllObjectPoolSetParnet()
        {
            foreach(var pool in uniquePoolDic)
            {
                foreach(var obj in pool.Value)
                {
                    // 해당 obj가 파괴되지않고 남아있다면
                    if(obj != null)
                    {
                        obj.transform.SetParent(parentPoolDic[PoolObjectType.Pool].transform);
                    }
                    // 해당 obj가 파괴된 상태라면
                    else
                    {
                        // 해당 queue에서 해당 원소를 제거한다
                        GameObject removeObj = obj;
                        for(int i = 0; i < pool.Value.Count; i++)
                        {
                            GameObject curObj = pool.Value.Dequeue();
                            if(removeObj != curObj)
                            {
                                pool.Value.Enqueue(curObj);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    
                }
            }
        }


    }//end of class					


}//end of namespace					