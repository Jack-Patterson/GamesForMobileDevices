using System;
using System.Collections.Generic;
using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchHandler : MonoBehaviour
    {
        private const float MaxTimeForTap = 0.5f;
        private Camera MainCamera => Camera.main;
        private float _touchTimer = 0;
        private bool _hasMoved = false;
        GestureAction _actOn;
        private GameObject _selectedObject;

        private void Start()
        {
            _actOn = FindObjectOfType<GestureAction>();
        }

        internal void CheckTouch(Touch touch)
        {
            Vector3 touchPosition = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    Ray ray = MainCamera.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider.GetComponent<IInteractable>() != null)
                        {
                            _selectedObject = hit.collider.gameObject;
                            _selectedObject.GetComponent<IInteractable>().EnableOutline();
                            print("Distance: " + Vector3.Distance(MainCamera.transform.position,
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
                        Ray rayC = MainCamera.ScreenPointToRay(touchPos);

                        float orbitDistance = Vector3.Distance(MainCamera.transform.position,
                            _selectedObject.transform.position);
                        Vector3 newPosition = MainCamera.transform.position + rayC.direction * orbitDistance;
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
                        MainCamera.transform.rotation = newRotation;
                    }

                    break;
                case TouchPhase.Ended:
                    _selectedObject?.GetComponent<IInteractable>()?.DisableOutline();
                    _actOn.TapAt(touch.position);

                    if (_touchTimer <= MaxTimeForTap && !_hasMoved)
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

        internal void Reset()
        {
            _touchTimer = 0;
            _hasMoved = false;
            _selectedObject = null;
        }
    }
}