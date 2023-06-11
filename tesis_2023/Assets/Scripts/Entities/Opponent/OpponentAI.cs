using UnityEngine;
using Toolbox;

namespace Entities.Opponent
{ 
    public class OpponentAI : MonoBehaviour
    {
        [SerializeField] private float goAwayDuration = 1f;

        private OpponentNavMesh opponentNavMesh;
        private Timer goAwayTimer = new Timer();

        private void Awake()
        {
            opponentNavMesh = GetComponentInParent<OpponentNavMesh>();
            goAwayTimer.SetTimer(goAwayDuration, Timer.TIMER_MODE.DECREASE);
        }

        private void Update()
        {
            // Time until attack a new car
            if (goAwayTimer.Active) goAwayTimer.UpdateTimer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (goAwayTimer.Active) return;

            if (other.CompareTag("Car"))
            {
                opponentNavMesh.SetDestination(other.gameObject.transform.position);
            }
        }

        public void ResetState()
        {
            goAwayTimer.ActiveTimer();
        }
    }
}