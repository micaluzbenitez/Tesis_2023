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
        [SerializeField] private OpponentsManager opponentsManager;
        private void Start()
        {
            playerLife.OnTakeDamage += uiManager.SetHealthBar;
            playerLife.OnSpeedChange += uiManager.SetSpeedText;
            carController.OnCarFlip += uiManager.EnableRespawnText;
            carController.OnCarStraighten += uiManager.DisableRespawnText;
            playerLife.OnIncreaseScore += uiManager.SetGameplayScoreText;
            playerLife.OnZeroHealth += DisabeCar;
            playerLife.OnZeroHealth += carController.DisableCarController;
            opponentsManager.OnOpponentsLose += uiManager.EnableVictoryPanel;
            opponentsManager.OnOpponentsLose += uiManager.DisableGameplayUI;
            opponentsManager.OnOpponentsLose += playerLife.Win;
            playerLife.OnWin += uiManager.SetVictoryScoreText;
            carController.OnTurboChange += uiManager.SetTurboSlider;
            playerLife.OnZeroHealth += uiManager.EnableDefeatPanel;

            //NavMeshBuilder.BuildNavMesh(); --> Esto es para cuando intanciemos obstaculos de forma random, se genere el navmesh en tiempo de ejecucion.
        }
        private void OnDestroy()
        {
            opponentsManager.OnOpponentsLose -= uiManager.EnableVictoryPanel;
            opponentsManager.OnOpponentsLose -= playerLife.Win;
            opponentsManager.OnOpponentsLose -= uiManager.DisableGameplayUI;
            playerLife.OnWin -= uiManager.SetVictoryScoreText;
            playerLife.OnZeroHealth -= uiManager.EnableDefeatPanel;
        }
        private void DisabeCar()
        {
            playerLife.OnTakeDamage -= uiManager.SetHealthBar;
            playerLife.OnSpeedChange -= uiManager.SetSpeedText;
            carController.OnCarFlip -= uiManager.EnableRespawnText;
            carController.OnCarStraighten -= uiManager.DisableRespawnText;
            playerLife.OnIncreaseScore -= uiManager.SetGameplayScoreText;
            playerLife.OnZeroHealth -= DisabeCar;
            playerLife.OnZeroHealth -= carController.DisableCarController;
            carController.OnTurboChange -= uiManager.SetTurboSlider;

        }
    }
}