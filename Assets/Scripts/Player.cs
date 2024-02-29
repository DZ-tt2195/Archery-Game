using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archery
{
    public class Player : MonoBehaviour
    {
        public bool availableArrow = false;
        public int playerNumber;
        public int score;
        [SerializeField] Arrow arrowPrefab;
        [SerializeField] KeyCode key;

        private void Update()
        {
            if (availableArrow && Input.GetKeyDown(key))
            {
                Arrow arrow = Instantiate(arrowPrefab);
                arrow.transform.position = new Vector3(0, 0, -5);
                arrow.player = this;
                availableArrow = false;
            }
        }
    }
}