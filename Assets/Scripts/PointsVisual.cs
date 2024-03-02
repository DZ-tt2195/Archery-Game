using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Archery
{
    public class PointsVisual : MonoBehaviour
    {
        [SerializeField] TMP_Text textbox;

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        public void Setup(string text, Color color, Vector3 position)
        {
            this.transform.localPosition = position + new Vector3(0, 0, -1);
            textbox.text = text;
            textbox.color = color;
            StartCoroutine(ExpandContract());
        }

        IEnumerator ExpandContract()
        {
            Vector3 maxSize = new(2, 2, 2);
            float elapsedTime = 0f;
            float waitTime = 0.75f;

            while (elapsedTime < waitTime)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, maxSize, elapsedTime / waitTime);
                elapsedTime += Time.deltaTime;
                transform.SetAsFirstSibling();
                yield return null;
            }

            while (waitTime > 0)
            {
                transform.localScale = Vector3.Lerp(maxSize, Vector3.zero, elapsedTime / waitTime);
                waitTime -= Time.deltaTime;
                transform.SetAsFirstSibling();
                yield return null;
            }

            Destroy(this.gameObject);
        }
    }
}
