using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class CarController : MonoBehaviour
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
            }
        }
    }
}