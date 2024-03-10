using System.Collections;
using UnityEngine;

namespace Archery
{
    public class Bullseye : MonoBehaviour
    {
        public TravelGuide tg;
        [SerializeField] GameObject obstaclePrefab;

        private void Start()
        {
            //generate a random number of obstacle around the bullseye
            int randomNumber = Random.Range(5, 10);
            for (int i = 0; i<randomNumber; i++)
            {
                GameObject newObstacle = Instantiate(obstaclePrefab, this.transform);
                float randomX = Random.Range(-3.5f, 3.5f);
                float randomY = Random.Range(-3.5f, 3.5f);
                newObstacle.transform.localPosition = new Vector3(randomX, randomY, -1);
            }
        }

        /// <summary>
        /// move to every position in the travel guide
        /// </summary>
        /// <returns></returns>
        public IEnumerator Travelling()
        {
            for (int i = 0; i < tg.position.Count; i++)
            {
                //move to next position, and the time varies
                yield return MoveBullseye(tg.position[i], tg.time[i] * Random.Range(0.75f, 1.25f));
            }
            StartCoroutine(ArcheryManager.instance.RoundOver(this));
            StartCoroutine(MoveBullseye(Vector3.zero, 0.25f)); //move back to the center of the screen
        }

        /// <summary>
        /// lerp the bullseye to the next position
        /// </summary>
        /// <param name="newPosition">the position to move to</param>
        /// <param name="waitTime">time it takes</param>
        /// <returns></returns>
        IEnumerator MoveBullseye(Vector3 newPosition, float waitTime)
        {
            float elapsedTime = 0;
            Vector3 originalPos = transform.localPosition;

            while (elapsedTime < waitTime)
            {
                transform.localPosition = Vector3.Lerp(originalPos, newPosition, elapsedTime / waitTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = newPosition;
        }
    }
}