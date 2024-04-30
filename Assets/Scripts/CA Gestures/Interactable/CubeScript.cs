using UnityEngine;

namespace GamesForMobileDevices.CA_Gestures.Interactable
{
    public class CubeScript : InteractableAbstract
    {
        public override void ProcessTap()
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }
}