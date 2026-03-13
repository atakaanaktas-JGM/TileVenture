using UnityEngine;

public class ScenePersist : MonoBehaviour
{

    void Awake()
    {
        // create our Singleton
        int numberScenePersist = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (numberScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RestartScenePersist()
    {
        Destroy(gameObject );
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
