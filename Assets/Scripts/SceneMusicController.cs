// SceneMusicController.cs
using UnityEngine;

[RequireComponent(typeof(AudioSource))] // 确保对象上有一个 AudioSource 组件
public class SceneMusicController : MonoBehaviour
{
    private AudioSource audioSource;
    private const string MasterVolumeKey = "MasterVolume"; // 与 MainMenuManager 中使用的键名相同

    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // 获取 AudioSource 组件的引用
    }

    void Start()
    {
        // 加载已保存的音量，如果未找到则默认为 0.5f
        float savedVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);
        audioSource.volume = savedVolume; // 设置音量

        // 如果 AudioSource 上的 Play On Awake 是勾选的，它将以这个音量开始播放。
        // 如果你取消勾选 AudioSource 上的 Play On Awake 并想从脚本控制播放：
        // if (!audioSource.isPlaying)
        // {
        //     audioSource.Play();
        // }
    }
}