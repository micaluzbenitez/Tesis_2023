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

        [Header("Car")]
        [SerializeField] private Vector3 centerOfMass;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 600.0f;
        [SerializeField] private float maxAcceleration = 30.0f;     // Avanzar

        [Header("Brake")]
        [SerializeField] private float brakeAcceleration = 50.0f;   // Freno
        [SerializeField] private float brakeSpeed = 300.0f;

        [Header("Turn")]
        [SerializeField] private float turnSensitivity = 1.0f;      // Girar
        [SerializeField] private float maxSteerAngle = 30.0f;       // Direccion del auto
        [SerializeField] private float steerSpeed = 0.6f;

        [Header("Wheels")]
        [SerializeField] private List<Wheel> wheels;

        private float moveInput;
        private float steerInput;
        private Rigidbody carRigidbody;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        public event Action<float> OnSpeedChange;
        public event Action OnCarFlip;
        public event Action OnCarStraighten;

        private bool isFlipped = false;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            carRigidbody = GetComponent<Rigidbody>();
            carRigidbody.centerOfMass = centerOfMass;
            FixWheelColliderVibration();
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
            //CheckOnSpeedChange();
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
                wheel.wheelCollider.motorTorque = moveInput * moveSpeed * maxAcceleration;
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
                    wheel.wheelCollider.brakeTorque = brakeSpeed * brakeAcceleration;
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

            if ((transform.eulerAngles.z > 90f && transform.eulerAngles.z < 350f) || transform.eulerAngles.z < -90f)
            {
                isFlipped = true;
                OnCarFlip?.Invoke();
                if (Input.GetKeyDown(KeyCode.R))
                {
                    transform.position = initialPosition;
                    transform.rotation = initialRotation;
                }

            }
            else if (isFlipped)
            {
                isFlipped = false;
                OnCarStraighten?.Invoke();
            }
        }

    }
}