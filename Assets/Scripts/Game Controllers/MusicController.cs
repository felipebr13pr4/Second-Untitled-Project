using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : AudioPlayer
{
    [SerializeField] private AudioClip[] m_musics;
    private string m_pastScene;

    public static MusicController Instance { get; private set; }
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

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Start()
    {
        m_audioVolume = PlayerPrefs.GetFloat(PrefKeys.MusicVolume, 0.25f);
        SetAudio(m_audioVolume);
        DecideMusic();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => DecideMusic();

    private void DecideMusic()
    {
        if (m_pastScene == "" ||  m_pastScene != SceneManager.GetActiveScene().name)
        {
            //if (SceneManager.GetActiveScene().name == SceneNames.MainMenu) ;
            //PlayMusic(m_musics[0]); uncomment when theres actually is music.

            //else if (SceneManager.GetActiveScene().name == SceneNames.MainGame) ;
            //PlayMusic(m_musics[1]); uncomment when theres actually is music.
        }
        m_pastScene = SceneManager.GetActiveScene().name;

    }

    public void PlayMusic(AudioClip clip)
    {
        m_audioSource.clip = clip;
        m_audioSource.Play();
    }
}