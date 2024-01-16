using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public class CubeScript : MonoBehaviour, IInteractable
    {
        public void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }
}