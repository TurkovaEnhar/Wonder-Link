using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelDatabaseSO database;
    public int currentLevelIndex = 0;
    public LevelDataSO CurrentLevel => database.GetLevel(currentLevelIndex);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= database.GetLevelCount())
        {
            Debug.Log("All levels completed!");
            return;
        }

        LoadLevel(currentLevelIndex);
    }
}