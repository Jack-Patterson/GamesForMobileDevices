using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public class SphereScript : InteractableAbstract
    {
        public override void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }

        public override void ProcessDrag(Vector3 newPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}
