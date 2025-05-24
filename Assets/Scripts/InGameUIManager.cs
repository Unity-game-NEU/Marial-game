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
    // �������ƿ��Դ� GameManager ��ȡ���������������ñ�����
    // public string mainMenuSceneName = "MainMenuScene";
    // public string levelSelectSceneName = "LevelSelectScene";

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f; // ȷ����Ϸʱ��������������
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            if (livesText != null) livesText.text = "����: " + GameManager.Instance.lives;
            if (coinsText != null) coinsText.text = "���: " + GameManager.Instance.coins;
        }
        else
        {
            if (livesText != null) livesText.text = "����: N/A";
            if (coinsText != null) coinsText.text = "���: N/A";
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
            // ��ȷֹͣ��ǰ�������֣���Ϊ LoadLevel �����²�����
            AudioManager.Instance.StopSound("Background");
            Debug.Log("�ؿ���������ֹͣ 'Background' ���֣�׼�����¼��ء�");
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadLevel(GameManager.Instance.world, GameManager.Instance.stage);
        }
        else
        {
            Debug.LogError("GameManager instance not found. Cannot restart level.");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���ı��÷���
        }
    }

    public void ExitToLevelSelect()
    {
        Time.timeScale = 1f;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopSound("Background"); // ֹͣ��ǰ��Ϸ�ؿ�������
            Debug.Log("�˳����ؿ�ѡ����ֹͣ 'Background' ���֡�");
        }

        string sceneToLoad = "LevelSelectScene"; // Ĭ��ֵ
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.levelSelectSceneName))
        {
            sceneToLoad = GameManager.Instance.levelSelectSceneName;
        }
        else
        {
            Debug.LogWarning("LevelSelectSceneName δ�� GameManager �����ã���ʹ��Ĭ��ֵ 'LevelSelectScene'");
        }

        SceneManager.LoadScene(sceneToLoad);
        // �ؿ�ѡ���������ֽ������������Ƶ�������ڼ��غ󲥷�
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopSound("Background"); // ֹͣ��ǰ��Ϸ�ؿ�������
            Debug.Log("�˳������˵�����ֹͣ 'Background' ���֡�");
        }

        string sceneToLoad = "MainMenuScene"; // Ĭ��ֵ
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.mainMenuSceneName))
        {
            sceneToLoad = GameManager.Instance.mainMenuSceneName;
        }
        else
        {
            Debug.LogWarning("MainMenuSceneName δ�� GameManager �����ã���ʹ��Ĭ��ֵ 'MainMenuScene'");
        }
        SceneManager.LoadScene(sceneToLoad);
        // ���˵������ֽ������������Ƶ�������ڼ��غ󲥷�
    }
}