using UnityEngine;

namespace com.GamesForMobileDevices.CA_Final
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        public float jumpForce = 5f; // Adjust the jump force as needed
        private bool _isGrounded = true; // Assume the character starts on the ground
        private Vector2 _startTouchPosition;
        private bool _stopTouch = false;
        public float swipeThreshold = 50f;
        public float moveSpeed = 2f; // Speed of horizontal movement
        public float minX = -5f; // Minimum x position
        public float maxX = 5f;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startTouchPosition = touch.position;
                        _stopTouch = false;
                        break;

                    case TouchPhase.Moved:
                        if (!_stopTouch)
                        {
                            Vector2 swipeDistance = touch.position - _startTouchPosition;
                            // Horizontal movement
                            if (Mathf.Abs(swipeDistance.x) > Mathf.Abs(swipeDistance.y))
                            {
                                MoveHorizontal(swipeDistance.x);
                            }
                            // Vertical swipe for jump
                            else if (swipeDistance.magnitude > swipeThreshold && swipeDistance.y > 0 && _isGrounded)
                            {
                                Jump();
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                        _stopTouch = true;
                        break;
                }
            }

            // Clamp position
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            transform.position = clampedPosition;
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