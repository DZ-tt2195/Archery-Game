using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Archery
{
    [Serializable]
    public struct TravelGuide
    {
        public List<Vector3> position;
        public List<float> time;
    }

    public class ArcheryManager : MonoBehaviour
    {
        List<Player> listOfPlayers = new();
        [SerializeField] List<TravelGuide> listOfBullseyeVariants = new();
        [SerializeField] Player playerPrefab;
        [SerializeField] Bullseye bullseyePrefab;
        [SerializeField] TMP_Text roundNumberText;
        [SerializeField] GameObject reticle;
        [SerializeField] Button backToTitleScreen;
        [SerializeField] TMP_Text player1Text;
        [SerializeField] TMP_Text player2Text;
        [SerializeField] GameObject scoreboardObject;
        [SerializeField] TMP_Text scoreTextPrefab;
        [SerializeField] RectTransform scoreboardCollection;

        private int _round;
        int Round {get {return _round;} set { _round = value; roundNumberText.text = $"ROUND {value}";}}
        private float _countdown;
        float Countdown { get { return _countdown; } set { _countdown = value;  roundNumberText.text = $"Next Round in {value:F1}"; } }

        void Start()
        {
            for (int i = 0; i < StoreInfo.instance.numPlayers; i++)
            {
                Player nextPlayer = Instantiate(playerPrefab);
                nextPlayer.playerNumber = i;
                nextPlayer.key = (i == 0) ? KeyCode.LeftShift : KeyCode.RightShift;
                listOfPlayers.Add(nextPlayer);
            }
            if (listOfPlayers.Count == 1)
                player2Text.gameObject.SetActive(false);

            backToTitleScreen.gameObject.SetActive(false);
            scoreboardObject.SetActive(false);
            StartCoroutine(NewRound());
        }

        IEnumerator NewRound()
        {
            Round++;
            reticle.SetActive(true);
            foreach (Player player in listOfPlayers)
            {
                player.availableArrow = true;
                player.scoreTally.Add(0);
            }

            Bullseye nextBullseye = Instantiate(bullseyePrefab);
            int randomNumber = UnityEngine.Random.Range(0, listOfBullseyeVariants.Count);
            nextBullseye.tg = listOfBullseyeVariants[randomNumber];
            Debug.Log(randomNumber);

            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
            yield return nextBullseye.Travelling();

            reticle.SetActive(false);
            foreach (Player player in listOfPlayers)
                player.availableArrow = false;

            yield return nextBullseye.Finished();
            var listOfArrows = FindObjectsOfType<Arrow>();
            foreach (Arrow arrow in listOfArrows)
                arrow.CalculateScore();

            if (StoreInfo.instance.selectedMode == StoreInfo.GameMode.HighScore &&
                Round == 10)
            {
                roundNumberText.text = "The end.";
                GameOver();
            }
            else if (StoreInfo.instance.selectedMode == StoreInfo.GameMode.Endless &&
                listOfArrows.Length < listOfPlayers.Count)
            {
                roundNumberText.text = "You missed an arrow.";
                GameOver();
            }
            else
            {
                Countdown = 2.5f;
                while (Countdown > 0)
                {
                    Countdown -= Time.deltaTime;
                    yield return null;
                }

                Destroy(nextBullseye.gameObject);
                StartCoroutine(NewRound());
            }
        }

        void GameOver()
        {
            backToTitleScreen.onClick.AddListener(() => StoreInfo.instance.NextScene(0));
            backToTitleScreen.gameObject.SetActive(true);

            scoreboardObject.SetActive(true);
            TMP_Text sideText = GameObject.Find("Player Text").GetComponent<TMP_Text>();
            sideText.text = $"Player";
            for (int i = 0; i < listOfPlayers.Count; i++)
                sideText.text += $"\n\n{i + 1}";

            for (int i = 0; i<Round; i++)
            {
                string textContent = $"R{i + 1}";
                foreach (Player player in listOfPlayers)
                    textContent += $"\n\n{player.scoreTally[i]}";
                AddText(textContent);
            }

            string totalScores = "Total";
            foreach (Player player in listOfPlayers)
            {
                int playerScore = 0;
                foreach (int tally in player.scoreTally)
                    playerScore += tally;
                totalScores += $"\n\n{playerScore}";
            }
            AddText(totalScores);
        }

        void AddText(string textContent)
        {
            TMP_Text nextText = Instantiate(scoreTextPrefab, scoreboardCollection);
            nextText.text = textContent;

            if (scoreboardCollection.childCount >= 11)
            {
                scoreboardCollection.sizeDelta = new Vector2(scoreboardCollection.sizeDelta.x + 150, scoreboardCollection.sizeDelta.y);
            }
        }
    }

}