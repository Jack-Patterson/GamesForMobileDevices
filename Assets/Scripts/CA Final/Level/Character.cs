using UnityEngine;

namespace com.GamesForMobileDevices.CA_Final.Level
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
            if (Input.touchCount == 0) return;

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

        private void MoveHorizontal(float swipeDistanceX)
        {
            // Determine direction based on swipe distance (positive for right, negative for left)
            float direction = swipeDistanceX > 0 ? 1 : -1;
            // Move the character
            Vector3 moveVector = new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
            _rigidbody.MovePosition(_rigidbody.position + moveVector);
        }

        private void OnCollisionEnter(Collision other)
        {
            // Check if the character has collided with the ground
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true; // The character is back on the ground
                _stopTouch = false;
            }
        }
    }
}