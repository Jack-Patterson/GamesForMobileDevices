using System;
using System.Linq;
using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchHandler : MonoBehaviour
    {
        private static Camera MainCamera => Camera.main;
        internal int touchId = -1;
        internal bool HasMultiTouchPartner => _multiTouchPartner != null;
        private const float MaxTimeForTap = 0.5f;
        private const float CameraRotateSpeed = 5f;
        private float _touchTimer;
        private bool _hasMoved;
        private GestureAction _actOn;
        private IInteractable _interactable;
        private float _distance;
        private TouchHandler _multiTouchPartner;
        internal int previousTouchId;

        private void Update()
        {
            if (touchId != previousTouchId)
            {
                print("not same");
            }

            try
            {
                print(Input.GetTouch(touchId).phase);
                print(Input.GetTouch(touchId).fingerId);
            }
            catch (Exception)
            {
                // ignored
            }

            if (touchId == -1 || !Input.touches.Any(touch => touch.fingerId == touchId)) return;

            CheckTouch();
        }

        internal void Initialize(int touchIndex, GestureAction actOn)
        {
            touchId = touchIndex;
            previousTouchId = touchIndex;
            _actOn = actOn;
            
            CheckTouch();
        }

        internal void Reset()
        {
            touchId = -1;
            _touchTimer = 0;
            _hasMoved = false;
            _interactable = null;
        }

        internal void SetMultiTouchPartner(TouchHandler touchHandler)
        {
            _multiTouchPartner = touchHandler;
        }

        internal void RemoveMultiTouchPartner()
        {
            _multiTouchPartner = null;
        }

        private void CheckTouch()
        {
            print(touchId);

            if (touchId != previousTouchId)
            {
                print("not same");
            }

            Touch touch = Input.GetTouch(touchId);
            
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

                    if (_interactable == null)
                    {
                        TouchManager.instance.RegisterMultiTouchCapableTouchHandler(this);
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
                        if (HasMultiTouchPartner)
                        {
                            // print("MultiTouchPartner");
                        }
                        else
                        {
                            float rotateX = -touchDeltaPosition.y * CameraRotateSpeed * Time.deltaTime;
                            float rotateY = touchDeltaPosition.x * CameraRotateSpeed * Time.deltaTime;
                            
                            Quaternion currentRotation = MainCamera.transform.rotation;
                            Quaternion newRotation = Quaternion.Euler(
                                currentRotation.eulerAngles.x + rotateX,
                                currentRotation.eulerAngles.y + rotateY,
                                0f
                            );
                            MainCamera.transform.rotation = newRotation;
                        }
                    }

                    break;
                case TouchPhase.Ended:
                    print("ended");
                    
                    _interactable?.DisableOutline();
                    TouchManager.instance.DeregisterMultiTouchCapableTouchHandler(this);
                    _multiTouchPartner?.RemoveMultiTouchPartner();
                    RemoveMultiTouchPartner();

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
                    
                    TouchManager.instance.RemoveTouchHandler(this);

                    break;
            }
        }
    }
}