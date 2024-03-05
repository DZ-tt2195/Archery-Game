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

        private void Awake()
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

        public void NextScene(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}