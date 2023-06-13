using UnityEngine;

namespace Entities.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Position")]
        [SerializeField] private float moveSmoothness;
        [SerializeField] private Vector3 moveOffset;

        [Header("Rotation")]
        [SerializeField] private float rotationSmoothness;
        [SerializeField] private Vector3 rotationOffset;

        [Header("Target")]
        [SerializeField] private Transform carTarget;

        private void FixedUpdate()
        {
            Movement();
            Rotation();
        }

        private void Movement()
        {
            Vector3 targetPos = carTarget.TransformPoint(moveOffset);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
        }

        private void Rotation()
        {
            var direction = carTarget.position - transform.position;
            var rotation = Quaternion.LookRotation(direction + rotationOffset, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSmoothness * Time.deltaTime);
        }
    }
}