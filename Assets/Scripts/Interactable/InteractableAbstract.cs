using UnityEngine;

namespace com.GamesForMobileDevices.Interactable
{
    public abstract class InteractableAbstract : MonoBehaviour, IInteractable
    {
        private Renderer _renderer;
        private Material _originalMaterial;

        [SerializeField] private DragType dragType = DragType.Orbit;

        public DragType DragType
        {
            get => dragType;
            set => dragType = value;
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _originalMaterial = _renderer.material;
        }

        public abstract void ProcessTap();

        public virtual void ProcessDrag(Vector3 newPosition)
        {
            Position = newPosition;
        }
        
        public virtual void ProcessScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }

        public void EnableOutline()
        {
            Material outlineMaterial = new Material(Shader.Find("Standard"));
            Color outlineColor = Color.blue;
            outlineColor.a = .5f;

            outlineMaterial.color = outlineColor;
            outlineMaterial.SetFloat("_Outline", .5f);
            _renderer.material = outlineMaterial;
        }

        public void DisableOutline()
        {
            _renderer.material = _originalMaterial;
        }
    }
}