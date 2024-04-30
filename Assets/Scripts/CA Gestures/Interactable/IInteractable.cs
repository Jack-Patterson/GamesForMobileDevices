using UnityEngine;

namespace GamesForMobileDevices.CA_Gestures.Interactable
{
    public interface IInteractable
    {
        public Vector3 Position { get; set; }
        public DragType DragType { get; set; }
        
        void ProcessTap();
        void ProcessDrag(Vector3 newPosition);
        void ProcessScale(float scale);
        void ProcessRotate(float angle);
        void EnableOutline();
        void DisableOutline();
    }
}