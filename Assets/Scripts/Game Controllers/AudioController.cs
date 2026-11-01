using UnityEngine;

public class AudioController : AudioBasics
{
    public static AudioController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_audioVolume = PlayerPrefs.GetFloat(PrefKeys.Volume, 1f);
        SetAudio(m_audioVolume);
    }
}