using UnityEngine;
using UnityEngine.AI;

namespace Entities.Opponent
{
    public class OpponentNavMesh : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        private Vector3 currentDestination;

        public void SetDestination(Vector3 position)
        {
            currentDestination = position;
            agent.SetDestination(currentDestination);
        }

        public bool ReachedDestination()
        {
            float distance = Vector3.Distance(agent.transform.position, currentDestination);
            return distance < 1;
        }
    }
}