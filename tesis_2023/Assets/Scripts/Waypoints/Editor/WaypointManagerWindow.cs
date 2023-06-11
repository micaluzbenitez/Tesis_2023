using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Waypoints.Editor
{
    public class WaypointManagerWindow : EditorWindow
    {
        [MenuItem("Tools/Waypoint Editor")]
        public static void Open()
        {
            GetWindow<WaypointManagerWindow>();
        }

        [SerializeField] private Transform waypointRoot;
        private Waypoint lastWaypointCreated;

        private void OnGUI()
        {
            SerializedObject obj = new SerializedObject(this);

            EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

            if (!waypointRoot)
            {
                EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                DrawButtons();
                EditorGUILayout.EndVertical();
            }

            obj.ApplyModifiedProperties();
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Create Waypoint")) CreateWaypoint();

            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<Waypoint>())
            {
                if (GUILayout.Button("Create Previous Waypoint")) CreatePreviousWaypoint();
                if (GUILayout.Button("Create Next Waypoint")) CreateNextWaypoint();
                if (GUILayout.Button("Remove Waypoint")) RemoveWaypoint();
            }
        }

        private Waypoint CreateWaypoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot, false);

            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
            if (lastWaypointCreated)
            {
                waypoint.transform.position = lastWaypointCreated.transform.position;
                waypoint.transform.forward = lastWaypointCreated.transform.forward;
            }

            lastWaypointCreated = waypoint;
            Selection.activeGameObject = waypoint.gameObject;
            return waypoint;
        }

        private void CreatePreviousWaypoint()
        {
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            Waypoint waypoint = CreateWaypoint();
            selectedWaypoint.previousWaypoints.Add(waypoint);
        }

        private void CreateNextWaypoint()
        {
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            Waypoint waypoint = CreateWaypoint();
            selectedWaypoint.nextWaypoints.Add(waypoint);
        }

        private void RemoveWaypoint()
        {
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

            for (int i = 0; i < waypointRoot.childCount; i++)
            {
                Waypoint waypoint = waypointRoot.GetChild(i).GetComponent<Waypoint>();
                if (waypoint.nextWaypoints.Contains(selectedWaypoint)) waypoint.nextWaypoints.Remove(selectedWaypoint);
                if (waypoint.previousWaypoints.Contains(selectedWaypoint)) waypoint.previousWaypoints.Remove(selectedWaypoint);
            }

            DestroyImmediate(selectedWaypoint.gameObject);
        }
    }
}