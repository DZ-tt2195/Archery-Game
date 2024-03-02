using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Archery
{
    public class Arrow : MonoBehaviour
    {
        bool moving = true;
        public Player player;
        [SerializeField] float bulletSpeed;

        [SerializeField] Material bluePlayer;
        [SerializeField] Material redPlayer;
        [SerializeField] PointsVisual textVisualPrefab;
        MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void AssignPlayer(Player player)
        {
            this.player = player;
            switch (this.player.playerNumber)
            {
                case 0:
                    meshRenderer.material = bluePlayer;
                    break;
                case 1:
                    meshRenderer.material = redPlayer;
                    break;
            }
        }

        private void Update()
        {
            if (moving)
            {
                this.transform.Translate(bulletSpeed * Time.deltaTime * Vector3.up);
            }
        }

        public void CalculateScore()
        {
            Vector3 currentPosition = transform.localPosition;
            currentPosition.z = 0;
            int score = 8 - (int)(Vector3.Distance(currentPosition, Vector3.zero) / 0.6f);

            player.scoreTally[^1] += score;
            PointsVisual newVisual = Instantiate(textVisualPrefab);
            newVisual.Setup($"+{score}", meshRenderer.material.color, this.transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullseye"))
            {
                moving = false;
                this.transform.SetParent(other.gameObject.transform);
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, -0.65f);
            }
            else if (other.CompareTag("Obstacle"))
            {
                Debug.Log("hit obstacle");
                Destroy(this.gameObject);
            }
        }
    }
}
