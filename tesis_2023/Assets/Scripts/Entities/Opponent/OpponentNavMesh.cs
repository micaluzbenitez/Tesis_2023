using UnityEngine;
using UnityEngine.AI;

namespace Entities.Opponent
{
    public class OpponentNavMesh : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private OpponentAI opponentAI;

        private Vector3 previousDestination;
        private Vector3 currentDestination;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                ResetDestination();
                opponentAI.ResetState();
            }
        }

        public void SetDestination(Vector3 position)
        {
            previousDestination = currentDestination;
            currentDestination = position;
            agent.SetDestination(currentDestination);
        }

        public void ResetDestination()
        {
            currentDestination = previousDestination;
            agent.SetDestination(currentDestination);
        }

        public bool ReachedDestination()
        {
            float distance = Vector3.Distance(transform.position, currentDestination);
            return distance < 1;
        }
    }
}