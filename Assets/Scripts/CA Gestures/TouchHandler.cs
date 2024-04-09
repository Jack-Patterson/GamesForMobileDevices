using System.Linq;
using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchHandler : MonoBehaviour
    {
        private static Camera MainCamera => Camera.main;
        public int TouchId { get; set; } = -1;
        private const float MaxTimeForTap = 0.5f;
        private const float CameraRotateSpeed = 5f;
        private float _touchTimer;
        private bool _hasMoved;
        private GestureAction _actOn;
        private IInteractable _interactable;
        private float _distance;


        public TouchHandler MultiTouchPartner {get => _multiTouchPartner; set => _multiTouchPartner = value;}
        private TouchHandler _multiTouchPartner;
        public bool HasMultiTouchPartner => _multiTouchPartner != null;
        public bool IsMultiTouchController { get; set; }
        private float _initialMultiTouchDistance = 0f;
        private float _initialMultiTouchAngle = 0f;

        private void Update()
        {
            if (TouchId == -1 || Input.touches.All(touch => touch.fingerId != TouchId)) return;

            CheckTouch();
        }

        public void Initialize(int touchIndex, GestureAction actOn)
        {
            TouchId = touchIndex;
            _actOn = actOn;

            CheckTouch();
        }

        public void Reset()
        {
            TouchId = -1;
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
            IsMultiTouchController = false;
        }

        public void CheckTouch()
        {
            Touch touch = Input.touches.First(t => t.fingerId == TouchId);

            Vector2 touchPosition = touch.position;
            Vector2 touchDeltaPosition = touch.deltaPosition;
            Vector3 cameraPosition = MainCamera.transform.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TouchManager.instance.RegisterMultiTouchCapableTouchHandler(this);
                    TouchManager.instance.CheckMultiTouch();

                    if (HasMultiTouchPartner)
                    {
                        GetInitialMultiTouchData();
                    }

                    if (!HasMultiTouchPartner || _multiTouchPartner?._interactable == null)
                    {
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
                    }

                    break;
                case TouchPhase.Stationary:
                    _touchTimer += Time.deltaTime;
                    break;
                case TouchPhase.Moved:
                    _hasMoved = true;

                    if (HasMultiTouchPartner)
                    {
                        if (!IsMultiTouchController) break;

                        Vector2 otherTouchPosition = _multiTouchPartner.GetTouchPosition();
                        IInteractable interactable = _interactable ?? _multiTouchPartner._interactable;

                        if (interactable != null)
                        {
                            float newDistance = Vector2.Distance(touchPosition, otherTouchPosition);
                            _actOn.ScaleAt(interactable, newDistance / _initialMultiTouchDistance);
                            _initialMultiTouchDistance = newDistance;

                            float cameraPositionZ = MainCamera.transform.position.z;
                            Vector3 fingerPositionWorld =
                                MainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y,
                                    cameraPositionZ));
                            Vector3 otherFingerPositionWorld =
                                MainCamera.ScreenToWorldPoint(new Vector3(otherTouchPosition.x, otherTouchPosition.y,
                                    cameraPositionZ));
                            float currentAngle = Mathf.Atan2(otherFingerPositionWorld.y - fingerPositionWorld.y,
                                otherFingerPositionWorld.x - fingerPositionWorld.x) * Mathf.Rad2Deg;
                            float angleDifference = currentAngle - _initialMultiTouchAngle;
                            _actOn.RotateAt(interactable, angleDifference);
                            _initialMultiTouchAngle = currentAngle;
                        }
                        else
                        {
                            if (TouchManager.instance.ShouldRotateCamera)
                            {
                                float currentAngle = Mathf.Atan2(otherTouchPosition.y - touchPosition.y,
                                    otherTouchPosition.x - touchPosition.x) * Mathf.Rad2Deg;
                                float angleDifference = currentAngle - _initialMultiTouchAngle;
                                MainCamera.transform.rotation *= Quaternion.AngleAxis(angleDifference, Vector3.forward);
                                _initialMultiTouchAngle = currentAngle;
                                print(currentAngle);
                            }
                            
                                float newDistance = Vector2.Distance(touchPosition, otherTouchPosition);
                                MainCamera.fieldOfView += (newDistance - _initialMultiTouchDistance) * 0.1f * -1f;
                                _initialMultiTouchDistance = newDistance;
                            
                        }
                    }
                    else
                    {
                        if (_interactable != null)
                        {
                            _actOn.DragAt(_interactable, touchPosition, _distance);
                        }
                        else
                        {
                            float rotateX = -touchDeltaPosition.y * CameraRotateSpeed * Time.deltaTime;
                            float rotateY = touchDeltaPosition.x * CameraRotateSpeed * Time.deltaTime;

                            Quaternion currentRotation = MainCamera.transform.rotation;
                            Quaternion newRotation = Quaternion.Euler(
                                currentRotation.eulerAngles.x + rotateX,
                                currentRotation.eulerAngles.y + rotateY,
                                currentRotation.eulerAngles.z
                            );
                            MainCamera.transform.rotation = newRotation;
                        }
                    }

                    break;
                case TouchPhase.Ended:
                    // print("ended " + touchId);

                    _interactable?.DisableOutline();
                    TouchManager.instance.UnregisterMultiTouchCapableTouchHandler(this);
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

                    TouchHandler touchHandler = this;

                    TouchManager.instance.RemoveTouchHandler(touchHandler);

                    break;
            }
        }

        private Vector2 GetTouchPosition()
        {
            return Input.touches.First(t => t.fingerId == TouchId).position;
        }

        public void GetInitialMultiTouchData()
        {
            Vector2 touchPosition = GetTouchPosition();
            Vector2 otherTouchPosition = _multiTouchPartner.GetTouchPosition();

            _initialMultiTouchDistance = Vector2.Distance(touchPosition, otherTouchPosition);

            float cameraPositionZ = MainCamera.transform.position.z;
            Vector2 touchPos, otherTouchPos;
            IInteractable interactable = _interactable ?? _multiTouchPartner._interactable;
            if (interactable != null)
            {
                (touchPos, otherTouchPos) = (
                    MainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, cameraPositionZ)),
                    MainCamera.ScreenToWorldPoint(new Vector3(otherTouchPosition.x, otherTouchPosition.y,
                        cameraPositionZ)));
            }
            else
            {
                (touchPos, otherTouchPos) = (touchPosition, otherTouchPosition);
            }

            _initialMultiTouchAngle =
                Mathf.Atan2(otherTouchPos.y - touchPos.y, otherTouchPos.x - touchPos.x) * Mathf.Rad2Deg;
        }
    }
}