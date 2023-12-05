using System.Collections.Generic;
using UnityEngine;
using Managers;
using UnityEditor.AI;

namespace Arena
{
    public class ArenaData : MonoBehaviour
    {
        [Header("Opponents manager")]
        [SerializeField] private OpponentsManager opponentsManager;

        [Header("Waypoints")]
        [SerializeField] private List<GameObject> waypoints = new List<GameObject>();

        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip musicClip;

        private void Awake()
        {
            NavMeshBuilder.BuildNavMesh();
            opponentsManager.SetWaypoints(waypoints);
            musicSource.clip = musicClip;
        }
    }
}