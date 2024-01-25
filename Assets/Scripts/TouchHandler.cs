using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchHandler : MonoBehaviour
    {
        private const float MaxTimeForTap = 0.5f;
        private const float CameraRotateSpeed = 5f;
        private Camera MainCamera => Camera.main;
        private float _touchTimer;
        private bool _hasMoved;
        GestureAction _actOn;
        private IInteractable _interactable;
        private float _distance;

        private void Start()
        {
            _actOn = FindObjectOfType<GestureAction>();
        }

        internal void CheckTouch(Touch touch)
        {
            Vector2 touchPosition = touch.position;
            Vector2 touchDeltaPosition = touch.deltaPosition;
            Vector3 cameraPosition = MainCamera.transform.position;
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Ray touchPositionToRay = MainCamera.ScreenPointToRay(touchPosition);
                    if (Physics.Raycast(touchPositionToRay, out RaycastHit hit))
                    {
                        if (hit.collider.TryGetComponent(out _interactable))
                        {
                            _interactable.EnableOutline();
                            _distance = Vector3.Distance(cameraPosition, _interactable.Position);
                            print("Distance: " + _distance);
                        }
                    }

                    break;
                case TouchPhase.Stationary:
                    _touchTimer += Time.deltaTime;
                    break;
                case TouchPhase.Moved:
                    _hasMoved = true;

                    if (_interactable != null)
                    {
                        _actOn.DragAt(_interactable, touchPosition, _distance);
                    }
                    else
                    {
                        float rotateX = -touchDeltaPosition.y * CameraRotateSpeed * Time.deltaTime;
                        float rotateY = touchDeltaPosition.x * CameraRotateSpeed * Time.deltaTime;

                        Quaternion currentRotation = transform.rotation;
                        Quaternion newRotation = Quaternion.Euler(
                            currentRotation.eulerAngles.x + rotateX,
                            currentRotation.eulerAngles.y + rotateY,
                            0f
                        );
                        MainCamera.transform.rotation = newRotation;
                    }

                    break;
                case TouchPhase.Ended:
                    _interactable?.DisableOutline();

                    if (_touchTimer <= MaxTimeForTap && !_hasMoved)
                    {
                        print($"Tap at {touchPosition}");
                        _actOn.TapAt(touchPosition);
                    }
                    else if (_hasMoved)
                    {
                        print($"Swipe at {touchPosition}");
                    }
                    else
                    {
                        print($"Hold at {touchPosition}");
                    }

                    break;
            }
        }

        internal void Reset()
        {
            _touchTimer = 0;
            _hasMoved = false;
            _interactable = null;
        }
    }
}