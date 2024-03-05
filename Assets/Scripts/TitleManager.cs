using UnityEngine;
using UnityEngine.UI;

namespace Archery
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] Button highScoreMode;
        [SerializeField] Button endlessMode;
        [SerializeField] Slider playerSlider;

        private void Start()
        {
            Application.targetFrameRate = 60;
            highScoreMode.onClick.AddListener(() => PlayGame(StoreInfo.GameMode.HighScore));
            endlessMode.onClick.AddListener(() => PlayGame(StoreInfo.GameMode.Endless));
        }

        void PlayGame(StoreInfo.GameMode gamemode)
        {
            StoreInfo.instance.selectedMode = gamemode;
            StoreInfo.instance.numPlayers = (int)playerSlider.value;
            StoreInfo.instance.NextScene(1);
        }
    }
}