using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public class SphereScript : InteractableAbstract
    {
        public override void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
