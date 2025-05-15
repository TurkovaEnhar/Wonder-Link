using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utility
{
    public class SceneManagement : MonoBehaviour
    {
        public static SceneManagement Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        
            DontDestroyOnLoad(this);
        }

  
        public void LoadGameSceneAsync()
        {
            SceneManager.LoadSceneAsync("SampleScene");
        }

   
        public void LoadMainMenuAsync()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
