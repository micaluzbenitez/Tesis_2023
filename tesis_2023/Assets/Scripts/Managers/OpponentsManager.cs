using Entities.Opponent;
using System.Collections;
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

        private void Start()
        {
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

                yield return new WaitForEndOfFrame();
                count++;
            }
        }
    }
}