// MainMenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 如果你使用了 UI.Slider 等

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject optionsPanel; // 你的选项面板

    [Header("Audio Settings")]
    public AudioSource backgroundMusicAudioSource; // 你的背景音乐 AudioSource
    public Slider volumeSlider; // 你的音量滑块

    // 新增：关卡选择场景的名称
    [Header("Scene Navigation")]
    public string levelSelectSceneName = "LevelSelectScene"; // 给你的关卡选择场景起个名字

    private const string MasterVolumeKey = "MasterVolume";
    private float currentVolume = 0.5f;

    void Start()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        currentVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);

        if (backgroundMusicAudioSource != null)
        {
            backgroundMusicAudioSource.volume = currentVolume;
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = currentVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // 修改 StartGame 方法
    public void StartGame()
    {
        Debug.Log("开始游戏按钮被点击！准备加载关卡选择界面: " + levelSelectSceneName);
        if (!string.IsNullOrEmpty(levelSelectSceneName))
        {
            SceneManager.LoadScene(levelSelectSceneName);
        }
        else
        {
            Debug.LogError("关卡选择场景名称 (levelSelectSceneName) 未在 MainMenuManager 中设置!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("退出游戏按钮被点击！");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void CloseOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        PlayerPrefs.SetFloat(MasterVolumeKey, currentVolume);
        PlayerPrefs.Save();
        Debug.Log("音量已保存: " + currentVolume);
    }

    public void SetVolume(float volume)
    {
        if (backgroundMusicAudioSource != null)
        {
            backgroundMusicAudioSource.volume = volume;
        }
        currentVolume = volume;
    }
}