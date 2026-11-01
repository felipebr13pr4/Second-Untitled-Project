using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private string m_currentScene = SceneNames.MainMenu;
    public bool IsInMenu => m_currentScene == SceneNames.MainMenu;


    public static SceneController Instance { get; private set; }
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetThings;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetThings;
    }

    private void Update()
    {
        // if (m_currentScene == SceneNames.MainMenu) return;
        if (Keyboard.current.rKey.wasPressedThisFrame)
            ReloadScene();
    }

    public void LoadScene(SceneType type)
    {
        SavingController.Instance.SaveAll();
        string sceneToLoad = type switch
        {
            SceneType.Game => SceneNames.MainGame,
            SceneType.Menu => SceneNames.MainMenu,
            _ => SceneNames.MainMenu,
        };

        m_currentScene = sceneToLoad;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ReloadScene()
    {
        SavingController.Instance.SaveAll();
        m_currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(m_currentScene);
    }

    private void ResetThings(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
        GameStateController.Instance.ResetStates();
    }
}