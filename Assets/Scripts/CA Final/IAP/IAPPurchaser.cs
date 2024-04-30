using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace GamesForMobileDevices.CA_Final.IAP
{
    public class IAPPurchaser : MonoBehaviour
    {
        public void OnProductsFetched(ProductCollection productCollection)
        {
            productCollection.all.ToList().ForEach(product =>
            {
                Debug.Log($"{product.metadata.localizedTitle} - {product.metadata.localizedPriceString} - {product.metadata.localizedDescription}");
            });
        }
        
        public void OnInitFailed(InitializationFailureReason reason, string s)
        {
            Debug.Log($"Initialization failed: {reason}");
        }
        
        public void OnPurchaseCompleted(Product product)
        {
            Debug.Log($"Purchased: {product.metadata.localizedTitle}");
        }
    }
}