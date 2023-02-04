using UnityEngine;

namespace Snake3
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTf;
        [SerializeField] private Transform _snakeTf;
        [SerializeField] private Transform _worldTf;
        [SerializeField] private float _cameraDistance = 15f;

        private bool _wasFixedUpdate = true;
        private Vector3 _moveTarget;
        private float _moveDistance;
        private Vector3 _snakeUpVector;
        private float _fixedUpdateTime;

        private void Start()
        {
            var snakePos = _snakeTf.position;
            var worldPos = _worldTf.position;

            _cameraTf.position = (snakePos - worldPos).normalized * _cameraDistance;
            _cameraTf.LookAt(worldPos, _snakeTf.rotation * Vector3.up);
        }

        private void FixedUpdate()
        {
            _wasFixedUpdate = true;
            _fixedUpdateTime = Time.time;  // Todo: may not be needed
        }

        private void Update()
        {
            // Todo: don't need to cache all of this here
            var worldPos = _worldTf.position;
            var cameraPos = _cameraTf.position;
            var cameraRot = _cameraTf.rotation;
            var snakePos = _snakeTf.position;
            var worldToSnakeVector = snakePos - worldPos;

            if (_wasFixedUpdate)
            {
                _snakeUpVector = _snakeTf.rotation * Vector3.up;
                _moveTarget = worldToSnakeVector.normalized * _cameraDistance;
                _moveDistance = (cameraPos - _moveTarget).magnitude;
                _wasFixedUpdate = false;
            }

            var lookRotation = Quaternion.LookRotation(worldPos - cameraPos, _snakeUpVector);
            var rotationDistance = Quaternion.Angle(cameraRot, lookRotation);  // Todo.

            var timeSinceFixedUpdate = Time.time - _fixedUpdateTime;
            var updateRatio = timeSinceFixedUpdate / Time.fixedDeltaTime;
            var updateFixedRatio = Time.deltaTime / Time.fixedDeltaTime;

            _cameraTf.position = Vector3.MoveTowards(cameraPos, _moveTarget, _moveDistance * updateFixedRatio);
            //_cameraTf.LookAt(worldPos, cameraRot * Vector3.up);
            //_cameraTf.rotation = Quaternion.Slerp(cameraRot, lookRotation, .01f);  // Ratio from 0 to 1
            //_cameraTf.rotation = Quaternion.Lerp(cameraRot, lookRotation, .01f);  // Ratio from 0 to 1
            _cameraTf.rotation = Quaternion.RotateTowards(cameraRot, lookRotation, rotationDistance * updateFixedRatio);  // Max degrees
        }
    }
}
