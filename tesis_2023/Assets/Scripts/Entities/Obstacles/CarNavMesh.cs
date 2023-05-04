using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Obstacles
{
    public class CarNavMesh : MonoBehaviour
    {
        private Transform[] goals;
        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0)) GoToGoal();
            if (Input.GetMouseButton(1)) GoToClick();
        }

        private void GoToGoal()
        {
            agent.destination = goals[0].position;
        }

        private void GoToClick()
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(movePosition, out var hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }

        public void SetGoals(Transform[] goals)
        {
            this.goals = goals;
        }
    }
}