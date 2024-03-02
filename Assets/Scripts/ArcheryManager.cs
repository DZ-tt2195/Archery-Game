using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Archery
{
    public class ArcheryManager : MonoBehaviour
    {
        List<Player> listOfPlayers = new();
        [SerializeField] Player playerPrefab;
        [SerializeField] List<Bullseye> possibleBullseyes = new();
        [SerializeField] TMP_Text roundNumberText;
        [SerializeField] GameObject reticle;
        [SerializeField] Button backToTitleScreen;
        [SerializeField] TMP_Text player1Text;
        [SerializeField] TMP_Text player2Text;

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

            Bullseye nextBullseye = Instantiate(possibleBullseyes[Random.Range(0, possibleBullseyes.Count)]);
            yield return new WaitForSeconds(Random.Range(0f, 2f));
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
                Countdown = 3;
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
        }
    }

}