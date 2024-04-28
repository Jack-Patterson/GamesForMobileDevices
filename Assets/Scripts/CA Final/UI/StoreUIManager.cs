using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamesForMobileDevices.CA_Final.UI
{
    public class StoreUIManager : UIManager
    {
        [SerializeField] private TMP_Text coinsText;
        
        private new void Start()
        {
            base.Start();
            coinsText.text = $"Coins: {GameManager.Coins}";
        }

        protected override void AssignButtons()
        {
            List<Button> buttons = GetComponentsInChildren<Button>().ToList();
            
            buttons.ForEach(button =>
            {
                if (button.name == "BackButton")
                {
                    button.onClick.AddListener(() => GameManager.SwitchToScene("MainMenu"));
                }
                
                if (button.name == "ModelItem")
                {
                    button.onClick.AddListener(() =>
                    {
                        // Purchase logic
                    });
                }
                
                if (button.name == "10CoinItem")
                {
                    button.onClick.AddListener(() =>
                    {
                        // Purchase logic
                    });
                }
                
                if (button.name == "DisableAds")
                {
                    button.onClick.AddListener(() =>
                    {
                        // Purchase logic
                        GameManager.Instance.AreAdsEnabled = false;
                    });
                }
            });

            
        }
    }
}