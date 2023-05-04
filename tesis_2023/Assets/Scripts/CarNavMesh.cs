using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Player controller

public class CarNavMesh : MonoBehaviour
{
    [SerializeField] private Transform goal;

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
        agent.destination = goal.position;
    }

    private void GoToClick()
    {
        Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(movePosition, out var hitInfo))
        {
            agent.SetDestination(hitInfo.point);
        }
    }
}
