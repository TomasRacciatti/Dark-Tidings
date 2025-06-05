using UnityEngine;

namespace Characters.Player
{
    public class PlayerView : MonoBehaviour
    {
        //[SerializeField] private AudioClip LandingAudioClip;
        //[SerializeField] private AudioClip[] FootstepAudioClips;
        //[Range(0, 1)] [SerializeField] private float FootstepAudioVolume = 0.5f;
    
        private Animator _animator;

        // animation IDs
        private int _animIDSpeedForward;
        private int _animIDSpeedRight;
        private int _animIDSpeedUp;
        private int _animIDJumped;
        private int _animIDLanded;
        private int _animIDGrounded;
        private int _animIDFalling;
    
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            AssignAnimationIDs();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeedForward = Animator.StringToHash("SpeedForward");
            _animIDSpeedRight = Animator.StringToHash("SpeedRight");
            _animIDSpeedUp = Animator.StringToHash("SpeedUp");
            _animIDJumped = Animator.StringToHash("Jumped");
            _animIDLanded = Animator.StringToHash("Landed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDFalling = Animator.StringToHash("Falling");
        }

        public void SetSpeed(float speed, float forward, float right)
        {
            _animator.SetFloat(_animIDSpeedForward, forward);
            _animator.SetFloat(_animIDSpeedRight, right);
        }

        public void SetVerticalSpeed(float up)
        {
            _animator.SetFloat(_animIDSpeedUp, up);
        }

        public void Jumped()
        {
            SetGrounded(false);
            _animator.SetTrigger(_animIDJumped);
            //sounds
        }

        public void Landed()
        {
            SetGrounded(true);
            SetFalling(false);
            _animator.SetTrigger(_animIDLanded);
            //sounds
        }

        public void SetGrounded(bool value)
        {
            _animator.SetBool(_animIDGrounded, value);
        }
    
        public void SetFalling(bool value)
        {
            _animator.SetBool(_animIDFalling, value);
        }
    }
}