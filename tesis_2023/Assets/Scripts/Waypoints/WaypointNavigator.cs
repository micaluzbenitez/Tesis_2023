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
                TakeWaypoint();
                opponentNavMesh.SetDestination(currentWaypoint.GetPosition());
            }
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