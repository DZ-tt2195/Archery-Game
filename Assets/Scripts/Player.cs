using System.Collections.Generic;
using UnityEngine;

namespace Archery
{
    public class Player : MonoBehaviour
    {
        public bool availableArrow = false;
        public int playerNumber;
        public List<int> scoreTally = new();
        [SerializeField] Arrow arrowPrefab;
        public KeyCode key;

        private void Update()
        {
            if (availableArrow && Input.GetKeyDown(key)) //if this has an arrow, fire it
            {
                Arrow arrow = Instantiate(arrowPrefab);
                arrow.transform.position = new Vector3(0, 0, -5);
                arrow.AssignPlayer(this);
                availableArrow = false;
            }
        }
    }
}