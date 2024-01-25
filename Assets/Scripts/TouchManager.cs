#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchManager : MonoBehaviour
    {
        private static readonly Touch NullTouch = new Touch { phase = TouchPhase.Canceled };
        [SerializeField] private int maxTouches = 5;
        private List<TouchHandler> _touchHandlers = new();
        private List<Touch> _activeTouches = new();

        private void Start()
        {
            InitializeTouchHandlers();
        }

        private void Update()
        {
            AssignTouches();
            CheckTouches();
        }

        private void InitializeTouchHandlers()
        {
            _touchHandlers = new List<TouchHandler>(maxTouches);
            _activeTouches = new List<Touch>(maxTouches);

            for (int i = 0; i < maxTouches; i++)
            {
                GameObject touchHandlerObject = new GameObject("TouchHandler_" + i);
                touchHandlerObject.transform.parent = transform;
                TouchHandler touchHandler = touchHandlerObject.AddComponent<TouchHandler>();
                _touchHandlers.Add(touchHandler);
                
                _activeTouches.Add(NullTouch);
            }
        }

        private void AssignTouches()
        {
            for (int i = 0; i < maxTouches; i++)
            {
                if (i < Input.touchCount)
                {
                    _activeTouches[i] = Input.GetTouch(i);
                }
                else
                {
                    _activeTouches[i] = NullTouch;
                }

            }
        }

        private void CheckTouches()
        {
            for (int i = 0; i < maxTouches; i++)
            {
                _touchHandlers[i].CheckTouch(_activeTouches[i]);

                if (_activeTouches[i].phase != TouchPhase.Ended) continue;
                _activeTouches[i] = default;

                TouchHandler touchHandler = _touchHandlers[i];
                _touchHandlers.Remove(touchHandler);
                _touchHandlers.Add(touchHandler);
                touchHandler.Reset();
            }
        }
    }
}