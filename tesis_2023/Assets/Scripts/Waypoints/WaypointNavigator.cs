using UnityEngine;
using Entities.Opponent;

namespace Waypoints
{
    [RequireComponent(typeof(OpponentNavMesh))]
    public class WaypointNavigator : MonoBehaviour
    {
        public Waypoint currentWaypoint;

        private OpponentNavMesh opponentNavMesh;
        private int direction;

        private void Awake()
        {
            opponentNavMesh = GetComponent<OpponentNavMesh>();
        }

        private void Start()
        {
            direction = Mathf.RoundToInt(Random.Range(0f, 1f));
            opponentNavMesh.SetDestination(currentWaypoint.GetPosition());
        }

        private void Update()
        {
            if (opponentNavMesh.ReachedDestination())
            {
                bool shouldBranch = false;
                if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
                {
                    shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
                }

                if (shouldBranch) TakeBranch();
                else TakeWaypoint();

                opponentNavMesh.SetDestination(currentWaypoint.GetPosition());
            }
        }

        private void TakeBranch()
        {
            currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
        }

        private void TakeWaypoint()
        {
            if (direction == 0)
            {
                if (currentWaypoint.nextWaypoint)
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }
                else
                {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                    direction = 1;
                }
            }
            else if (direction == 1)
            {
                if (currentWaypoint.previousWaypoint)
                {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                }
                else
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                    direction = 0;
                }
            }
        }
    }
}