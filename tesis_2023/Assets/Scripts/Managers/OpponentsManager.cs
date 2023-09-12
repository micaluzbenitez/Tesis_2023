using Entities;
using Entities.Opponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waypoints;

namespace Managers
{
    public class OpponentsManager : MonoBehaviour
    {
        [Header("Opponents")]
        [SerializeField] private GameObject[] opponentPrefabs;
        [SerializeField] private int opponentsQuantity;

        [Header("Waypoints")]
        [SerializeField] private GameObject[] waypoints;
        private List<CarLifeBehaviour> opponentLifes;
        private List<OpponentAI> opponentIAs;



        private void Start()
        {
            opponentLifes = new List<CarLifeBehaviour>();
            opponentIAs = new List<OpponentAI>();
            StartCoroutine(Spawn());


        }

        private IEnumerator Spawn()
        {
            int count = 0;

            while (count < opponentsQuantity)
            {
                int randomOpponent = Random.Range(0, opponentPrefabs.Length);
                GameObject obj = Instantiate(opponentPrefabs[randomOpponent]);


                Transform child = transform.GetChild(Random.Range(0, transform.childCount));
                obj.GetComponent<OpponentAI>().SetWaypoints(waypoints);

                obj.transform.position = child.position;

                opponentIAs.Add(obj.GetComponent<OpponentAI>());
                opponentLifes.Add(obj.GetComponent<CarLifeBehaviour>());
                yield return new WaitForEndOfFrame();
                count++;
            }
            for (int i = 0; i < opponentLifes.Count; i++)
            {
                opponentLifes[i].OnZeroHealth += opponentIAs[i].DisableIA;
            }
        }
    
    }
}