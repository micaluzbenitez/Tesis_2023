using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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

        private State state;
        private NavMeshAgent agent;
        private GameObject[] waypoints;
        private int waypointInd;

        private GameObject target;
        private Vector3 direction;
        private bool knockback;
        private bool alive;

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
                waypointInd = Random.Range(0, waypoints.Length - 1);
                if (waypointInd > waypoints.Length - 1) waypointInd = 0;
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
            if (collision.gameObject.CompareTag("Car"))
            {
                direction = collision.transform.forward;
                waypointInd = Random.Range(0, waypoints.Length - 1);
                StartCoroutine(KnockBack());
            }
        }

        private IEnumerator KnockBack()
        {
            knockback = true;
            agent.speed = 2;
            agent.acceleration = 10;

            yield return new WaitForSeconds(0.2f);

            knockback = false;
            agent.speed = 25;
            agent.angularSpeed = 300;
            agent.acceleration = 30;
            target = null;
            state = State.PATROL;
        }

        public void SetWaypoints(GameObject[] waypoints)
        {
            this.waypoints = waypoints;
            waypointInd = Random.Range(0, waypoints.Length - 1);
        }
    }
}