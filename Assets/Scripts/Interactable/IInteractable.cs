using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public interface IInteractable
    {
        public Vector3 Position { get; set; }
        public DragType DragType { get; set; }
        
        void ProcessTap();
        void ProcessDrag(Vector3 newPosition);
        void EnableOutline();
        void DisableOutline();
    }
}