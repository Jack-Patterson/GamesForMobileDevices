#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchManager : MonoBehaviour
    {
        internal static TouchManager Instance = null!;
        private GestureAction _actOn = null!;
        private readonly List<TouchHandler> _multiTouchCapableTouchHandlers = new();
        private List<TouchHandler> touchHandlers = new List<TouchHandler>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _actOn = FindObjectOfType<GestureAction>();
        }

        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        CreateTouchHandler(touch);
                        break;
                }
            }

            touchHandlers.RemoveAll(handler => handler.touchId == -1 || !Input.touches.Any(t => t.fingerId == handler.touchId));

            // Reassign touchIds based on remaining touches
            foreach (Touch touch in Input.touches)
            {
                TouchHandler existingHandler = touchHandlers.FirstOrDefault(handler => handler.touchId == touch.fingerId);
                if (existingHandler == null)
                {
                    CreateTouchHandler(touch);
                }
            }

            // Handle touch end outside of the loop to avoid modifying the collection during enumeration
            List<int> endedTouches = touchHandlers
                .Where(handler => !Input.touches.Any(t => t.fingerId == handler.touchId))
                .Select(handler => handler.touchId)
                .ToList();

            foreach (int endedTouchId in endedTouches)
            {
                RemoveTouchHandler(endedTouchId);
            }
        }
        
        private void CreateTouchHandler(Touch touch)
        {
            GameObject touchHandlerParent = new GameObject($"TouchHandler {touch.fingerId}");
            touchHandlerParent.transform.SetParent(transform);
            TouchHandler newTouchHandler = touchHandlerParent.AddComponent<TouchHandler>();
            newTouchHandler.Initialize(touch.fingerId, _actOn);
            touchHandlers.Add(newTouchHandler);
        }

        public void RemoveTouchHandler(TouchHandler touchHandler)
        {
            touchHandlers.Remove(touchHandler);
            // AdjustTouchHandlers(touchHandler.touchId);
            Destroy(touchHandler.gameObject);
        }

        private void RemoveTouchHandler(int touchId)
        {
            TouchHandler handlerToRemove = touchHandlers.Find(handler => handler.touchId == touchId);
            if (handlerToRemove != null)
            {
                RemoveTouchHandler(handlerToRemove);
            }
        }

        private void AdjustTouchHandlers(int removedTouchId)
        {
            foreach (TouchHandler handler in touchHandlers)
            {
                if (handler.touchId > removedTouchId)
                {
                    handler.touchId--;
                }
            }
        }
    }
}