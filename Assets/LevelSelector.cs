using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private LevelConfig[] levelConfigs;
    [SerializeField] private LevelManager levelManager;

    public void LoadLevel(int index)
    {
        if (index < levelConfigs.Length)
        {
            LoadLevel(levelConfigs[index]);
        }
    }

    private void LoadLevel(LevelConfig levelConfig) {
        levelManager.SetLevelConfig(levelConfig);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the same scene with new config
    }

    public void LoadNextLevel() {
        Debug.Log("000");
        Debug.Log($"Loading next level: {levelManager.GetLevelConfig().nextLevel}");
        LoadLevel(levelManager.GetLevelConfig().nextLevel);
    }

    public void LoadFirstLevel() {
        LoadLevel(0);
    }
}
