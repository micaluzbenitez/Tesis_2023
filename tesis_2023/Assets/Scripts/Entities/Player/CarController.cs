using UnityEngine;
using System;
using System.Collections.Generic;

namespace Entities.Player
{

    public class CarController : MonoBehaviour
    {
        public enum Axel
        {
            Front,
            Back
        }

        [Serializable]
        public struct Wheel
        {
            public GameObject wheelModel;
            public WheelCollider wheelCollider;
            public Axel axel;
        }

        [Header("Car data")]
        [SerializeField] private float maxAcceleration = 30.0f;     // Avanzar
        [SerializeField] private float brakeAcceleration = 50.0f;   // Freno
        [SerializeField] private float turnSensitivity = 1.0f;      // Girar
        [SerializeField] private float maxSteerAngle = 30.0f;       // Direccion del auto
        [SerializeField] private Vector3 centerOfMass;

        [Header("Wheels")]
        [SerializeField] private List<Wheel> wheels;

        private float moveInput;
        private float steerInput;
        private Rigidbody carRigidbody;

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private const float moveSpeed = 600.0f;
        private const float steerSpeed = 0.6f;
        private const float brakeSpeed = 300.0f;

        private CarLifeBehaviour carLifeBehaviour;

        public event Action<float> OnSpeedChange;
        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            carRigidbody = GetComponent<Rigidbody>();
            carRigidbody.centerOfMass = centerOfMass;
            FixWheelColliderVibration();
            carLifeBehaviour = gameObject.AddComponent<CarLifeBehaviour>();
        }

        #region FixWheelCollider
        private void FixWheelColliderVibration()
        {
            //https://docs.unity3d.com/ScriptReference/WheelCollider.ConfigureVehicleSubsteps.html
            const int speedThreshold = 5;
            const int stepsBelowThreshold = 12;
            const int stepsAboveThreshold = 15;

            foreach (Wheel wheel in wheels)
            {
                wheel.wheelCollider.ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);
            }
        }
        #endregion

        private void Update()
        {
            GetInputs();
            AnimateWheels();
            CheckRespawn();
        }

        private void FixedUpdate()
        {
            Move();
            Steer();
            Brake();
            CheckOnSpeedChange();
        }

        private void GetInputs()
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }

        private void Move()
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = moveInput * moveSpeed * maxAcceleration * Time.deltaTime;
            }
        }

        private void Steer()
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    var steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                    wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, steerSpeed);
                }
            }
        }

        private void Brake()
        {
            if (Input.GetKey(KeyCode.Space) || moveInput == 0)
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = brakeSpeed * brakeAcceleration * Time.deltaTime;
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }
            }
        }

        private void AnimateWheels()
        {
            foreach (var wheel in wheels)
            {
                Quaternion rotation;
                Vector3 position;
                wheel.wheelCollider.GetWorldPose(out position, out rotation);
                wheel.wheelModel.transform.position = position;
                wheel.wheelModel.transform.rotation = rotation;
            }
        }
        private void CheckRespawn()
        {
            if (Mathf.Approximately(180f, transform.eulerAngles.z))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    transform.position = initialPosition;
                    transform.rotation = initialRotation;
                }

            }
        }

        private void CheckOnSpeedChange()
        {
            OnSpeedChange?.Invoke(carLifeBehaviour.GetSpeed());
        }
    }
}