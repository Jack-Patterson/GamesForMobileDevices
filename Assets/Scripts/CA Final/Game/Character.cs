using GamesForMobileDevices.CA_Final.UI;
using UnityEngine;

namespace GamesForMobileDevices.CA_Final.Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        public float jumpForce = 5f;
        private bool _isGrounded = true;
        private Vector2 _startTouchPosition;
        private bool _stopTouch = false;
        public float swipeThreshold = 50f;
        public float moveSpeed = 2f;
        public float minX = -5f;
        public float maxX = 5f;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (Input.touchCount == 0 || GameManager.instance.IsGameOver) return;

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    _stopTouch = false;
                    break;

                case TouchPhase.Moved:
                    if (!_stopTouch && _isGrounded)
                    {
                        Vector2 swipeDistance = touch.position - _startTouchPosition;
                        if (swipeDistance.magnitude > swipeThreshold &&
                            Mathf.Abs(swipeDistance.y) > Mathf.Abs(swipeDistance.x) && swipeDistance.y > 0)
                        {
                            Jump();
                            _stopTouch = true;
                        }
                    }

                    Vector3 position = transform.position;
                    Vector3 touchPosition = _camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y,
                        position.z - _camera.transform.position.z));
                    position = new Vector3(touchPosition.x, position.y, position.z);
                    transform.position = position;
                    break;

                case TouchPhase.Ended:
                    _stopTouch = true;
                    break;
            }
        }

        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            _isGrounded = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            ICollidable collidable = other.gameObject.GetComponent<ICollidable>();
            
            if (collidable != null)
            {
                switch (collidable)
                {
                    case Ground:
                        _isGrounded = true;
                        _stopTouch = false;
                        break;
                    case Target:
                        if (GameManager.instance.IsGameOver) return;
                        
                        GameManager.instance.IsGameOver = true;
                        FindObjectOfType<GameUIManager>()?.ShowGameOver();
                        break;
                }
            }
        }
    }
}