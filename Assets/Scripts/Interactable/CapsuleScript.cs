using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public class CapsuleScript : MonoBehaviour, IInteractable
    {
        public void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
