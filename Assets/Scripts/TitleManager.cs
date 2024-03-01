using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Archery
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] Button highScoreMode;
        [SerializeField] Button endlessMode;

        private void Start()
        {
            Application.targetFrameRate = 60;
            highScoreMode.onClick.AddListener(() => PlayGame(StoreInfo.GameMode.HighScore));
            endlessMode.onClick.AddListener(() => PlayGame(StoreInfo.GameMode.Endless));
        }

        void PlayGame(StoreInfo.GameMode gamemode)
        {
            StoreInfo.instance.selectedMode = gamemode;
            StoreInfo.instance.NextScene(1);
        }
    }
}