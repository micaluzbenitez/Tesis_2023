using UnityEngine;
using UnityEngine.AI;

namespace Entities.Opponent
{
    public class OpponentNavMesh : MonoBehaviour
    {
        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0)) GoToClick();
        }

        private void GoToClick()
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(movePosition, out var hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }
    }
}