using UnityEngine;

namespace Snake3
{
    /**
     * Controls the movement of game camera.
     */
    public class CameraController : MonoBehaviour
    {
        // ----------------------------
        // Fields
        // ----------------------------

        [SerializeField] private Transform _cameraTf;
        [SerializeField] private Transform _snakeTf;
        [SerializeField] private Transform _worldTf;
        [SerializeField] private float _cameraDistance;

        private Vector3 _snakeUpVector;
        private Vector3 _moveTarget;
        private float _moveDistance;
        private bool _wasFixedUpdate;

        // ----------------------------
        // Event Functions
        // ----------------------------

        private void Start()
        {
            InitializeCameraPosition();
            _wasFixedUpdate = true;  // To force recalculation on first Update().
        }

        private void FixedUpdate()
        {
            _wasFixedUpdate = true;
        }

        private void Update()
        {
            MoveCameraOneStep();
        }

        // ----------------------------
        // Methods
        // ----------------------------

        /**
         * Set initial camera position and rotation.
         */
        private void InitializeCameraPosition()
        {
            var snakePos = _snakeTf.position;
            var worldPos = _worldTf.position;
            _cameraTf.position = (snakePos - worldPos).normalized * _cameraDistance;
            _cameraTf.LookAt(worldPos, _snakeTf.rotation * Vector3.up);
        }

        /**
         * Move the camera one update-step closer to its target location.
         */
        private void MoveCameraOneStep()
        {
            var worldPos = _worldTf.position;
            var cameraPos = _cameraTf.position;
            var cameraRot = _cameraTf.rotation;
            var worldToSnakeVector = _snakeTf.position - worldPos;

            // Snake moves on fixed update, so we need to recalculate some variables.
            if (_wasFixedUpdate)
            {
                _snakeUpVector = _snakeTf.rotation * Vector3.up;
                _moveTarget = worldToSnakeVector.normalized * _cameraDistance;
                _moveDistance = (cameraPos - _moveTarget).magnitude;
                _wasFixedUpdate = false;
            }

            var lookRotation = Quaternion.LookRotation(worldPos - cameraPos, _snakeUpVector);
            var rotationDistance = Quaternion.Angle(cameraRot, lookRotation);
            var updateRatio = Time.deltaTime / Time.fixedDeltaTime;

            _cameraTf.position = Vector3.MoveTowards(
                cameraPos, _moveTarget, _moveDistance * updateRatio);
            _cameraTf.rotation = Quaternion.RotateTowards(
                cameraRot, lookRotation, rotationDistance * updateRatio);
        }
    }
}
