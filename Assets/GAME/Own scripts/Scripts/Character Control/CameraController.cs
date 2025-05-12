using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

namespace Synty.AnimationBaseLocomotion.Samples
{
    public class CameraController : MonoBehaviour
    {
       
        private const int _LAG_DELTA_TIME_ADJUSTMENT = 20;

       
        [Header("Character & Camera References")]
        [Tooltip("The character game object")]
        [SerializeField] private GameObject _syntyCharacter;
        [Tooltip("Main camera used for player perspective")]
        [SerializeField] private Camera _mainCamera;

        [Header("Target Transforms")]
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Transform _lockOnTarget;

        [Header("Input & Control Settings")]
        [SerializeField] private bool _invertCamera;
        [SerializeField] private bool _hideCursor;
        [SerializeField] private bool _isLockedOn;

        [Header("Camera Movement Settings")]
        [SerializeField] private float _mouseSensitivity;
        [SerializeField] private float _cameraDistance;
        [SerializeField] private float _cameraHeightOffset;
        [SerializeField] private float _cameraHorizontalOffset;
        [SerializeField] private float _cameraTiltOffset;
        [SerializeField] private Vector2 _cameraTiltBounds = new Vector2(-10f, 45f);
        [SerializeField] private float _positionalCameraLag = 1f;
        [SerializeField] private float _rotationalCameraLag = 1f;

        
        private float _cameraInversion;
        private InputReader _inputReader;
        private Transform _syntyCamera;

        private float _lastAngleX, _lastAngleY;
        private float _newAngleX, _newAngleY;
        private float _rotationX, _rotationY;

        private Vector3 _lastPosition, _newPosition;

      
        private void Start()
        {
            InitializeReferences();
            SetupCursor();
            InitializeCamera();
            SyncInitialCameraTransform();
        }

        private void Update()
        {
            UpdateRotation();
            UpdatePosition();
            UpdateCameraTransform();
            SavePreviousState();
        }

       
        private void InitializeReferences()
        {
            _syntyCamera = transform.GetChild(0);
            _inputReader = _syntyCharacter.GetComponent<InputReader>();
            _playerTarget = _syntyCharacter.transform.Find("SyntyPlayer_LookAt");
            _lockOnTarget = _syntyCharacter.transform.Find("TargetLockOnPos");
        }

        private void SetupCursor()
        {
            if (_hideCursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void InitializeCamera()
        {
            _cameraInversion = _invertCamera ? 1 : -1;
        }

        private void SyncInitialCameraTransform()
        {
            transform.position = _playerTarget.position;
            transform.rotation = _playerTarget.rotation;
            _lastPosition = transform.position;

            _syntyCamera.localPosition = new Vector3(_cameraHorizontalOffset, _cameraHeightOffset, -_cameraDistance);
            _syntyCamera.localEulerAngles = new Vector3(_cameraTiltOffset, 0f, 0f);
        }

       
        private void UpdateRotation()
        {
            float rotationSpeed = 1 / (_rotationalCameraLag / _LAG_DELTA_TIME_ADJUSTMENT);

            _rotationX = _inputReader._mouseDelta.y * _cameraInversion * _mouseSensitivity;
            _rotationY = _inputReader._mouseDelta.x * _mouseSensitivity;

            _newAngleX += _rotationX;
            _newAngleX = Mathf.Clamp(_newAngleX, _cameraTiltBounds.x, _cameraTiltBounds.y);
            _newAngleX = Mathf.Lerp(_lastAngleX, _newAngleX, rotationSpeed * Time.deltaTime);

            if (_isLockedOn)
            {
                Vector3 aimVector = _lockOnTarget.position - _playerTarget.position;
                Quaternion targetRotation = Quaternion.LookRotation(aimVector);
                targetRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                _newAngleY = targetRotation.eulerAngles.y;
            }
            else
            {
                _newAngleY += _rotationY;
                _newAngleY = Mathf.Lerp(_lastAngleY, _newAngleY, rotationSpeed * Time.deltaTime);
            }
        }

        private void UpdatePosition()
        {
            float positionSpeed = 1 / (_positionalCameraLag / _LAG_DELTA_TIME_ADJUSTMENT);
            _newPosition = Vector3.Lerp(_lastPosition, _playerTarget.position, positionSpeed * Time.deltaTime);
            transform.position = _newPosition;
        }

        private void UpdateCameraTransform()
        {
            transform.eulerAngles = new Vector3(_newAngleX, _newAngleY, 0);
            _syntyCamera.localPosition = new Vector3(_cameraHorizontalOffset, _cameraHeightOffset, -_cameraDistance);
            _syntyCamera.localEulerAngles = new Vector3(_cameraTiltOffset, 0f, 0f);
        }

        private void SavePreviousState()
        {
            _lastPosition = _newPosition;
            _lastAngleX = _newAngleX;
            _lastAngleY = _newAngleY;
        }

        
        public void LockOn(bool enable, Transform newLockOnTarget)
        {
            _isLockedOn = enable;

            if (newLockOnTarget != null)
            {
                _lockOnTarget = newLockOnTarget;
            }
        }

        #region Camera Info Getters
        public Vector3 GetCameraPosition() => _mainCamera.transform.position;
        public Vector3 GetCameraForward() => _mainCamera.transform.forward;
        public Vector3 GetCameraForwardZeroedY() => new Vector3(_mainCamera.transform.forward.x, 0, _mainCamera.transform.forward.z);
        public Vector3 GetCameraForwardZeroedYNormalised() => GetCameraForwardZeroedY().normalized;
        public Vector3 GetCameraRightZeroedY() => new Vector3(_mainCamera.transform.right.x, 0, _mainCamera.transform.right.z);
        public Vector3 GetCameraRightZeroedYNormalised() => GetCameraRightZeroedY().normalized;
        public float GetCameraTiltX() => _mainCamera.transform.eulerAngles.x;
        #endregion
    }
}
