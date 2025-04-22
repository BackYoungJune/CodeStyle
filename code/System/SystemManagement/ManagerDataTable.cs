using System;
using UnityEngine;

namespace Dev_System
{
    [Serializable]
    public class ManagerPrefabInfo
    {
        public string name;
        public GameObject Prefab;
        public Manager_UniqueID ID;
    }

    [CreateAssetMenu(fileName = "ManagerDataTable_", menuName = "#ScriptableObject/Managers")]
    public class ManagerDataTable : ScriptableObject
    {
        public ManagerPrefabInfo[] pManagerInfos { get { return ManagerInfos; } }

        [Header("----------Managers Setting Info----------")]
        [SerializeField] private ManagerPrefabInfo[] ManagerInfos;

    }
}