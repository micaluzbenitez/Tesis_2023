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

        private void CreateWaypoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot, false);
            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

            if (waypointRoot.childCount > 1)
            {
                waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
                waypoint.previousWaypoint.nextWaypoint = waypoint;
                // Place the waypoint at the last position
                waypoint.transform.position = waypoint.previousWaypoint.transform.position;
                waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
            }

            Selection.activeGameObject = waypoint.gameObject;
        }

        private void CreatePreviousWaypoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot, false);
            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            waypoint.transform.position = selectedWaypoint.transform.position;
            waypoint.transform.forward = selectedWaypoint.transform.forward;

            if (selectedWaypoint.previousWaypoint)
            {
                waypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
                selectedWaypoint.previousWaypoint.nextWaypoint = waypoint;
            }

            waypoint.nextWaypoint = selectedWaypoint;
            selectedWaypoint.previousWaypoint = waypoint;
            waypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

            Selection.activeGameObject = waypoint.gameObject;
        }

        private void CreateNextWaypoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(waypointRoot, false);
            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            waypoint.transform.position = selectedWaypoint.transform.position;
            waypoint.transform.forward = selectedWaypoint.transform.forward;

            waypoint.previousWaypoint = selectedWaypoint;
            if (selectedWaypoint.nextWaypoint)
            {
                selectedWaypoint.nextWaypoint.previousWaypoint = waypoint;
                waypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            }

            selectedWaypoint.nextWaypoint = waypoint;
            waypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex() + 1);

            Selection.activeGameObject = waypoint.gameObject;
        }

        private void RemoveWaypoint()
        {
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

            if (selectedWaypoint.nextWaypoint)
            {
                selectedWaypoint.nextWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            }
            if (selectedWaypoint.previousWaypoint)
            {
                selectedWaypoint.previousWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
                Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
            }

            DestroyImmediate(selectedWaypoint.gameObject);
        }
    }
}