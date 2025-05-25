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

    [Header("Scene Navigation")] // ������Ѿ������ Header ��
    public string levelSelectSceneName = "LevelSelectScene"; // ȷ�����������ȷ
    public string mainMenuSceneName = "MainMenuScene";       // Ҳ����������ͳһ�������˵�������

    private void Awake()
    {
        if (Instance != null && Instance != this) // ȷ�� Instance != this �Է��ظ���������
        {
            DestroyImmediate(gameObject); // ����Ѵ���һ����ͬ��ʵ�������ٵ�ǰ���
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
        Debug.Log($"Lives set to: {lives}"); // ��ѡ�ĵ�����־
    }

    public void SetCoins(int newCoins)
    {
        coins = newCoins;
        Debug.Log($"Coins set to: {coins}"); // ��ѡ�ĵ�����־
    }

    public void ResetPlayerStatsForLevelSelect() // һ���ۺϵķ��������ڴӹؿ�ѡ�����ʱ����
    {
        SetLives(3); // ������ϣ����Ĭ������ֵ
        SetCoins(0);
    }




    private void Start()
    {
        Application.targetFrameRate = 60;
        // NewGame(); // <--- �������Ƴ� NewGame() ���Զ�����
        // NewGame() Ӧ�������˵��� "����Ϸ" ��ť���ض��߼�����
    }

    public void NewGame() // �����ѡ������Ϸ��ʱ���ô˷���
    {
        lives = 3;
        coins = 0;
        // ����ϷĬ�ϴ� 1-1 ��ʼ
        LoadLevel(1, 1);
    }

    public void GameOver()
    {
        // ������������ﵼ���� Game Over ��Ļ��������ֱ�� NewGame()
        // ����: SceneManager.LoadScene("GameOverScreen");
        // Ϊ�˱���ԭ�߼�����ʱ������ NewGame()
        Debug.Log("Game Over! Restarting a new game."); // ������־�������
        NewGame();
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        Debug.Log($"Loading Level: {world}-{stage}"); // ������־�������

        // ���ű������ֵ��߼����Ա���������ȷ��AudioManager������������ȷ
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Background"); // ȷ�� "Background" ��Ч����ȷ
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null. Cannot play background sound.");
        }
        if (world != 3)
        { SceneManager.LoadScene($"{world}-{stage}"); }// ȷ����������Ϊ "1-1", "1-2", "2-1", "2-2"
        else
        {
            SceneManager.LoadScene("MainMenuScene");
            AudioManager.Instance.StopAllLoopingSounds();

         }

    }

    public void NextLevel()
    {
        // ������Ҫ�����Ƶ��߼������������л�����Ϸ����
        // ����: if (stage == 2 && world == 1) LoadLevel(2,1); else if ...
        // ��ʱ����ԭ������ֻ�����ӵ�ǰ����� stage
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevelInternal)); // �޸�Ϊ���� ResetLevelInternal
        Invoke(nameof(ResetLevelInternal), delay); // �޸�Ϊ���� ResetLevelInternal
        // AudioManager.Instance.PlaySound("Background"); // �����Ƿ��������ظ����ţ�LoadLevel������
    }

    // ��ԭ���� ResetLevel() ������Ϊ ResetLevelInternal() ���������Ա����� Invoke �ķ�������ͻ�����
    private void ResetLevelInternal()
    {
        lives--;
        Debug.Log($"Lives remaining: {lives}"); // ������־

        if (lives > 0)
        {
            // AudioManager.Instance.PlaySound("Background"); // LoadLevel�лᲥ��
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    // ����ⲿ����Ҫһ�����ӳٵ� ResetLevel�����Ա��������������ֱ�Ӽ�����������
    public void ResetLevel()
    {
        ResetLevelInternal();
    }


    public void AddCoin()
    {
        coins++;
        // Debug.Log($"Coins: {coins}"); // ��ѡ�ĵ�����־

        if (coins >= 100) // ͨ���� >= 100
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
        // Debug.Log($"Lives: {lives}"); // ��ѡ�ĵ�����־
    }
}