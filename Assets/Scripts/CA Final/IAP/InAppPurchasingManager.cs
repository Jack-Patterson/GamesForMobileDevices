using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace GamesForMobileDevices.CA_Final.IAP
{
    public class InAppPurchasingManager : MonoBehaviour, IDetailedStoreListener
    {
        public static InAppPurchasingManager instance;
        
        public IAPItem ModelItem { get; private set; }
        public IAPItem CoinItem { get; private set; }
        public IAPItem DisableAdsItem { get; private set; }

        private ConfigurationBuilder _builder;
        private IStoreController _storeController;
        
        private async void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                .SetEnvironmentName("test");
#else
                .SetEnvironmentName("production");
#endif
            await UnityServices.InitializeAsync(options);
            ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
            operation.completed += HandleIAPCatalogLoaded;
        }
        
        private void HandleIAPCatalogLoaded(AsyncOperation operation)
        {
            ResourceRequest request = operation as ResourceRequest;
            print($"Loaded Asset: {request.asset}");
            
            ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
            print($"Loaded Catalog with {catalog.allProducts.Count} products.");
            
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.fake));

            foreach (ProductCatalogItem item in catalog.allProducts)
            {
                print(item);
                builder.AddProduct(item.id, item.type);
            }
            
            UnityPurchasing.Initialize(this, builder);
        }

        public void Purchase(IAPItem item)
        {
            print(_storeController);
            _storeController.InitiatePurchase(item.ProductId);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.LogError("Initialized successfully." + controller);
            _storeController = controller;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            print($"Initialization failed. Reason: {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            print($"Initialization failed. Reason: {error}\n Message: {message}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Product product = purchaseEvent.purchasedProduct;
            print("Purchase successful.");
            
            if (product.definition.id == IAPItem.modelItem.ProductId)
            {
                GameManager.instance.UseDefaultPlayerModel = false;
                print("Model purchased.");
            }
            else if (product.definition.id == IAPItem.coinItem.ProductId)
            {
                GameManager.instance.AddCoins(10);
                print("Coins purchased.");
            }
            else if (product.definition.id == IAPItem.disableAdsItem.ProductId)
            {
                GameManager.instance.AreAdsEnabled = false;
                print("Ads disabled.");
            }

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            print($"Purchase failed.\n Product: {product.definition.id}\n Reason: {failureReason}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            print($"Purchase failed.\n Product: {product.definition.id}\n Reason: {failureDescription}");
        }
    }
}