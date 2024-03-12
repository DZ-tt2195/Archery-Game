using UnityEngine;

namespace Archery
{
    [RequireComponent(typeof(MeshRenderer))]
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

        private void OnEnable()
        {
            ArcheryManager.instance.arrowScore += this.CalculateScore;
        }

        private void OnDestroy()
        {
            ArcheryManager.instance.arrowScore -= this.CalculateScore;
        }

        /// <summary>
        /// remember who shot this arrow
        /// </summary>
        /// <param name="player"></param>
        public void AssignPlayer(Player player)
        {
            this.player = player;
            switch (this.player.playerNumber)
            {
                case 0: //if player 1, this arrow is blue
                    meshRenderer.material = bluePlayer;
                    break;
                case 1: //if player 2, this arow is red
                    meshRenderer.material = redPlayer;
                    break;
            }
        }

        private void Update()
        {
            if (moving) //if this is still moving, move the bullet up
                this.transform.Translate(bulletSpeed * Time.deltaTime * Vector3.up);
        }

        /// <summary>
        /// calculate score based on distance from the center of the bullseye
        /// </summary>
        void CalculateScore()
        {
            Vector3 currentPosition = transform.localPosition;
            currentPosition.z = 0; //ignore the z position in calculations
            int score = 8 - (int)(Vector3.Distance(currentPosition, Vector3.zero) / 0.6f); //closer to the bullseye is worth more points

            player.scoreTally[^1] += score; //set player's score to the calculated number
            PointsVisual newVisual = Instantiate(textVisualPrefab, this.transform); //create a text visual of the score
            newVisual.Setup($"+{score}", meshRenderer.material.color);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullseye")) //if this hits a bullseye, it stops moving and stays where it is
            {
                moving = false;
                this.transform.SetParent(other.gameObject.transform);
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, -0.65f);
            }
            else if (other.CompareTag("Obstacle")) //if this hits an obstacle, it's gone
            {
                Destroy(this.gameObject);
            }
        }
    }
}
