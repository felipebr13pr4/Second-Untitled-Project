using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController Instance { get; private set; }
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
        // Put things here.
    }

    private void OnDisable()
    {
        // Put things here.
    }
}
