using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GamesForMobileDevices.CA_Final.UI
{
    public class UIManager : MonoBehaviour
    {
        protected static GameManager GameManager => GameManager.Instance;

        protected void Start()
        {
            AssignButtons();
            SubscribeText();
            AssignInitialText();
        }

        protected virtual void AssignButtons()
        {
            Button backButton = GetComponentsInChildren<Button>().ToList()
                .FirstOrDefault(button => button.name == "BackButton");
            if (backButton != null)
                backButton.onClick.AddListener(() => GameManager.SwitchToScene("MainMenu"));
        }

        protected virtual void SubscribeText()
        {
        }
        
        protected virtual void AssignInitialText()
        {
        }
    }
}