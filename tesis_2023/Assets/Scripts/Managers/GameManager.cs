using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AI;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Car")]
        [SerializeField] private CarNavMesh car;

        [Header("Goals")]
        [SerializeField] private Transform[] goals;

        private void Start()
        {
            CarNavMesh carController = Instantiate(car);
            carController.SetGoals(goals);

            //NavMeshBuilder.BuildNavMesh(); --> Esto es para cuando intanciemos obstaculos de forma random, se genere el navmesh en tiempo de ejecucion.
        }
    }
}