using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Archery
{
    /// <summary>
    /// how a bullseye moves
    /// </summary>
    [Serializable]
    public struct TravelGuide
    {
        public List<Vector3> position; //a list of positions this moves to
        public List<float> time; //how long it takes to move to the next position
    }

    public class ArcheryManager : MonoBehaviour
    {
        public static ArcheryManager instance;
        [Header("Players")]
        List<Player> listOfPlayers = new();
        [SerializeField] Player playerPrefab;

        [Header("Bullseyes")]
        [SerializeField] Bullseye bullseyePrefab;
        [SerializeField] List<TravelGuide> listOfBullseyeVariants = new();
        public event Action arrowScore;

        [Header("UI")]
        [SerializeField] GameObject reticle;
        [SerializeField] TMP_Text topText;
        [SerializeField] List<TMP_Text> playerTexts;
        [SerializeField] Button backToTitleScreen;

        [Header("Scoreboard")]
        [SerializeField] GameObject scoreboardObject;
        [SerializeField] TMP_Text scoreTextPrefab;
        [SerializeField] RectTransform scoreboardCollection;

        private int _round; //when the round number increases, automatically change the text on the top of the screen
        int Round { get { return _round; } set { _round = value; topText.text = $"ROUND {value}"; } }
        private float _countdown; //when counting down before the nex tround, automatically change the text on the top of the screen
        float Countdown { get { return _countdown; } set { _countdown = value; topText.text = $"Next Round in {value:F1}"; } }

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            if (StoreInfo.instance == null) //for unity editor purposes, warp to title screen if storeinfo doesn't exist
                StoreInfo.NextScene(0);

            for (int i = 0; i < StoreInfo.instance.numPlayers; i++) //generate players
            {
                Player nextPlayer = Instantiate(playerPrefab);
                nextPlayer.playerNumber = i;
                nextPlayer.key = (i == 0) ? KeyCode.LeftShift : KeyCode.RightShift;
                listOfPlayers.Add(nextPlayer);
                playerTexts[i].gameObject.SetActive(true);
            }

            backToTitleScreen.gameObject.SetActive(false);
            scoreboardObject.SetActive(false);
            StartCoroutine(NewRound());
        }

        /// <summary>
        /// generate a bullseye, have it do its thing
        /// </summary>
        /// <returns></returns>
        IEnumerator NewRound()
        {
            Round++; //increase the round number
            reticle.SetActive(true);

            foreach (Player player in listOfPlayers)
            {
                player.availableArrow = true;//give each player an arrow
                player.scoreTally.Add(0);//add 0 to their tally
            }

            Bullseye nextBullseye = Instantiate(bullseyePrefab); //generate a bullseye and give it a random travel guide
            int randomNumber = UnityEngine.Random.Range(0, listOfBullseyeVariants.Count);
            nextBullseye.tg = listOfBullseyeVariants[randomNumber];

            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f)); //a dramatic wait
            StartCoroutine(nextBullseye.Travelling()); //move the bullseye
        }

        /// <summary>
        /// the round has finished
        /// </summary>
        /// <returns></returns>
        public IEnumerator RoundOver(Bullseye bullseye)
        {             
            reticle.SetActive(false); 
            foreach (Player player in listOfPlayers)
                player.availableArrow = false; //arrows can no longer be shot

            arrowScore?.Invoke(); //calculate arrows, if there are any

            if (StoreInfo.instance.selectedMode == StoreInfo.GameMode.HighScore &&
                Round == 10) //if it's round 10 in high score mode
            {
                GameOver("Finished!");
            }
            else if (StoreInfo.instance.selectedMode == StoreInfo.GameMode.Endless &&
                CheckForMisses()) //if any arrows missed in endless mode
            {
                GameOver("Missed!");
            }
            else
            {
                Countdown = 2.5f; //wait 2.5 seconds 
                while (Countdown > 0)
                {
                    Countdown -= Time.deltaTime;
                    yield return null;
                }

                Destroy(bullseye.gameObject); //destroy the bullseye and begin a new round
                StartCoroutine(NewRound());
            }
        }

        /// <summary>
        /// ending the game
        /// </summary>
        /// <param name="endMessage">the message to display on top</param>
        void GameOver(string endMessage)
        {
            topText.text = endMessage;
            backToTitleScreen.onClick.AddListener(() => StoreInfo.NextScene(0));
            backToTitleScreen.gameObject.SetActive(true); //display button to return to title screen

            scoreboardObject.SetActive(true); //display the scoreboard
            TMP_Text sideText = GameObject.Find("Player Text").GetComponent<TMP_Text>(); //first text is for players
            sideText.text = $"Player";
            for (int i = 0; i < listOfPlayers.Count; i++)
                sideText.text += $"\n\n{i + 1}";

            for (int i = 0; i<Round; i++) //for each round played
            {
                string textContent = $"R{i + 1}"; //the round number
                foreach (Player player in listOfPlayers)
                    textContent += $"\n\n{player.scoreTally[i]}"; //get their score they got that round
                AddText(textContent); //add it to the scoreboard
            }

            string totalScores = "Total"; //one final text for total scores
            foreach (Player player in listOfPlayers)
            {
                int playerScore = 0;
                foreach (int tally in player.scoreTally)
                    playerScore += tally;
                totalScores += $"\n\n{playerScore}";
            }
            AddText(totalScores); //add it to the scoreboard
        }

        /// <summary>
        /// check if any arrows missed
        /// </summary>
        /// <returns>return true if a score is 0</returns>
        bool CheckForMisses()
        {
            foreach (Player player in listOfPlayers)
            {
                if (player.scoreTally[^1] == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// adding text to the scoreboard
        /// </summary>
        /// <param name="textContent"></param>
        void AddText(string textContent)
        {
            TMP_Text nextText = Instantiate(scoreTextPrefab, scoreboardCollection);
            nextText.text = textContent;

            if (scoreboardCollection.childCount >= 11) //if the text goes off the screen
            {
                //increase the size of the scoreboard so that you can scroll through it
                scoreboardCollection.sizeDelta = new Vector2(scoreboardCollection.sizeDelta.x + 150, scoreboardCollection.sizeDelta.y);
            }
        }
    }

}