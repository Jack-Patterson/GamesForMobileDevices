using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public class SphereScript : MonoBehaviour, IInteractable
    {
        public void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
