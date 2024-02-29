using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullseye : MonoBehaviour
{
    [SerializeField] List<TravelGuide> travelPoints;

    [Serializable]
    public struct TravelGuide
    {
        public Vector3 position;
        public float time;
    }

    public IEnumerator Travelling()
    {
        foreach (TravelGuide tg in travelPoints)
        {
            yield return MoveBullseye(tg.position, tg.time*UnityEngine.Random.Range(0.75f, 1.25f));
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
