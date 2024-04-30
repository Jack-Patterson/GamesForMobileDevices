using UnityEngine;

namespace GamesForMobileDevices.CA_Gestures.Interactable
{
    public class SphereScript : InteractableAbstract
    {
        public override void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
