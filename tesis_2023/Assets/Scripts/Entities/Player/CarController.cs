using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

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

        [Header("Drift")]
        [SerializeField] float driftFactor = 0.95f;

        [Header("Feedbacks")]
        [SerializeField] private ParticleSystem DriftParticles;
        [SerializeField] private ParticleSystem turboParticles;

        [Header("Unity events")]
        [SerializeField] private UnityEvent OnActiveTurbo;
        [SerializeField] private UnityEvent OnDesactiveTurbo;

        private Rigidbody carRigidbody;

        private bool braked = false;
        private Vector3 prevPosition;
        private float currentSpeed = 0.0f;
        private bool isDead = false;
        private float maxTorque = 2000;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        public event Action<float> OnSpeedChange;
        public event Action OnCarFlip;
        public event Action OnCarStraighten;
        public event Action<float> OnTurboChange;

        private bool isFlipped = false;
        private bool onFloor = false;

        private float currentTurbo = 0;
        private bool isTurboActive;

        private List<WheelCollider> driveWheels = new List<WheelCollider>();

        private WheelFrictionCurve originalFriction;
        private bool isDrifting = false;

        [SerializeField] LayerMask groundLayer;

        [SerializeField] private GameObject hitParticles;
        private void Start()
        {
            isTurboActive = false;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            prevPosition = transform.position;
            carRigidbody = GetComponent<Rigidbody>();
            carRigidbody.centerOfMass = centerOfMass.transform.localPosition;

            for (int i = 0; i < wheels.Count; i++)
            {
                driveWheels.Add(wheels[i].wheelCollider);
            }

            originalFriction = driveWheels[0].sidewaysFriction;
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
                    float verticalInput = Input.GetAxis("Vertical");

                    // Cambia esta línea para permitir que el auto se mueva hacia atrás inmediatamente al presionar la tecla "S"
                    wheel.wheelCollider.motorTorque = maxTorque * verticalInput;

                    // Agrega fuerza hacia adelante o hacia atrás al cuerpo del auto según la entrada vertical
                    carRigidbody.AddForce(transform.forward * maxTorque * verticalInput);

                    // Changing car direction Here we are changing the steer angle of the front tires of the car so that we can change the car direction.
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


            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                StartDrift();
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                StopDrift();
            }

            if (isDrifting)
            {
                ApplyDrift();
            }
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                ActivateTurbo();
            }

            else if (isTurboActive)
            {
                DeactivateTurbo();
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
            if (!onFloor) return;

            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 10000;
            }

            Vector3 upDirection = transform.TransformDirection(Vector3.up);
            Vector3 linecastStart = transform.position;
            Vector3 linecastEnd = transform.position + upDirection * 6f;

            if (Physics.Linecast(linecastStart, linecastEnd, out RaycastHit hit, groundLayer))
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

            Debug.DrawLine(linecastStart, linecastEnd, Color.red);
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
            if (other.CompareTag("Floor"))
            {
                onFloor = true;
                if (isDrifting) DriftParticles.Play();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Floor"))
            {
                onFloor = false;
                DriftParticles.Stop();
            }
        }

        private void RechargeTurbo()
        {
            if (!isTurboActive && currentTurbo < turboCapacity)
            {
                currentTurbo += turboRechargeRate * Time.deltaTime / 2;
                currentTurbo = Mathf.Clamp(currentTurbo, 0f, turboCapacity);

                OnTurboChange?.Invoke(currentTurbo);
            }
        }

        private void ActivateTurbo()
        {
            if (currentTurbo >= turboConsumptionRate)
            {
                turboParticles.Play();
                isTurboActive = true;
                currentTurbo -= turboConsumptionRate;

                carRigidbody.AddForce(transform.forward * turboForce);

                OnTurboChange?.Invoke(currentTurbo);
                Debug.Log(turboParticles.isPlaying);
                //Debug.Log("se activo el turbo");

                OnActiveTurbo?.Invoke();
            }
        }

        private void DeactivateTurbo()
        {
            isTurboActive = false;
            turboParticles.Stop();
            OnDesactiveTurbo?.Invoke();
        }

        private void StartDrift()
        {
            isDrifting = true;
            DriftParticles.Play();

            foreach (WheelCollider wheel in driveWheels)
            {
                WheelFrictionCurve driftFriction = new WheelFrictionCurve();
                driftFriction.stiffness = 0.25f;
                wheel.sidewaysFriction = driftFriction;
            }
        }

        private void ApplyDrift()
        {
            foreach (WheelCollider wheel in driveWheels)
            {
                wheel.motorTorque = 0f;
            }
        }

        private void StopDrift()
        {
            isDrifting = false;
            DriftParticles.Stop();

            foreach (WheelCollider wheel in driveWheels)
            {

                wheel.sidewaysFriction = originalFriction;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                Instantiate(hitParticles, collision.contacts[0].point, Quaternion.identity);
            }
        }
    }
}