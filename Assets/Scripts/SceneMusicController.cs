// SceneMusicController.cs
using UnityEngine;

[RequireComponent(typeof(AudioSource))] // ȷ����������һ�� AudioSource ���
public class SceneMusicController : MonoBehaviour
{
    private AudioSource audioSource;
    private const string MasterVolumeKey = "MasterVolume"; // �� MainMenuManager ��ʹ�õļ�����ͬ

    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // ��ȡ AudioSource ���������
    }

    void Start()
    {
        // �����ѱ�������������δ�ҵ���Ĭ��Ϊ 0.5f
        float savedVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);
        audioSource.volume = savedVolume; // ��������

        // ��� AudioSource �ϵ� Play On Awake �ǹ�ѡ�ģ����������������ʼ���š�
        // �����ȡ����ѡ AudioSource �ϵ� Play On Awake ����ӽű����Ʋ��ţ�
        // if (!audioSource.isPlaying)
        // {
        //     audioSource.Play();
        // }
    }
}