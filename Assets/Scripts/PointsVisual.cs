using System.Collections;
using UnityEngine;
using TMPro;

namespace Archery
{
    public class PointsVisual : MonoBehaviour
    {
        TMP_Text textbox;

        private void Awake()
        {
            textbox = this.transform.GetChild(0).GetComponent<TMP_Text>();
            transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// a visual display of the arrow's score
        /// </summary>
        /// <param name="text">what the text says</param>
        /// <param name="color">color of the text</param>
        /// <param name="position">position to spawn this</param>
        public void Setup(string text, Color color)
        {
            this.transform.localEulerAngles = new Vector3(-90, 0, 0);
            this.transform.localPosition = new Vector3(0, -1.5f, 0);
            textbox.text = text;
            textbox.color = color;
            StartCoroutine(ExpandContract());
        }

        /// <summary>
        /// expand this visual then shrink it back down
        /// </summary>
        /// <returns></returns>
        IEnumerator ExpandContract()
        {
            Vector3 maxSize = new(2, 2, 2);
            float elapsedTime = 0f;
            float waitTime = 1f;

            while (elapsedTime < waitTime) //lerp this to max size
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, maxSize, elapsedTime / waitTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            while (waitTime > 0) //lerp this to 0
            {
                transform.localScale = Vector3.Lerp(maxSize, Vector3.zero, 1-waitTime);
                waitTime -= Time.deltaTime;
                yield return null;
            }

            Destroy(this.gameObject);
        }
    }
}
