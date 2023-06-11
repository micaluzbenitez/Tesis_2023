using System.Collections.Generic;
using UnityEngine;

namespace Waypoints
{
    public class Waypoint : MonoBehaviour
    {
        [Header("Waypoint")]
        public float width = 1f;
        public List<Waypoint> previousWaypoints = new List<Waypoint>();
        public List<Waypoint> nextWaypoints = new List<Waypoint>();

        public Vector3 GetPosition()
        {
            Vector3 minBound = transform.position + transform.right * width / 2f;
            Vector3 maxBound = transform.position - transform.right * width / 2f;

            return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
        }
    }
}