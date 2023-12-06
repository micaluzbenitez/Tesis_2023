using System.Collections.Generic;
using UnityEngine;
using Managers;
using UnityEngine.AI;

namespace Arena
{
    public class ArenaData : MonoBehaviour
    {
        [Header("AI")]
        [SerializeField] private NavMeshData navMeshData_A;

        [Header("Opponents manager")]
        [SerializeField] private OpponentsManager opponentsManager;

        [Header("Waypoints")]
        [SerializeField] private List<GameObject> waypoints = new List<GameObject>();

        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip musicClip;

        private void Awake()
        {
            NavMesh.RemoveAllNavMeshData();
            NavMesh.AddNavMeshData(navMeshData_A);

            opponentsManager.SetWaypoints(waypoints);
            musicSource.clip = musicClip;
        }
    }
}