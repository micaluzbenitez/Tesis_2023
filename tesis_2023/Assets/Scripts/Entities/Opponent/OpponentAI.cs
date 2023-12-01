using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Entities.Opponent
{
    public class OpponentAI : MonoBehaviour
    {
        public enum State
        {
            PATROL,
            CHASE
        }

        [Header("Speed")]
        [SerializeField] private float patrolSpeed = 0.5f;
        [SerializeField] private float chaseSpeed = 1f;

        [Header("Waypoint")]
        [SerializeField] private float distancePerWaypoint = 10f;

        [Header("KnockBack")]
        [SerializeField] private float knockbackInitialSpeed = 2f;
        [SerializeField] private float knockbackInitialAcceleration = 10f;
        [SerializeField] private float knockbackSpeed = 25f;
        [SerializeField] private float knockbackAcceleration = 30f;
        [SerializeField] private float knockbackAngularSpeed = 300f;

        private State state;
        private NavMeshAgent agent;
        private List<GameObject> waypoints;
        private int waypointInd;

        private GameObject target;
        private Vector3 direction;
        private bool knockback;
        private bool alive;

        public Action<GameObject> OnDeath;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = true;
            agent.updateRotation = true;

            state = State.PATROL;
            alive = true;
        }

        private void Update()
        {
            UpdateFSM();
        }

        private void FixedUpdate()
        {
            if (knockback) agent.velocity = direction * 10;
        }

        private void UpdateFSM()
        {
            if (alive)
            {
                if (state == State.PATROL) Patrol();
                else Chase();
            }
            else
            {
                agent.enabled = false;
            }
        }

        private void Patrol()
        {
            agent.speed = patrolSpeed;

            if (Vector3.Distance(transform.position, waypoints[waypointInd].transform.position) >= distancePerWaypoint)
            {
                agent.SetDestination(waypoints[waypointInd].transform.position);
            }
            else if (Vector3.Distance(transform.position, waypoints[waypointInd].transform.position) <= distancePerWaypoint)
            {
                waypointInd = Random.Range(0, waypoints.Count - 1);
                if (waypointInd > waypoints.Count - 1) waypointInd = 0;
            }
        }

        private void Chase()
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.transform.position);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Car"))
            {
                state = State.CHASE;
                target = collision.gameObject;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (alive)
            {
                if (collision.gameObject.CompareTag("Car"))
                {
                    direction = collision.transform.forward;
                    waypointInd = Random.Range(0, waypoints.Count - 1);
                    StartCoroutine(KnockBack());
                }
            }
        }

        private IEnumerator KnockBack()
        {
            knockback = true;
            agent.speed = knockbackInitialSpeed;
            agent.acceleration = knockbackInitialAcceleration;

            yield return new WaitForSeconds(0.2f);

            if (alive)
            {

                knockback = false;
                agent.speed = knockbackSpeed;
                agent.angularSpeed = knockbackAngularSpeed;
                agent.acceleration = knockbackAcceleration;
                target = null;
                state = State.PATROL;
            }
        }

        public void SetWaypoints(List<GameObject> waypoints)
        {
            this.waypoints = waypoints;
            waypointInd = Random.Range(0, waypoints.Count - 1);
        }

        public void DisableIA()
        {
            alive = false;
            enabled = false;
            agent = null;
            OnDeath?.Invoke(waypoints[waypointInd]);
        }

        public float GetVelocity()
        {
            return agent.velocity.magnitude;
        }
        public bool IsAlive()
        {
            return alive;
        }
    }
}