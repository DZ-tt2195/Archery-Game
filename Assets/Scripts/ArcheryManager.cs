using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Archery
{
    public class ArcheryManager : MonoBehaviour
    {
        List<Player> listOfPlayers = new();
        [SerializeField] Player playerPrefab;
        [SerializeField] List<Bullseye> possibleBullseyes = new();
        [SerializeField] TMP_Text roundNumberText;
        [SerializeField] GameObject reticle;

        private int _round;
        int Round {get {return _round;} set { _round = value; roundNumberText.text = $"ROUND {value}";}}
        private float _countdown;
        float Countdown { get { return _countdown; } set { _countdown = value;  roundNumberText.text = $"Next Round in {value:F1}"; } }

        void Start()
        {
            Application.targetFrameRate = 60;

            for (int i = 0; i < 1; i++)
            {
                Player nextPlayer = Instantiate(playerPrefab);
                nextPlayer.playerNumber = i;
                listOfPlayers.Add(nextPlayer);
            }
            StartCoroutine(NewRound());
        }

        IEnumerator NewRound()
        {
            Round++;
            reticle.SetActive(true);
            foreach (Player player in listOfPlayers)
                player.availableArrow = true;

            Bullseye nextBullseye = Instantiate(possibleBullseyes[Random.Range(0, possibleBullseyes.Count)]);
            yield return new WaitForSeconds(Random.Range(0f, 3f));
            yield return nextBullseye.Travelling();

            reticle.SetActive(false);
            foreach (Player player in listOfPlayers)
                player.availableArrow = false;

            yield return nextBullseye.Finished();
            var listOfArrows = FindObjectsOfType<Arrow>();
            foreach (Arrow arrow in listOfArrows)
                arrow.CalculateScore();

            Countdown = 4;
            while (Countdown > 0)
            {
                Countdown -= Time.deltaTime;
                yield return null;
            }

            Destroy(nextBullseye.gameObject);
            StartCoroutine(NewRound());
        }
    }
}