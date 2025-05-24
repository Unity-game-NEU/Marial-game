// LevelSelectManager.cs
using UnityEngine;
// using UnityEngine.SceneManagement; // 如果你之前有用到

public class LevelSelectManager : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenuScene";

    public void SelectLevel1_1()
    {
        if (GameManager.Instance != null)
        {
            // 使用新的公共方法来设置 lives 和 coins
            // GameManager.Instance.SetLives(3);
            // GameManager.Instance.SetCoins(0);
            // 或者调用综合方法：
            GameManager.Instance.ResetPlayerStatsForLevelSelect();

            GameManager.Instance.LoadLevel(1, 1);
        }
        else
        {
            Debug.LogError("GameManager.Instance is not found!");
        }
    }

    public void SelectLevel1_2()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetPlayerStatsForLevelSelect();
            GameManager.Instance.LoadLevel(1, 2);
        }
        else
        {
            Debug.LogError("GameManager.Instance is not found!");
        }
    }

    public void SelectLevel2_1()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetPlayerStatsForLevelSelect();
            GameManager.Instance.LoadLevel(2, 1);
        }
        else
        {
            Debug.LogError("GameManager.Instance is not found!");
        }
    }

    public void SelectLevel2_2()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetPlayerStatsForLevelSelect();
            GameManager.Instance.LoadLevel(2, 2);
        }
        else
        {
            Debug.LogError("GameManager.Instance is not found!");
        }
    }

    public void BackToMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("MainMenuSceneName is not set in LevelSelectManager!");
        }
    }
}