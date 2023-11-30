using Entities;
using Entities.Opponent;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    public class OpponentsManager : MonoBehaviour
    {
        [Header("Opponents")]
        [SerializeField] private GameObject[] opponentPrefabs;
        [SerializeField] private int opponentsQuantity;

        [Header("Waypoints")]
        [SerializeField] private GameObject[] waypoints;

        [Header("Minimap")]
        [SerializeField] private Minimap minimap;

        private List<CarLifeBehaviour> opponentLifes;
        private List<OpponentAI> opponentIAs;
        private int carModelIndex = 0;
        
        public event System.Action OnOpponentsLose;

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
                //int randomOpponent = Random.Range(0, opponentPrefabs.Length);

                GameObject obj = Instantiate(opponentPrefabs[carModelIndex]);
                carModelIndex++;
                if (carModelIndex == opponentPrefabs.Length) carModelIndex = 0;

                Transform child = transform.GetChild(Random.Range(0, transform.childCount));
                obj.GetComponent<OpponentAI>().SetWaypoints(waypoints);

                obj.transform.position = child.position;

                opponentIAs.Add(obj.GetComponent<OpponentAI>());
                opponentLifes.Add(obj.GetComponent<CarLifeBehaviour>());

                minimap.InitOpponentMark(obj.transform);

                yield return new WaitForEndOfFrame();
                count++;
            }
            for (int i = 0; i < opponentLifes.Count; i++)
            {
                opponentLifes[i].OnZeroHealth += opponentIAs[i].DisableIA;
                opponentLifes[i].OnZeroHealth += CheckOpponentsAlive;
            }
        }

        private void CheckOpponentsAlive()
        {
            opponentsQuantity--;
            if (opponentsQuantity <= 0)
            {
                OnOpponentsLose?.Invoke();
            }
        }

    }
}