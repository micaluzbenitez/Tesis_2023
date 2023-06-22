using Entities;
using Entities.Player;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CarController carController;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private CarLifeBehaviour playerLife;
        private void Start()
        {
            playerLife.OnTakeDamage += uiManager.SetHealthBar;
            playerLife.OnSpeedChange += uiManager.SetSpeedText;

            //NavMeshBuilder.BuildNavMesh(); --> Esto es para cuando intanciemos obstaculos de forma random, se genere el navmesh en tiempo de ejecucion.
        }
        private void OnDestroy()
        {
            playerLife.OnTakeDamage -= uiManager.SetHealthBar;
            playerLife.OnSpeedChange -= uiManager.SetSpeedText;
        }
    }
}