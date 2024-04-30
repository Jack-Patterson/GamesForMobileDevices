using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GamesForMobileDevices.CA_Gestures
{
    public class TouchManager : MonoBehaviour
    {
        internal static TouchManager instance = null!;
        internal bool ShouldRotateCamera => _shouldRotateCameraToggle.isOn;
        internal bool ShouldUseAccelerometer => _shouldUseAccelerometerToggle.isOn;
        private GestureAction _actOn = null!;
        private readonly List<TouchHandler> _multiTouchCapableTouchHandlers = new();
        private readonly List<TouchHandler> _touchHandlers = new();
        private Toggle _shouldRotateCameraToggle;
        private Toggle _shouldUseAccelerometerToggle;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _actOn = FindObjectOfType<GestureAction>();
            Toggle[] toggles = FindObjectsOfType<Toggle>();
            _shouldRotateCameraToggle = toggles.First(toggle => toggle.name == "ShouldRotateCameraToggle");
            _shouldUseAccelerometerToggle = toggles.First(toggle => toggle.name == "ShouldUseAccelerometerToggle");
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

            _touchHandlers.RemoveAll(handler =>
                handler.TouchId == -1 || !Input.touches.Any(t => t.fingerId == handler.TouchId));

            // Reassign touchIds based on remaining touches
            foreach (Touch touch in Input.touches)
            {
                TouchHandler existingHandler =
                    _touchHandlers.FirstOrDefault(handler => handler.TouchId == touch.fingerId)!;
                if (existingHandler == null)
                {
                    CreateTouchHandler(touch);
                }
            }

            // Handle touch end outside of the loop to avoid modifying the collection during enumeration
            List<int> endedTouches = _touchHandlers
                .Where(handler => !Input.touches.Any(t => t.fingerId == handler.TouchId))
                .Select(handler => handler.TouchId)
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

                    touchHandler1.IsMultiTouchController = true;
                    touchHandler2.IsMultiTouchController = false;
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
            _touchHandlers.Add(newTouchHandler);
        }

        public void RemoveTouchHandler(TouchHandler touchHandler)
        {
            _touchHandlers.Remove(touchHandler);
            // AdjustTouchHandlers(touchHandler.touchId);
            Destroy(touchHandler.gameObject);
        }

        private void RemoveTouchHandler(int touchId)
        {
            TouchHandler handlerToRemove = _touchHandlers.Find(handler => handler.TouchId == touchId);
            if (handlerToRemove != null)
            {
                RemoveTouchHandler(handlerToRemove);
            }
        }

        private void AdjustTouchHandlers(int removedTouchId)
        {
            foreach (TouchHandler handler in _touchHandlers)
            {
                if (handler.TouchId > removedTouchId)
                {
                    handler.TouchId--;
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