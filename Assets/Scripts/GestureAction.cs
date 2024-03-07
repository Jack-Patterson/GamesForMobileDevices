using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class GestureAction : MonoBehaviour
    {
        private Camera MainCamera => Camera.main;

        internal void TapAt(Vector2 touchPosition)
        {
            Ray ray = MainCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable?.ProcessTap();
                }
            }
        }

        internal void DragAt(IInteractable interactable, Vector2 touchPosition, float distance = 0)
        {
            Ray ray = MainCamera.ScreenPointToRay(touchPosition);

            if (distance == 0 && Physics.Raycast(ray, out RaycastHit hit))
            {
                interactable?.ProcessDrag(hit.point);
            }
            else
            {
                interactable?.ProcessDrag(ray.GetPoint(distance));
            }
        }

        internal void DragAtRaycast(IInteractable interactable, Vector2 touchPosition)
        {
            Ray ray = MainCamera.ScreenPointToRay(touchPosition);
        }
    }
}