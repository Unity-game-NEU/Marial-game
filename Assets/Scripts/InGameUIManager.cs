// InGameUIManager.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI coinsText;

    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    // 场景名称可以从 GameManager 获取，或者在这里配置备用名
    // public string mainMenuSceneName = "MainMenuScene";
    // public string levelSelectSceneName = "LevelSelectScene";

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f; // 确保游戏时间是正常流动的
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            if (livesText != null) livesText.text = "生命: " + GameManager.Instance.lives;
            if (coinsText != null) coinsText.text = "金币: " + GameManager.Instance.coins;
        }
        else
        {
            if (livesText != null) livesText.text = "生命: N/A";
            if (coinsText != null) coinsText.text = "金币: N/A";
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        if (AudioManager.Instance != null)
        {
            // 明确停止当前背景音乐，因为 LoadLevel 会重新播放它
            AudioManager.Instance.StopSound("Background");
            Debug.Log("关卡重启：已停止 'Background' 音乐，准备重新加载。");
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadLevel(GameManager.Instance.world, GameManager.Instance.stage);
        }
        else
        {
            Debug.LogError("GameManager instance not found. Cannot restart level.");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 最后的备用方案
        }
    }

    public void ExitToLevelSelect()
    {
        Time.timeScale = 1f;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopSound("Background"); // 停止当前游戏关卡的音乐
            Debug.Log("退出到关卡选择：已停止 'Background' 音乐。");
        }

        string sceneToLoad = "LevelSelectScene"; // 默认值
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.levelSelectSceneName))
        {
            sceneToLoad = GameManager.Instance.levelSelectSceneName;
        }
        else
        {
            Debug.LogWarning("LevelSelectSceneName 未在 GameManager 中配置，将使用默认值 'LevelSelectScene'");
        }

        SceneManager.LoadScene(sceneToLoad);
        // 关卡选择界面的音乐将由其自身的音频控制器在加载后播放
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopSound("Background"); // 停止当前游戏关卡的音乐
            Debug.Log("退出到主菜单：已停止 'Background' 音乐。");
        }

        string sceneToLoad = "MainMenuScene"; // 默认值
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.mainMenuSceneName))
        {
            sceneToLoad = GameManager.Instance.mainMenuSceneName;
        }
        else
        {
            Debug.LogWarning("MainMenuSceneName 未在 GameManager 中配置，将使用默认值 'MainMenuScene'");
        }
        SceneManager.LoadScene(sceneToLoad);
        // 主菜单的音乐将由其自身的音频控制器在加载后播放
    }
}