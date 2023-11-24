using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class CarLifeBehaviour : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private int maxHealth = 100;

        [Header("Car parts")]
        [SerializeField] private Rigidbody[] parts = null;

        [Header("Feedbacks")]
        [SerializeField] private ParticleSystem smokeParticles;
        [SerializeField] private ParticleSystem fireParticles;
        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private ParticleSystem destroyParticles;
        [SerializeField] private ParticleSystem lava;
        [SerializeField] private ParticleSystem redSmoke;

        public List<Transform> childs;
        public List<Renderer> renderers;


        public int currentHealth;
        private int score;
        private float previousTime;
        private float speed;
        private float previousSpeed;
        private bool alive = true;

        private Vector3 previousPosition;

        public event Action OnZeroHealth;
        public event Action<int> OnWin;
        public event Action<int> OnTakeDamage;
        public event Action<int> OnIncreaseScore;
        public event Action<float> OnSpeedChange;

        private void Start()
        {

            score = 0;
            currentHealth = maxHealth;
            InitVelocityData();
            FindAllChilds(transform);
            FindAllRenderers();
            ChangeRenderersColors(1, 1, 1);

        }

        private void TakeDamage(float damage)
        {
            currentHealth -= (int)damage;
            OnTakeDamage?.Invoke(currentHealth);

            Debug.Log(gameObject.name + " life: " + currentHealth);

            if (currentHealth <= 25f)
            {
                smokeParticles.Stop();
                fireParticles.Play();
            }
            else if (currentHealth <= 50f)
            {
                smokeParticles.Play();
            }

            if (currentHealth <= 0 && alive)
            {
                fireParticles.Stop();
                explosionParticles.Play();
                destroyParticles.Play();
                OnZeroHealth?.Invoke();
                alive = false;
                DestroyCarParts();
                this.enabled = false;
                ChangeRenderersColors(0, 0, 0);
                redSmoke.Play();
                lava.Play();
            }
        }

        private void InitVelocityData()
        {
            speed = 0f;
            previousPosition = transform.position;
            previousTime = 0f;
        }

        private void FixedUpdate()
        {
            UpdateVelocityData();

            if (Input.GetKeyDown(KeyCode.M)) DestroyCarParts();
        }

        private void UpdateVelocityData()
        {
            Vector3 currentPosition = transform.position;
            float currentTime = Time.time;
            float distance = Vector3.Distance(currentPosition, previousPosition);
            speed = (distance / (currentTime - previousTime)) * 3.6f;
            previousPosition = currentPosition;
            previousTime = currentTime;

            OnSpeedChange?.Invoke(speed);
            previousSpeed = speed;
        }

        private void ToDamageOpponent(Collision collision)
        {
            CarLifeBehaviour otherCarLife = collision.gameObject.GetComponent<CarLifeBehaviour>();

            if (otherCarLife != null)
            {
                otherCarLife.TakeDamage(previousSpeed / 4f);
                score += (int)(previousSpeed / 2f);
                OnIncreaseScore?.Invoke(score);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                Vector3 collisionNormal = collision.contacts[0].normal;
                float dotProduct = Vector3.Dot(transform.forward, collisionNormal);
                float allowedAngle = 0.8f;

                if (dotProduct < -allowedAngle || dotProduct > allowedAngle)
                {
                    Debug.Log("de frente");
                    ToDamageOpponent(collision);

                }
            }
        }

        private void DestroyCarParts()
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].gameObject.transform.SetParent(null);
                parts[i].gameObject.transform.rotation = Random.rotation;

                parts[i].isKinematic = false;
                parts[i].useGravity = true;

                Vector3 explosionDirection = (parts[i].position - transform.position).normalized;
                parts[i].AddForce(explosionDirection * Random.Range(10, 20), ForceMode.VelocityChange);
                parts[i].AddTorque(Random.insideUnitSphere * Random.Range(10, 20), ForceMode.VelocityChange);
            }
        }

        public void Win()
        {
            OnWin?.Invoke(score);
        }

        private void FindAllChilds(Transform objTransform)
        {
            childs.Add(objTransform);

            if (objTransform.childCount > 0)
            {
                for (int i = 0; i < objTransform.childCount; i++)
                {
                    FindAllChilds(objTransform.GetChild(i));
                }
            }
        }
        private void FindAllRenderers()
        {
            foreach (var child in childs)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderers.Add(renderer);
                }
            }
        }

        private void ChangeRenderersColors(float r, float g, float b)
        {

            foreach (var renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;
                Color newColor = new Color(r, g, b);


                foreach (var material in materials)
                {
                    if (material != null)
                    {
                        if (material.name != "OutlineShader" && material.name != "OutlineShader2")
                        {
                            material.color = newColor;
                        }
                        else
                        {
                            material.color = new Color(0, 0, 0);
                        }
                    }
                }

            }
        }
    }
}