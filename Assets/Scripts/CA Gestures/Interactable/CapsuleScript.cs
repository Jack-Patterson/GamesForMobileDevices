using UnityEngine;

namespace GamesForMobileDevices.CA_Gestures.Interactable
{
    public class CapsuleScript : InteractableAbstract
    {
        public override void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
