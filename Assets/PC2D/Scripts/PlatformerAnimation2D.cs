using UnityEngine;

namespace PC2D
{
    /// <summary>
    /// This is a very very very simple example of how an animation system could query information from the motor to set state.
    /// This can be done to explicitly play states, as is below, or send triggers, float, or bools to the animator. Most likely this
    /// will need to be written to suit your game's needs.
    /// </summary>

    public class PlatformerAnimation2D : MonoBehaviour
    {
        public float jumpRotationSpeed;
        public GameObject visualChild;

        private PC2DMotor _motor;
        private Animator _animator;
        private bool _isJumping;
        private bool _currentFacingLeft;

        // Use this for initialization
        void Start()
        {
            _motor = GetComponent<PC2DMotor>();
            _animator = visualChild.GetComponent<Animator>();
            _animator.Play("Idle");

            _motor.onJump += SetCurrentFacingLeft;
        }

        // Update is called once per frame
        void Update()
        {
            if (_motor.motorState == PC2DMotor.MotorState.Jumping ||
                _isJumping &&
                    (_motor.motorState == PC2DMotor.MotorState.Falling ||
                                 _motor.motorState == PC2DMotor.MotorState.FallingFast))
            {
                _isJumping = true;
                _animator.Play("Jump");

                if (_motor.velocity.x <= -0.1f)
                {
                    _currentFacingLeft = true;
                }
                else if (_motor.velocity.x >= 0.1f)
                {
                    _currentFacingLeft = false;
                }

                Vector3 rotateDir = _currentFacingLeft ? Vector3.forward : Vector3.back;
                visualChild.transform.Rotate(rotateDir, jumpRotationSpeed * Time.deltaTime);
            }
            else
            {
                _isJumping = false;
                visualChild.transform.rotation = Quaternion.identity;

                if (_motor.motorState == PC2DMotor.MotorState.Falling ||
                                 _motor.motorState == PC2DMotor.MotorState.FallingFast)
                {
                    _animator.Play("Fall");
                }
                else if (_motor.motorState == PC2DMotor.MotorState.WallSliding ||
                         _motor.motorState == PC2DMotor.MotorState.WallSticking)
                {
                    _animator.Play("Cling");
                }
                else if (_motor.motorState == PC2DMotor.MotorState.OnCorner)
                {
                    _animator.Play("On Corner");
                }
                else if (_motor.motorState == PC2DMotor.MotorState.Slipping)
                {
                    _animator.Play("Slip");
                }
                else if (_motor.motorState == PC2DMotor.MotorState.Dashing)
                {
                    _animator.Play("Dash");
                }
                else
                {
                    if (_motor.velocity.sqrMagnitude >= 0.1f * 0.1f)
                    {
                        _animator.Play("Walk");
                    }
                    else
                    {
                        _animator.Play("Idle");
                    }
                }
            }

            // Facing
            float valueCheck = _motor.normalizedXMovement;

            if (_motor.motorState == PC2DMotor.MotorState.Slipping ||
                _motor.motorState == PC2DMotor.MotorState.Dashing ||
                _motor.motorState == PC2DMotor.MotorState.Jumping)
            {
                valueCheck = _motor.velocity.x;
            }
            
            if (Mathf.Abs(valueCheck) >= 0.1f)
            {
                Vector3 newScale = visualChild.transform.localScale;
                newScale.x = Mathf.Abs(newScale.x) * ((valueCheck > 0) ? 1.0f : -1.0f);
                visualChild.transform.localScale = newScale;
            }
        }

        private void SetCurrentFacingLeft()
        {
            _currentFacingLeft = _motor.facingLeft;
        }
    }
}
