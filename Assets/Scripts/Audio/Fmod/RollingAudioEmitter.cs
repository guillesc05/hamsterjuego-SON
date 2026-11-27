using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class RollingAudioEmitter : MonoBehaviour
{
    [SerializeField]  private EventReference _movementEvent;
    [SerializeField] private string _parameterName = "Surface";
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private MovementComponent _movementComponent;

    private EventInstance _eventInstance;
    private SurfaceType _currentSurface = SurfaceType.Invalid;
    private bool _isPlaying;

    void Start()
    {
        // Initialize the movement event as an instance
        _eventInstance = RuntimeManager.CreateInstance(_movementEvent);
        RuntimeManager.AttachInstanceToGameObject(_eventInstance, gameObject);
    }

    void Update()
    {
        bool grounded = _groundDetector.IsGrounded();

        // Update the surface while grounded
        if (grounded)
        {
            SurfaceType surface = _groundDetector.GetGroundSurface();
            if (surface != SurfaceType.Invalid && surface != _currentSurface)
            {
                _currentSurface = surface;
                _eventInstance.setParameterByNameWithLabel(_parameterName, surface.ToString());
            }
        }

        bool shouldPlay = grounded && _movementComponent.IsMoving();

        if (shouldPlay && !_isPlaying)
        {
            _eventInstance.start();
            _isPlaying = true;
        }
        else if (!shouldPlay && _isPlaying)
        {
            _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _isPlaying = false;
        }
    }
}
