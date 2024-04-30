using System;

namespace GamesForMobileDevices.CA_Final.IAP
{
    [Serializable]
    public class IAPItem
    {
        public static IAPItem modelItem = new IAPItem("player_model_new", "New Model", "Unlock a new model", 5);
        public static IAPItem coinItem = new IAPItem("10_coins", "10 Coins", "Get 10 coins", 2);
        public static IAPItem disableAdsItem = new IAPItem("ads_disable", "Disable Ads", "Remove ads from the game", 4);
        
        public string ProductId { set; get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public IAPItem(string productId, string name, string description, int price)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}