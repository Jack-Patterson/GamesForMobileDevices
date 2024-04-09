using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public partial class GestureAction : MonoBehaviour
    {
        private Camera MainCamera => Camera.main;
        [SerializeField] private LayerMask planeLayerMask;
        [SerializeField] private LayerMask surfaceLayerMask;

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

            switch (interactable.DragType)
            {
                case DragType.Surface:
                    if (Physics.Raycast(ray, out RaycastHit surfaceHit, Mathf.Infinity, surfaceLayerMask))
                    {
                        Vector3 surfacePoint = surfaceHit.point;
                        surfacePoint.y += 1f;
                        interactable?.ProcessDrag(surfaceHit.point);
                    }
                    break;
                case DragType.Plane:
                    if (Physics.Raycast(ray, out RaycastHit planeHit, Mathf.Infinity, planeLayerMask))
                    {
                        interactable?.ProcessDrag(planeHit.point);
                    }
                    break;
                case DragType.Orbit:
                default:
                    interactable?.ProcessDrag(ray.GetPoint(distance));
                    break;
            }
        }

        public void ScaleAt(IInteractable interactable, float distance)
        {
            interactable.ProcessScale(distance);
        }
        
        public void RotateAt(IInteractable interactable, float angle)
        {
            interactable.ProcessRotate(angle);
        }
    }
}