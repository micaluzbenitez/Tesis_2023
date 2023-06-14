using UnityEngine;

namespace Entities.Player
{
    public class EngineSound : MonoBehaviour
    {
        [Header("Speed")]
        public float minSpeed;
        public float maxSpeed;

        [Header("Pitch")]
        public float minPitch;
        public float maxPitch;

        private Rigidbody carRigidbody;
        private AudioSource carAudio;
        private float currentSpeed;
        private float currentPitch;

        private void Start()
        {
            carAudio = GetComponent<AudioSource>();
            carRigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            UpdateEngineSound();
        }

        private void UpdateEngineSound()
        {
            currentSpeed = carRigidbody.velocity.magnitude;
            currentPitch = carRigidbody.velocity.magnitude / 60f;

            if (currentSpeed < minSpeed)
            {
                carAudio.pitch = minPitch;
            }

            if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
            {
                carAudio.pitch = minPitch + currentPitch;
            }

            if (currentSpeed > maxSpeed)
            {
                carAudio.pitch = maxPitch;
            }
        }
    }
}