// MainMenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // �����ʹ���� UI.Slider ��

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject optionsPanel; // ���ѡ�����

    [Header("Audio Settings")]
    public AudioSource backgroundMusicAudioSource; // ��ı������� AudioSource
    public Slider volumeSlider; // �����������

    // �������ؿ�ѡ�񳡾�������
    [Header("Scene Navigation")]
    public string levelSelectSceneName = "LevelSelectScene"; // ����Ĺؿ�ѡ�񳡾��������

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

    // �޸� StartGame ����
    public void StartGame()
    {
        Debug.Log("��ʼ��Ϸ��ť�������׼�����عؿ�ѡ�����: " + levelSelectSceneName);
        if (!string.IsNullOrEmpty(levelSelectSceneName))
        {
            SceneManager.LoadScene(levelSelectSceneName);
        }
        else
        {
            Debug.LogError("�ؿ�ѡ�񳡾����� (levelSelectSceneName) δ�� MainMenuManager ������!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("�˳���Ϸ��ť�������");
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
        Debug.Log("�����ѱ���: " + currentVolume);
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