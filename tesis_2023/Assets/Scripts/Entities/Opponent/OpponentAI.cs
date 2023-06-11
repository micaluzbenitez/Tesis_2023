using UnityEngine;

namespace Entities.Opponent
{ 
    public class OpponentAI : MonoBehaviour
    {
        private OpponentNavMesh opponentNavMesh;
        private bool goingToTarget = false;

        private void Awake()
        {
            opponentNavMesh = GetComponentInParent<OpponentNavMesh>();
        }

        private void Update()
        {
            if (goingToTarget && opponentNavMesh.ReachedDestination())
            {
                ResetOpponentNavMesh();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                goingToTarget = true;
                opponentNavMesh.SetDestination(other.gameObject.transform.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                ResetOpponentNavMesh();
            }
        }

        private void ResetOpponentNavMesh()
        {
            goingToTarget = false;
            opponentNavMesh.ResetDestination();
        }
    }
}