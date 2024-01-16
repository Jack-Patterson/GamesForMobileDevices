using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class GestureAction : MonoBehaviour
    {
        internal void TapAt(Vector2 touchPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.ProcessTap();
                }
            }
        }
    }
}
