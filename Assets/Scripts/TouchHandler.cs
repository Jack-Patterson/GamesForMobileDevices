using System;
using System.Collections.Generic;
using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchHandler: MonoBehaviour
    {
        private const float MaxTimeForSwipe = 0.5f;
        private float _touchTimer;
        private bool _hasMoved;
        GestureAction _actOn;
        private List<TouchHandler> _touchHandlers = new List<TouchHandler>();
        private GameObject _selectedObject;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            _actOn = FindObjectOfType<GestureAction>();
        }
        
        private void CheckTouch()
        {
            Touch[] currentTouches = Input.touches;

            if (currentTouches.Length >= 1)
            {
                Touch touch = currentTouches[0];
                Vector3 touchPosition = touch.position;
                float distance = Vector3.Distance(touchPosition, _mainCamera.transform.position);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touchTimer = 0f;
                        _hasMoved = false;

                        Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            if (hit.collider.GetComponent<IInteractable>() != null)
                            {
                                _selectedObject = hit.collider.gameObject;
                                _selectedObject.GetComponent<IInteractable>().EnableOutline();
                                print("Distance: " + Vector3.Distance(_mainCamera.transform.position,
                                    _selectedObject.transform.position));
                            }
                        }

                        break;
                    case TouchPhase.Stationary:
                        _touchTimer += Time.deltaTime;
                        break;
                    case TouchPhase.Moved:
                        _hasMoved = true;

                        if (_selectedObject != null)
                        {
                            Vector3 touchPos = new Vector3(touch.position.x, touch.position.y, 0);
                            Ray rayC = _mainCamera.ScreenPointToRay(touchPos);
                            
                            float orbitDistance = Vector3.Distance(_mainCamera.transform.position, _selectedObject.transform.position);
                            Vector3 newPosition = _mainCamera.transform.position + rayC.direction * orbitDistance;
                            _selectedObject.transform.position = newPosition;
                            
                        }
                        else
                        {
                            float rotateX = -touch.deltaPosition.y * 5f * Time.deltaTime;
                            float rotateY = touch.deltaPosition.x * 5f * Time.deltaTime;
                            
                            Quaternion currentRotation = transform.rotation;
                            Quaternion newRotation = Quaternion.Euler(
                                currentRotation.eulerAngles.x + rotateX,
                                currentRotation.eulerAngles.y + rotateY,
                                0
                            );
                            transform.rotation = newRotation;
                        }

                        break;
                    case TouchPhase.Ended:
                        _selectedObject?.GetComponent<IInteractable>()?.DisableOutline();
                        _actOn.TapAt(touch.position);

                        if (_touchTimer <= MaxTimeForSwipe && !_hasMoved)
                        {
                            print($"Tap at {touch.position}");
                        }
                        else if (_hasMoved)
                        {
                            print($"Swipe at {touch.position}");
                        }
                        else
                        {
                            print($"Hold at {touch.position}");
                        }

                        _selectedObject = null;

                        break;
                }
            }
        }
    }
}