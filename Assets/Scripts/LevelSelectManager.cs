// LevelSelectManager.cs
using UnityEngine;
// using UnityEngine.SceneManagement; // �����֮ǰ���õ�

public class LevelSelectManager : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenuScene";

    public void SelectLevel1_1()
    {
        if (GameManager.Instance != null)
        {
            // ʹ���µĹ������������� lives �� coins
            // GameManager.Instance.SetLives(3);
            // GameManager.Instance.SetCoins(0);
            // ���ߵ����ۺϷ�����
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