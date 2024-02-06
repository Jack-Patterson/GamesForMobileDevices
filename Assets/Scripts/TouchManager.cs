#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace com.GamesForMobileDevices
{
    public class TouchManager : MonoBehaviour
    {
        internal static TouchManager instance = null!;
        private readonly List<TouchHandler> _multiTouchCapableTouchHandlers = new();
        [SerializeField] private int maxTouches = 5;
        private List<TouchHandler> _touchHandlers = new();
        private GestureAction _actOn = null!;

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
            _touchHandlers.Remove(touchHandler);
            Destroy(touchHandler.gameObject);
        }

        internal void RegisterMultiTouchCapableTouchHandler(TouchHandler touchHandler)
        {
            _multiTouchCapableTouchHandlers.Add(touchHandler);
        }
        
        internal void DeregisterMultiTouchCapableTouchHandler(TouchHandler touchHandler)
        {
            _multiTouchCapableTouchHandlers.Remove(touchHandler);
        }
        
        private TouchHandler CreateTouchHandler(int touchIndex)
        {
            GameObject touchHandlerObject = new GameObject("TouchHandler_" + touchIndex);
            touchHandlerObject.transform.parent = transform;
            TouchHandler touchHandler = touchHandlerObject.AddComponent<TouchHandler>();
            touchHandler.Initialize(touchIndex, _actOn);
            _touchHandlers.Add(touchHandler);

            return touchHandler;
        }

        private void AssignTouches()
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (_touchHandlers.Count < Input.touches.Length)
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