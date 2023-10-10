using UnityEngine;
using System;
using System.Collections.Generic;
using static Entities.Player.CarController;

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
        [SerializeField] private Transform centerOfMass;

        [Header("Wheels")]
        [SerializeField] private List<Wheel> wheels;

        [Header("Turbo")]
        [SerializeField] private float turboForce = 20000f;
        [SerializeField] private float turboCapacity = 100.0f;
        [SerializeField] private float turboRechargeRate = 10.0f;
        [SerializeField] private float turboConsumptionRate;

        private Rigidbody carRigidbody;

        private bool braked = false;
        private Vector3 prevPosition;
        public float currentSpeed = 0.0f;
        private bool isDead = false;
        private float maxTorque = 1000;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        public event Action<float> OnSpeedChange;
        public event Action OnCarFlip;
        public event Action OnCarStraighten;

        private bool isFlipped = false;
        private bool onFloor = false;

        private float currentTurbo = 0;
        private bool isTurboActive = false;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            prevPosition = transform.position;
            carRigidbody = GetComponent<Rigidbody>();
            carRigidbody.centerOfMass = centerOfMass.transform.localPosition;
        }

        private void FixedUpdate()
        {
            if (!isDead)
            {
                if (!braked)
                {
                    foreach (var wheel in wheels)
                    {
                        wheel.wheelCollider.brakeTorque = 0;
                    }
                }

                // Speed of car, Car will move as you will provide the input to it.
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.motorTorque = maxTorque * Input.GetAxis("Vertical");
                }

                // Changing car direction Here we are changing the steer angle of the front tires of the car so that we can change the car direction.
                foreach (var wheel in wheels)
                {
                    if (wheel.axel == Axel.Front)
                    {
                        wheel.wheelCollider.steerAngle = 15 * Input.GetAxis("Horizontal");
                    }
                }
            }
        }

        private void Update()
        {
            Vector3 curMov = transform.position - prevPosition;
            currentSpeed = curMov.magnitude / Time.deltaTime;
            prevPosition = transform.position;

            Brake();

            // For tire rotate
            foreach (var wheel in wheels)
            {
                wheel.wheelModel.transform.Rotate(wheel.wheelCollider.rpm / 60 * 360 * Time.deltaTime, 0, 0);
            }

            // Changing tire direction
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    Vector3 temp = wheel.wheelModel.transform.localEulerAngles;
                    temp.y = wheel.wheelCollider.steerAngle - wheel.wheelModel.transform.localEulerAngles.z;
                    wheel.wheelModel.transform.localEulerAngles = temp;
                }
            }

            AnimateWheels();
            CheckRespawn();

            // Turbo
            CheckTurbo();
            RechargeTurbo();
            if (currentTurbo < turboConsumptionRate) DeactivateTurbo();
            Debug.Log(currentTurbo);
        }

        private void Brake()
        {
            if (Input.GetButton("Jump")) braked = true;
            else braked = false;

            if (braked)
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 10000;
                    wheel.wheelCollider.motorTorque = 0;
                }
            }
        }

        private void CheckTurbo()
        {
            if (Input.GetKey(KeyCode.LeftShift)) ActivateTurbo();
            else DeactivateTurbo();
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
            if (!onFloor) return;

            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 10000;
            }

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

        public void DisableCarController()
        {
            isDead = true;

            for (int i = 0; i < wheels.Count; i++)
            {
                Destroy(wheels[i].wheelCollider);
            }

            Destroy(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Floor")) onFloor = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Floor")) onFloor = false;
        }

        private void RechargeTurbo()
        {
            if (!isTurboActive && currentTurbo < turboCapacity)
            {
                currentTurbo += turboRechargeRate * Time.deltaTime / 2;
                currentTurbo = Mathf.Clamp(currentTurbo, 0f, turboCapacity);
            }
        }

        private void ActivateTurbo()
        {
            if (currentTurbo >= turboConsumptionRate)
            {
                isTurboActive = true;
                currentTurbo -= turboConsumptionRate;

                carRigidbody.AddForce(transform.forward * turboForce);
                Debug.Log("hola");
            }
        }

        private void DeactivateTurbo()
        {
            isTurboActive = false;
        }
    }
}