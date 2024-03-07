using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public class CubeScript : InteractableAbstract
    {
        public override void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }
}