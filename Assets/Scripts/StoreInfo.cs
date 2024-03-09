using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archery
{
    public class StoreInfo : MonoBehaviour
    {
        public static StoreInfo instance;
        public int numPlayers = 1;
        public enum GameMode { HighScore, Endless };
        public GameMode selectedMode;

        private void Awake() //this remembers the game's settings chosen on the title screen
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public static void NextScene(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}