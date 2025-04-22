using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine.UI;

// 이 클래스는 애플리케이션의 인앱 결제(IAP)를 조정합니다. Meta Quest 대시보드에서 IAP를 설정하는 방법에 대한 지침은 Readme를 참조하십시오. 
// 데모에서 사용되는 소비성 IAP 항목은 하나뿐입니다: 파워볼!
public class IAPManager : MonoBehaviour
{
    // 사용자가 더 많은 파워볼을 구매했을 때 알릴 게임 컨트롤러
    //[SerializeField] private GameController m_gameController;

    // IAP 항목의 현재 가격을 표시할 위치
    [SerializeField] private Text m_priceText;

    // Meta Quest 대시보드에서 구성한 구매 가능한 IAP 제품
    private const string CONSUMABLE_1 = "PowerballPack1";

    void Start()
    {
        FetchProductPrices();
        FetchPurchasedProducts();
    }

    // 구성된 IAP 항목의 현재 가격을 가져옴
    public void FetchProductPrices()
    {
        string[] skus = { CONSUMABLE_1 };
        IAP.GetProductsBySKU(skus).OnComplete(GetProductsBySKUCallback);
    }

    void GetProductsBySKUCallback(Message<ProductList> msg)
    {
        if (msg.IsError)
        {
            Debug.LogError(msg);
            return;
        }

        foreach (Product p in msg.GetProductList())
        {
            Debug.LogFormat("Product: sku:{0} name:{1} price:{2}", p.Sku, p.Name, p.FormattedPrice);
            if (p.Sku == CONSUMABLE_1)
            {
                m_priceText.text = p.FormattedPrice;
            }
        }
    }

    // Durable로 구매한 IAP 항목을 가져옵니다. 샘플을 확장하여 이를 포함하지 않는 한 반환되지 않아야 합니다.
    public void FetchPurchasedProducts()
    {
        IAP.GetViewerPurchases().OnComplete(GetViewerPurchasesCallback);
    }

    void GetViewerPurchasesCallback(Message<PurchaseList> msg)
    {
        if (msg.IsError)
        {
            Debug.LogError(msg);
            return;
        }

        foreach (Purchase p in msg.GetPurchaseList())
        {
            Debug.LogFormat("Purchased: sku:{0} granttime:{1} id:{2}", p.Sku, p.GrantTime, p.ID);
        }
    }

    public void BuyPowerBallsPressed()
    {
#if UNITY_EDITOR
        //m_gameController.AddPowerballs(1);
#else
        IAP.LaunchCheckoutFlow(CONSUMABLE_1).OnComplete(LaunchCheckoutFlowCallback);
#endif
    }

    private void LaunchCheckoutFlowCallback(Message<Purchase> msg)
    {
        if (msg.IsError)
        {
            Debug.LogError(msg);
            return;
        }

        Purchase p = msg.GetPurchase();
        Debug.Log("purchased " + p.Sku);
        //m_gameController.AddPowerballs(3);
    }
}