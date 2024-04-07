#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace com.GamesForMobileDevices
{
    public class TouchManager : MonoBehaviour
    {
        internal static TouchManager Instance = null!;
        internal bool ShouldRotateCamera => _toggle.isOn;
        private GestureAction _actOn = null!;
        private readonly List<TouchHandler> _multiTouchCapableTouchHandlers = new();
        private readonly List<TouchHandler> touchHandlers = new List<TouchHandler>();
        private Toggle _toggle;

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
            _toggle = FindObjectOfType<Toggle>();
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
            
            CheckMultiTouch();
        }

        internal void CheckMultiTouch()
        {
            // Handle multi-touch capable touch handlers
            if (_multiTouchCapableTouchHandlers.Count >= 2)
            {
                for (int i = 0; i < _multiTouchCapableTouchHandlers.Count; i += 2)
                {
                    if (i + 1 >= _multiTouchCapableTouchHandlers.Count) continue;
                    TouchHandler touchHandler1 = _multiTouchCapableTouchHandlers[i];
                    TouchHandler touchHandler2 = _multiTouchCapableTouchHandlers[i + 1];
                    
                    if (touchHandler1.HasMultiTouchPartner || touchHandler2.HasMultiTouchPartner) continue;
                    touchHandler1.SetMultiTouchPartner(touchHandler2);
                    touchHandler2.SetMultiTouchPartner(touchHandler1);

                    touchHandler1.isMultiTouchController = true;
                    touchHandler2.isMultiTouchController = false;
                    touchHandler1.GetInitialMultiTouchData();
                }
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
        
        internal void RegisterMultiTouchCapableTouchHandler(TouchHandler touchHandler)
        {
            _multiTouchCapableTouchHandlers.Add(touchHandler);
        }
        
        internal void UnregisterMultiTouchCapableTouchHandler(TouchHandler touchHandler)
        {
            _multiTouchCapableTouchHandlers.Remove(touchHandler);
        }
    }
}