using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public class CapsuleScript : InteractableAbstract
    {
        public override void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
