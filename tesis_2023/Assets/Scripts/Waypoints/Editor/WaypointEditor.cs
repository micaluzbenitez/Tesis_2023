using UnityEditor;
using UnityEngine;
using ToolBox;

namespace Waypoints.Editor
{
    [InitializeOnLoad()]
    public class WaypointEditor
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
        {
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.yellow * 0.5f;
            }

            Gizmos.DrawSphere(waypoint.transform.position, 2f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f),
                            waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

            if (waypoint.previousWaypoints != null && waypoint.previousWaypoints.Count > 0)
            {
                Gizmos.color = Color.red;

                for (int i = 0; i < waypoint.previousWaypoints.Count; i++)
                {
                    Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
                    Vector3 offsetTo = waypoint.previousWaypoints[i].transform.right * waypoint.previousWaypoints[i].width / 2f;

                    DrawArrow.ForGizmoTwoPoints(waypoint.transform.position + offset, waypoint.previousWaypoints[i].transform.position + offsetTo, 2f);
                }
            }

            if (waypoint.nextWaypoints != null && waypoint.nextWaypoints.Count > 0)
            {
                Gizmos.color = Color.green;

                for (int i = 0; i < waypoint.nextWaypoints.Count; i++)
                {
                    Vector3 offset = waypoint.transform.right * -waypoint.width / 2f;
                    Vector3 offsetTo = waypoint.nextWaypoints[i].transform.right * -waypoint.nextWaypoints[i].width / 2f;

                    DrawArrow.ForGizmoTwoPoints(waypoint.transform.position + offset, waypoint.nextWaypoints[i].transform.position + offsetTo, 2f);
                }
            }
        }
    }
}