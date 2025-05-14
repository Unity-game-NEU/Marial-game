// LevelSelectManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Scene Navigation")]
    public string mainMenuSceneName = "MainMenuScene"; // ������˵������ļ���

    // �������������ڱ��ؿ���ť����
    // ���� 'levelSceneName' ������Ҫ���ص�ʵ����Ϸ�ؿ��ĳ����ļ���
    public void LoadLevel(string levelSceneName)
    {
        if (!string.IsNullOrEmpty(levelSceneName))
        {
            Debug.Log("���عؿ�: " + levelSceneName);
            SceneManager.LoadScene(levelSceneName);
        }
        else
        {
            Debug.LogError("Ҫ���صĹؿ���������Ϊ��!");
        }
    }

    // �������˵��ķ���
    public void BackToMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.Log("�������˵�: " + mainMenuSceneName);
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("���˵��������� (mainMenuSceneName) δ�� LevelSelectManager ������!");
        }
    }
}