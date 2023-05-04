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
        [SerializeField] private CarController car;

        [Header("Goals")]
        [SerializeField] private Transform[] goals;

        private void Start()
        {
            CarController carController = Instantiate(car);
            carController.SetGoals(goals);

            //NavMeshBuilder.BuildNavMesh(); --> Esto es para cuando intanciemos obstaculos de forma random, se genere el navmesh en tiempo de ejecucion.
        }
    }
}