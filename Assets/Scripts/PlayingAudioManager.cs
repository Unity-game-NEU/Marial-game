using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop = false;
        public bool playOnStart = false; // 新增：标记在游戏启动时播放
    }

    public SoundEffect[] soundEffects;
    private Dictionary<string, SoundEffect> soundDictionary;

    // 仅用于循环音频的AudioSource
    private Dictionary<string, AudioSource> loopingSources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 初始化字典
            soundDictionary = new Dictionary<string, SoundEffect>();
            loopingSources = new Dictionary<string, AudioSource>();

            foreach (var sound in soundEffects)
            {
                soundDictionary[sound.name] = sound;

                // 为循环音频预创建AudioSource
                if (sound.loop)
                {
                    GameObject sourceObj = new GameObject($"LoopingAudio_{sound.name}");
                    sourceObj.transform.SetParent(transform);
                    AudioSource source = sourceObj.AddComponent<AudioSource>();
                    source.clip = sound.clip;
                    source.volume = sound.volume;
                    source.pitch = 1.0f;  // 强制设置为1.0，确保正常速度
                    source.loop = true;
                    source.playOnAwake = false;
                    source.spatialBlend = 0;  // 确保是2D音效

                    // 禁用所有可能影响音质的特效
                    source.bypassEffects = true;
                    source.bypassListenerEffects = true;
                    source.bypassReverbZones = true;
                    source.enabled = true;

                    loopingSources[sound.name] = source;
                    if (sound.name == "Background" && sound.playOnStart)
                    {
                        source.Play();
                        Debug.Log($"播放音频: {sound.name}, 使用音频剪辑: {source.clip.name}");
                    }
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 播放声音
    public void PlaySound(string soundName, Vector3? position = null)
    {
        if (!soundDictionary.TryGetValue(soundName, out SoundEffect sound))
        {
            Debug.LogWarning($"声音 {soundName} 未找到!");
            return;
        }

        // 循环音频使用预创建的AudioSource
        if (sound.loop)
        {
            if (loopingSources.TryGetValue(soundName, out AudioSource source))
            {
                if (!source.isPlaying)
                {
                    source.Play();
                }
            }
            return;
        }

        // 非循环音频直接使用PlayClipAtPoint
        Vector3 pos = position ?? Camera.main.transform.position;
        AudioSource.PlayClipAtPoint(sound.clip, pos, sound.volume);
    }

    // 停止特定声音
    public void StopSound(string soundName)
    {
        if (loopingSources.TryGetValue(soundName, out AudioSource source))
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    // 停止所有循环声音
    public void StopAllLoopingSounds()
    {
        foreach (var source in loopingSources.Values)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

}