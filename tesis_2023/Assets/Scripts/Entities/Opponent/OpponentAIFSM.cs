using System;
using UnityEngine;

namespace Entities.Opponent
{ 
    public class OpponentAIFSM : MonoBehaviour
    {
        public delegate void GhostDestroyAction(OpponentAIFSM ghost);
        public GhostDestroyAction OnGhostDestroy;

        public float lifeLostPerShot = 10;

        [Serializable] 
        public class GhostData
        {
            public float size;
            public float life;
        }

        private Vector3 size;
        private float ghostLife;

        public enum GhostState
        {
            Idle,
            GoingToTarget,
            GoAway,
            Last
        }

        [Header("Ghost states data")]
        [SerializeField] private GhostState state;
        public float speed = 8;
        public float targetDistance = 30;
        public float distanceToStop = 1;
        public float distanceToRestart = 5;
        public float timeStopped = 2;
        private GameObject target;
        private float time; 

        public void InitGhost(GhostData ghostData)
        {
            size = Vector3.one * ghostData.size;
            ghostLife = ghostData.life;
        }

        private void Update()
        {
            transform.localScale = size;

            time += Time.deltaTime;
            switch(state)
            {
                case GhostState.Idle:
                    if (time > timeStopped)
                    {
                        if (Vector3.Distance(target.transform.position, transform.position) < targetDistance)   // Si el player se encuentra cerca del fantasma, este va a atacarlo
                            NextState();
                    }                        
                    break;
                case GhostState.GoingToTarget:
                    Vector3 dir = target.transform.position - transform.position;
                    transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

                    if (Vector3.Distance(target.transform.position, transform.position) > targetDistance)   // Si el player se escapa, el fantasme vuelve a su comportamiento erratico
                        state = GhostState.Idle;

                    if (Vector3.Distance(transform.position, target.transform.position) < distanceToStop)
                    {
                        //GameManager.Get().LifePointsLost(GameManager.Get().ghostPointsLife);
                        NextState();
                    }
                    break;
                case GhostState.GoAway:
                    Vector3 dir02 = transform.position - target.transform.position;
                    transform.Translate(dir02.normalized * speed * Time.deltaTime, Space.World);

                    if (Vector3.Distance(transform.position, target.transform.position) > distanceToRestart)
                        NextState();
                    break;
            }
        }

        private void NextState()
        {
            time = 0;
            int intState = (int)state;
            intState++;
            intState = intState % ((int)GhostState.Last);
            SetState((GhostState)intState);
        }

        private void SetState(GhostState ghostState)
        {
            state = ghostState;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Ball")
            {
                ghostLife -= lifeLostPerShot;

                if (ghostLife <= 0)
                {
                    if (OnGhostDestroy != null)
                        OnGhostDestroy(this);
                }
            }
        }
    }
}