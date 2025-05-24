using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;
    public int coins { get; private set; } = 0;

    [Header("Scene Navigation")] // 你可能已经有这个 Header 了
    public string levelSelectSceneName = "LevelSelectScene"; // 确保这个名字正确
    public string mainMenuSceneName = "MainMenuScene";       // 也可以在这里统一管理主菜单场景名

    private void Awake()
    {
        if (Instance != null && Instance != this) // 确保 Instance != this 以防重复销毁自身
        {
            DestroyImmediate(gameObject); // 如果已存在一个不同的实例，销毁当前这个
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
  
    public void SetLives(int newLives)
    {
        lives = newLives;
        Debug.Log($"Lives set to: {lives}"); // 可选的调试日志
    }

    public void SetCoins(int newCoins)
    {
        coins = newCoins;
        Debug.Log($"Coins set to: {coins}"); // 可选的调试日志
    }

    public void ResetPlayerStatsForLevelSelect() // 一个综合的方法，用于从关卡选择进入时重置
    {
        SetLives(3); // 或者你希望的默认生命值
        SetCoins(0);
    }

  


    private void Start()
    {
        Application.targetFrameRate = 60;
        // NewGame(); // <--- 从这里移除 NewGame() 的自动调用
        // NewGame() 应该由主菜单的 "新游戏" 按钮或特定逻辑触发
    }

    public void NewGame() // 当玩家选择“新游戏”时调用此方法
    {
        lives = 3;
        coins = 0;
        // 新游戏默认从 1-1 开始
        LoadLevel(1, 1);
    }

    public void GameOver()
    {
        // 你可能想在这里导航到 Game Over 屏幕，而不是直接 NewGame()
        // 例如: SceneManager.LoadScene("GameOverScreen");
        // 为了保持原逻辑，暂时还调用 NewGame()
        Debug.Log("Game Over! Restarting a new game."); // 增加日志方便调试
        NewGame();
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        Debug.Log($"Loading Level: {world}-{stage}"); // 增加日志方便调试

        // 播放背景音乐的逻辑可以保留，但请确保AudioManager存在且配置正确
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Background"); // 确保 "Background" 音效名正确
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null. Cannot play background sound.");
        }

        SceneManager.LoadScene($"{world}-{stage}"); // 确保场景命名为 "1-1", "1-2", "2-1", "2-2"
    }

    public void NextLevel()
    {
        // 这里需要更完善的逻辑来处理世界切换和游戏结束
        // 例如: if (stage == 2 && world == 1) LoadLevel(2,1); else if ...
        // 暂时保持原样，它只会增加当前世界的 stage
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevelInternal)); // 修改为调用 ResetLevelInternal
        Invoke(nameof(ResetLevelInternal), delay); // 修改为调用 ResetLevelInternal
        // AudioManager.Instance.PlaySound("Background"); // 考虑是否在这里重复播放，LoadLevel里已有
    }

    // 将原来的 ResetLevel() 重命名为 ResetLevelInternal() 或其他，以避免与 Invoke 的方法名冲突或混淆
    private void ResetLevelInternal()
    {
        lives--;
        Debug.Log($"Lives remaining: {lives}"); // 增加日志

        if (lives > 0)
        {
            // AudioManager.Instance.PlaySound("Background"); // LoadLevel中会播放
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    // 如果外部仍需要一个无延迟的 ResetLevel，可以保留这个，但它会直接减生命并加载
    public void ResetLevel()
    {
        ResetLevelInternal();
    }


    public void AddCoin()
    {
        coins++;
        // Debug.Log($"Coins: {coins}"); // 可选的调试日志

        if (coins >= 100) // 通常是 >= 100
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
        // Debug.Log($"Lives: {lives}"); // 可选的调试日志
    }
}