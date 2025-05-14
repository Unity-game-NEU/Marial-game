// LevelSelectManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Scene Navigation")]
    public string mainMenuSceneName = "MainMenuScene"; // 你的主菜单场景文件名

    // 公共方法，用于被关卡按钮调用
    // 参数 'levelSceneName' 将是你要加载的实际游戏关卡的场景文件名
    public void LoadLevel(string levelSceneName)
    {
        if (!string.IsNullOrEmpty(levelSceneName))
        {
            Debug.Log("加载关卡: " + levelSceneName);
            SceneManager.LoadScene(levelSceneName);
        }
        else
        {
            Debug.LogError("要加载的关卡场景名称为空!");
        }
    }

    // 返回主菜单的方法
    public void BackToMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.Log("返回主菜单: " + mainMenuSceneName);
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("主菜单场景名称 (mainMenuSceneName) 未在 LevelSelectManager 中设置!");
        }
    }
}