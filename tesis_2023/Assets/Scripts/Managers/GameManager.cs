using Entities;
using Entities.Player;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CarController carController;
        private CarLifeBehaviour carLifeBehaviour;
        private void Start()
        {
            carLifeBehaviour = carController.gameObject.GetComponent<CarLifeBehaviour>();
            carLifeBehaviour.OnPlayerTakeDamage += carController.RecieveCurrentHealth;
            //NavMeshBuilder.BuildNavMesh(); --> Esto es para cuando intanciemos obstaculos de forma random, se genere el navmesh en tiempo de ejecucion.
        }
    }
}