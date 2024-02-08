#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchManager : MonoBehaviour
    {
        internal static TouchManager instance = null!;
        private GestureAction _actOn = null!;
        private readonly List<TouchHandler> _touchHandlers = new();
        private readonly List<TouchHandler> _multiTouchCapableTouchHandlers = new();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _actOn = FindObjectOfType<GestureAction>();
            
            print(Input.touchCount);
            print(Input.touches);
        }

        private void Update()
        {
            AssignTouches();
        }

        internal void RemoveTouchHandler(TouchHandler touchHandler)
        {
            int handlerIndex = _touchHandlers.IndexOf(touchHandler);
            _touchHandlers.RemoveAt(handlerIndex);
            Destroy(touchHandler.gameObject);

            foreach (TouchHandler otherTouchHandler in _touchHandlers)
            {
                otherTouchHandler.previousTouchId = otherTouchHandler.touchId;
                otherTouchHandler.touchId -= _touchHandlers.IndexOf(otherTouchHandler);
                otherTouchHandler.gameObject.name = "TouchHandler_" + otherTouchHandler.touchId;
            }
        }

        internal void RegisterMultiTouchCapableTouchHandler(TouchHandler touchHandler)
        {
            _multiTouchCapableTouchHandlers.Add(touchHandler);
        }
        
        internal void DeregisterMultiTouchCapableTouchHandler(TouchHandler touchHandler)
        {
            _multiTouchCapableTouchHandlers.Remove(touchHandler);
        }
        
        private void CreateTouchHandler(int touchIndex)
        {
            GameObject touchHandlerObject = new GameObject("TouchHandler_" + touchIndex);
            touchHandlerObject.transform.parent = transform;
            TouchHandler touchHandler = touchHandlerObject.AddComponent<TouchHandler>();
            touchHandler.Initialize(touchIndex, _actOn);
            _touchHandlers.Add(touchHandler);
        }

        private void AssignTouches()
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (!_touchHandlers.Exists(touchHandler => touchHandler.touchId == i))
                {
                    CreateTouchHandler(i);
                }
            }
            
            if (_multiTouchCapableTouchHandlers.Count < 2) return;
            
            TouchHandler touchHandler1 = _multiTouchCapableTouchHandlers[0];
            TouchHandler touchHandler2 = _multiTouchCapableTouchHandlers[1];
            
            if (touchHandler1.touchId == -1 || touchHandler2.touchId == -1) return;
            if (touchHandler1.HasMultiTouchPartner || touchHandler2.HasMultiTouchPartner) return;
            
            touchHandler1.SetMultiTouchPartner(touchHandler2);
            touchHandler2.SetMultiTouchPartner(touchHandler1);
        }
    }
}