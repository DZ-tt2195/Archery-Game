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
            int randomNumber = Random.Range(0, 10);
            for (int i = 0; i<randomNumber; i++)
            {
                GameObject newObstacle = Instantiate(obstaclePrefab, this.transform);
                float randomX = Random.Range(1f, 3.5f) * (Random.Range(0, 2) * 2 - 1);
                float randomY = Random.Range(1f, 3.5f) * (Random.Range(0, 2) * 2 - 1);
                newObstacle.transform.localPosition = new Vector3(randomX, randomY, -1f);
            }
        }

        public IEnumerator Travelling()
        {
            for (int i = 0; i < tg.position.Count; i++)
            {
                yield return MoveBullseye(tg.position[i], tg.time[i] * Random.Range(0.75f, 1.25f));
            }
        }

        public IEnumerator Finished()
        {
            yield return MoveBullseye(Vector3.zero, 0.25f);
        }

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