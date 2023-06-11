using UnityEngine;
using Entities.Opponent;

namespace Waypoints
{
    [RequireComponent(typeof(OpponentNavMesh))]
    public class WaypointNavigator : MonoBehaviour
    {
        private OpponentNavMesh opponentNavMesh;
        private Waypoint waypoint;
        private int direction;

        private void Awake()
        {
            opponentNavMesh = GetComponent<OpponentNavMesh>();
        }

        private void Start()
        {
            direction = Mathf.RoundToInt(Random.Range(0f, 1f));
            opponentNavMesh.SetDestination(waypoint.GetPosition());
        }

        private void Update()
        {
            if (opponentNavMesh.ReachedDestination())
            {
                CalculateNextWaypoint();
                opponentNavMesh.SetDestination(waypoint.GetPosition());
            }
        }

        private void CalculateNextWaypoint()
        {
            if (direction == 0)
            {
                if (waypoint.nextWaypoints != null && waypoint.nextWaypoints.Count > 0)
                {
                    waypoint = waypoint.nextWaypoints[Random.Range(0, waypoint.nextWaypoints.Count)];
                }
                else
                {
                    waypoint = waypoint.previousWaypoints[Random.Range(0, waypoint.previousWaypoints.Count)];
                    direction = 1;
                }
            }
            else if (direction == 1)
            {
                if (waypoint.previousWaypoints != null && waypoint.previousWaypoints.Count > 0)
                {
                    waypoint = waypoint.previousWaypoints[Random.Range(0, waypoint.previousWaypoints.Count)];
                }
                else
                {
                    waypoint = waypoint.nextWaypoints[Random.Range(0, waypoint.nextWaypoints.Count)];
                    direction = 0;
                }
            }
        }

        public void SetInitialWaypoint(Waypoint waypoint)
        {
            this.waypoint = waypoint;
        }
    }
}