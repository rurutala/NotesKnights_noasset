using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI master_volume_text;

    [SerializeField]
    TextMeshProUGUI bgm_volume_text;

    [SerializeField]
    TextMeshProUGUI se_volume_text;

    [SerializeField]
    private AudioMixer audioMixer;

    public List<AudioClip> bgmList = new List<AudioClip>();
    public List<AudioClip> seList = new List<AudioClip>();

    [SerializeField] AudioSource bgmSource; // BGM再生用のAudioSource
    [SerializeField] AudioSource seSource;  // SE再生用のAudioSource

    public static AudioManager Instance;

    private void Awake()
    {
        // シングルトンのインスタンスを確立
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 既に存在する場合は新しいオブジェクトを破棄
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaster(float volume)
    {
        audioMixer.SetFloat("MasterVol", volume);
        float conversion_volume = (volume + 80);
        master_volume_text.text = conversion_volume.ToString("0");
    }

    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMVol", volume);
        float conversion_volume = (volume + 80);
        bgm_volume_text.text = conversion_volume.ToString("0");
    }

    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEVol", volume);
        float conversion_volume = (volume + 80);
        se_volume_text.text = conversion_volume.ToString("0");
    }

    /// <summary>
    /// SEを再生する関数
    /// </summary>
    /// <param name="seClip"></param>
    public void PlaySE(AudioClip seClip)
    {

        if (seClip != null)
        {
            seSource.PlayOneShot(seClip);
        }
    }

    public void PlayBGM(AudioClip bgmClip)
    {
        bgmSource.Stop();
        if (bgmClip != null)
        {

            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
    }
}
