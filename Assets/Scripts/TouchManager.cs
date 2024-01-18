using System.Collections.Generic;
using com.GamesForMobileDevices.Interactable;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchManager : MonoBehaviour
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

        private void Update()
        {
            CheckTouch();
            AssignTouchHandler();
            
            // print(selectedObject);
        }

        private void AssignTouchHandler()
        {
            // Touch[] currentTouches = Input.touches;
            //
            // if (currentTouches.Length >= 1)
            // {
            //     if (_touchHandlers)
            // }
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
                            Vector3 selectedObjectPosition = _selectedObject.transform.position;

                            Vector3 pos = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distance));
                            _selectedObject.transform.position = new Vector3(pos.x, pos.y, selectedObjectPosition.z);
                        }
                        break;
                    case TouchPhase.Ended:
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