using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchManager : MonoBehaviour
    {
        private const float MaxTimeForSwipe = 0.5f;
        private float _touchTimer;
        private bool _hasMoved;
        GestureAction _actOn;

        private void Start()
        {
            _actOn = FindObjectOfType<GestureAction>();
        }

        private void Update()
        {
            CheckTouch();
        }

        private void CheckTouch()
        {
            Touch[] currentTouches = Input.touches;

            if (currentTouches.Length >= 1)
            {
                Touch touch = currentTouches[0];
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touchTimer = 0f;
                        _hasMoved = false;
                        break;
                    case TouchPhase.Stationary:
                        _touchTimer += Time.deltaTime;
                        break;
                    case TouchPhase.Moved:
                        _hasMoved = true;
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

                        break;
                }
            }
        }
    }
}