using System.Collections;
using UnityEngine;

public class SavingController : MonoBehaviour
{
    private bool m_isAutoSaveOn = true;

    public static SavingController Instance { get; private set; }
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

    private void OnEnable() => GameStateController.OnGamePaused += SaveAll;

    private void Start() => StartCoroutine(AutoSave());

    private void OnDisable() => GameStateController.OnGamePaused -= SaveAll;

    private void OnApplicationQuit() => SaveAll();

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5);
            if (m_isAutoSaveOn) Save();
        }
    }

    public void SaveAll()
    {
        Save();
        SavePlayerPrefs();
        PlayerPrefs.Save();
    }

    private void Save()
    {
        // Put things here.
    }

    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat(PrefKeys.Volume, AudioController.Instance.AudioVolume);
        PlayerPrefs.SetFloat(PrefKeys.MusicVolume, MusicController.Instance.AudioVolume);
        PlayerPrefs.SetInt(PrefKeys.ScreenWidth, Screen.width);
        PlayerPrefs.SetInt(PrefKeys.ScreenHeight, Screen.height);
        PlayerPrefs.SetInt(PrefKeys.FullScreen, Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? 1 : 0);
    }
}