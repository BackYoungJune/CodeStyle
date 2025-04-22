/*********************************************************					
* DLCManager.cs					
* 작성자 : YoungJune					
* 작성일 : 2024.07.16 오후 1:22					
**********************************************************/
using Dev_System;
using NUnit.Framework;
using Oculus.Platform;
using Oculus.Platform.Models;
using PixelCrushers.DialogueSystem.UnityGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dev_DLC
{


    public class DLCInfoStruct
    {
        public string Name;                 // DLC 이름
        public bool Purchase;               // 구매 여부
        public string price;                 // 가격
        public string discription;          // 설명

        public DLCInfoStruct(string _name = "", bool _purchase = false, string _price = "", string _discription = "")
        {
            Name = _name;
            Purchase = _purchase;
            price = _price;
            discription = _discription;
        }
    }

    public class DLCManager : MonoBehaviour
    {
        //--------------------------------------------------------					
        // 외부 참조 함수 & 프로퍼티					
        //--------------------------------------------------------	

        // DLC 구매 메서드
        public void PurchaseDLC(DLCSkuID sku)
        {
            DevOculus.PurchaseItem(sku.ToString(), PurchaseCallback);
        }

        // 구매 콜백 메서드
        void PurchaseCallback(Message<Purchase> msg)
        {
            if (msg.IsError)
            {
                Debug.LogError("Purchase failed: " + msg.GetError().Message);
                PurchaseFail?.Invoke();
                return;
            }

            Debug.Log("Purchase successful1: " + msg.Data.Sku);
            Debug.Log("Purchase successful2: " + skuDic.Count);
            foreach (var dic in skuDic)
            {
                Debug.Log("Purchase successful3: " + dic.Key.ToString());
            }
            DLCSkuID sku = DevUtils.StringToEnum<DLCSkuID>(msg.Data.Sku);
            // 구매 성공 후 처리
            if (skuDic.ContainsKey(sku))
            {
                skuDic[sku].Purchase = true;
                Debug.Log("Purchase successful4: " + skuDic[sku].Name);
                Debug.Log("Purchase successful4: " + skuDic[sku].Purchase);
                PurchaseSucess?.Invoke();
            }
        }

        public void DLCInitNetwork()
        {
            if (networkInit == null && AllNetwork == false)
            {
                networkInit = StartCoroutine(DLCInitNetworkCor());
            }
        }

        IEnumerator DLCInitNetworkCor()
        {
            yield return new WaitUntil(() => UnityEngine.Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
UnityEngine.Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork);

            // Oculus 플랫폼 초기화
            DevOculus.PlatformInit();
            yield return new WaitUntil(() => DevOculus.dlcState == DevOculus.DLCState.Init);

            // 등록된 SKU 목록을 가져오는 메서드
            FetchProductPrices();
            yield return new WaitUntil(() => DevOculus.dlcState == DevOculus.DLCState.ProductPrices);

            // 구매 상태 확인
            DevOculus.CheckPurchaseStatus(GetPurchasesCallback);
            yield return new WaitUntil(() => DevOculus.dlcState == DevOculus.DLCState.CheckPurchase);

            // 사용자 연령대 정보 가져오기
            GetUserAgeCategory();
            yield return new WaitUntil(() => DevOculus.dlcState == DevOculus.DLCState.GetAgeCategory);

            Managers.AllClearDLC = true;
            AllNetwork = true;
        }

        public Dictionary<DLCSkuID, DLCInfoStruct> pSkuDic { get { return skuDic; } }
        public Action PurchaseSucess = null;
        public Action PurchaseFail = null;
        public bool pAllNetwork { get { return AllNetwork; } }
        //--------------------------------------------------------					
        // 내부 필드 변수					
        //--------------------------------------------------------		
        [SerializeField] private DLCDataTable dlcTable;
        private Dictionary<DLCSkuID, DLCInfoStruct> skuDic = new();

        private int retryCount = 0;
        const int maxRetryCount = 3;
        private bool AllNetwork = false;

        Coroutine networkInit = null;

        void Awake()
        {
            DLCInit();
        }

        void DLCInit()
        {
            Managers.AllClearDLC = false;
            PurchaseSucess = null;
            PurchaseFail = null;

            DLCDictionarynit();

            bool internetPossible = UnityEngine.Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
    UnityEngine.Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ? true : false;

            // 인터넷 끊겨있을 경우
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                Managers.AllClearDLC = true;
            }

            networkInit = StartCoroutine(DLCInitNetworkCor());
        }

        void DLCDictionarynit()
        {
            // DLCSkuID enum 값을 string 배열로 변환
            string[] dlcSkuStrings = Enum.GetNames(typeof(DLCSkuID));

            // None 값을 제외한 새로운 배열 생성
            string[] filteredDlcSkuStrings = dlcSkuStrings
                .Where(sku => sku != DLCSkuID.None.ToString())
                .ToArray();

            // 기존 skuDic 초기화
            if (skuDic == null) skuDic = new();
            skuDic.Clear();
            foreach (var dlcStr in filteredDlcSkuStrings)
            {
                skuDic.Add(DevUtils.StringToEnum<DLCSkuID>(dlcStr), new DLCInfoStruct());
            }
        }

        // 등록된 SKU 목록을 가져오는 메서드
        void FetchProductPrices()
        {
            string[] nameArray = skuDic.Keys.Select(key => key.ToString()).ToArray();

            // 등록 SKU 목록 가져오기
            DevOculus.GetProductSKU(nameArray, GetProductsBySKUCallback);
        }

        // SKU 목록 가져오기
        void GetProductsBySKUCallback(Message<ProductList> msg)
        {
            if (msg.IsError)
            {
                Debug.LogError("Error checking get product sku " + msg.GetError().Message);
                return;
            }

            // skuDic 초기화 및 셋팅
            foreach (Product p in msg.GetProductList())
            {
                Debug.LogFormat("Product: sku:{0} name:{1} price:{2} discription:{3}", p.Sku, p.Name, p.FormattedPrice, p.Description);
                DLCSkuID skuId = DevUtils.StringToEnum<DLCSkuID>(p.Sku);
                DLCInfoStruct dlcInfo = new DLCInfoStruct(
                _name: p.Name,
                _purchase: false,   // 나중에 구매목록 가져올 때 구매한 사람들은 true로 바꿔줄거임
                _price: p.FormattedPrice,
                _discription: "Short description for " + p.Description);
                if (skuDic.ContainsKey(skuId))
                {
                    skuDic[skuId] = dlcInfo;    // 기존 키의 값 업데이트
                }
                else
                {
                    skuDic.Add(DevUtils.StringToEnum<DLCSkuID>(p.Sku), value: dlcInfo);
                }
                Debug.LogFormat("Dic: name:{0} price:{1} discription:{2}", skuDic[skuId].Name, skuDic[skuId].price, skuDic[skuId].discription);
            };
        }

        // 구매 목록을 가져온 후 호출되는 콜백 메서드
        void GetPurchasesCallback(Message<PurchaseList> msg)
        {
            // 오류가 발생하면 오류 메시지를 출력하고 메서드를 종료
            if (msg.IsError)
            {
                Debug.LogError("Error checking purchase status: " + msg.GetError().Message);
                return;
            }

            // 구매 목록 데이터를 가져옴
            PurchaseList purchases = msg.Data;
            ProcessPurchases(purchases);
        }

        void ProcessPurchases(PurchaseList purchases)
        {
            // 구매 목록에서 DLCDataTable에 있는 SKU 배열에 있는지 확인하고, purchase를 true로 바꿔준다
            Debug.Log("process purchase successfully : " + purchases);
            foreach (var purchase in purchases)
            {
                var dlc = System.Array.Find(dlcTable.pDLCInfos, x => x.skuID.ToString() == purchase.Sku);
                if (dlc != null)
                {
                    if (skuDic.ContainsKey(dlc.skuID))
                    {
                        skuDic[dlc.skuID].Purchase = true;
                        Debug.Log("skuDic[dlc.skuID].Purchase : " + dlc.skuID.ToString());
                        Debug.Log("skuDic[dlc.skuID].Purchase : " + skuDic[dlc.skuID].Name);
                        Debug.Log("skuDic[dlc.skuID].Purchase : " + skuDic[dlc.skuID].price);
                        Debug.Log("skuDic[dlc.skuID].Purchase : " + skuDic[dlc.skuID].Purchase);
                        Debug.Log("skuDic[dlc.skuID].Purchase : " + skuDic[dlc.skuID].discription);
                    }
                    else
                    {
                        Debug.LogWarning("skuDic에 해당 SKU ID가 없습니다: " + dlc.skuID.ToString());
                    }
                }
                else
                {
                    Debug.LogWarning("DLCDataTable에서 해당 SKU를 찾을 수 없습니다: " + purchase.Sku);
                }
            }
        }

        void GetUserAgeCategory()
        {
            // 사용자 연령대 정보를 캐시에서 가져옴
            string cachedAgeCategory = PlayerPrefs.GetString("UserAgeCategory", null);

            // 인터넷 연결 상태 확인
            if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
            {
                // 인터넷 연결이 없고 캐시된 데이터가 있는 경우
                if (!string.IsNullOrEmpty(cachedAgeCategory))
                {
                    Debug.Log("캐시된 사용자 연령대: " + cachedAgeCategory);
                    // 캐시된 연령대 사용
                    HandleAgeCategory(cachedAgeCategory);
                }
                else
                {
                    Debug.LogWarning("인터넷 연결이 없고 캐시된 데이터가 없습니다.");
                    // 기본 연령대 사용 (필요 시)
                    HandleAgeCategory("Unknown");
                }
            }
            else
            {
                // 인터넷 연결이 있는 경우 API 호출
                DevOculus.GetAgeCategory(GetAgeCategoryCallback);
            }
        }

        void GetAgeCategoryCallback(Message<UserAccountAgeCategory> msg)
        {
            if (msg.IsError)
            {
                // API 호출이 실패한 경우 에러 로그 출력
                Debug.LogError("연령대 정보를 가져오는 데 실패했습니다: " + msg.GetError().Message);
                // 캐시된 데이터 사용 또는 기본 값
                // UserAgeCategory에 저장된 값을 가져오고 없으면 Unknown을 반환한다.
                string cachedAgeCategory = PlayerPrefs.GetString("UserAgeCategory", "Unknown");
                HandleAgeCategory(cachedAgeCategory);

                if (retryCount < maxRetryCount)
                {
                    retryCount++;
                    Debug.Log("연령대 정보를 다시 시도합니다. 재시도 횟수: " + retryCount);
                    GetUserAgeCategory();
                }
                else
                {
                    Debug.LogError("최대 재시도 횟수에 도달했습니다.");
                }

                return;
            }

            // API 호출이 성공한 경우 연령대 정보 저장
            string ageCategory = msg.Data.AgeCategory.ToString();
            Debug.Log("사용자 연령대: " + ageCategory);

            // 연령대 정보를 캐시에 저장
            PlayerPrefs.SetString("UserAgeCategory", ageCategory);
            PlayerPrefs.Save();

            Debug.Log("PlayerPrefabs 연령대: " + PlayerPrefs.GetString("UserAgeCategory"));

            // 연령대 정보 처리
            HandleAgeCategory(ageCategory);
        }

        // 연령대에 따른 앱 로직 처리

        void HandleAgeCategory(string ageCategory)
        {
            // 현재 인터넷 x, 캐시 데이터x, API 호출 실패 시 Unknown 으로 들어오고 나머진 
            // 사용자가 오프라인이거나 API 호출이 실패할 때 앱 사용자는 차단되어서는 안 됩니다. (Unknown은 차단하지 않는다)
            // 1. Unknown: 연령대가 알려지지 않은 사용자 계정입니다.
            // 2. Ch: 어린이(Children) 계정입니다.
            // 3. Tn: 청소년(Teen) 계정입니다.
            // 4. Ad: 성인(Adult) 계정입니다.
            DevOculus.dlcState = DevOculus.DLCState.GetAgeCategory;
            Debug.Log("처리된 연령대: " + ageCategory);
        }

        void OnDisable()
        {
            PurchaseSucess = null;
            PurchaseFail = null;
        }

    }//end of class					
}//end of namespace					