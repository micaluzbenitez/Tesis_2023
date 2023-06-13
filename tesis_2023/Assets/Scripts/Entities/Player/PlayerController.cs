using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        public class AxleData
        {
            public string tag;
            public WheelCollider rightWheel;
            public WheelCollider leftWheel;
            [Tooltip("Is this wheel attached to motor?")] public bool motor;
            [Tooltip("Does this wheel apply steer angle?")] public bool steering;
        }

        [Header("Car data")]
        [SerializeField, Tooltip("Maximum torque the motor can apply to wheel")] private float maxMotorTorque;
        [SerializeField, Tooltip("Maximum steer angle the wheel can have")] private float maxSteeringAngle;

        [Header("Wheels")]
        [SerializeField] private List<AxleData> axleData;

        // finds the corresponding visual wheel
        // correctly applies the transform
        public void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            if (collider.transform.childCount == 0) return;

            Transform visualWheel = collider.transform.GetChild(0);

            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);

            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }

        private void FixedUpdate()
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            Movement(motor, steering);
        }

        private void Movement(float motor, float steering)
        {
            foreach (AxleData axleInfo in axleData)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }
        }
    }
}