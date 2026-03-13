
using System;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{

   [SerializeField] int playerLives = 3;
    
  
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image[] hearthsContainer;
    int score = 0;
    
    void Awake()
    {
        // create our Singleton
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
       
         scoreText.text = score.ToString();
       
    }


    // when player dies, do certain things
    // reduce number of lives
    // if we have no lives left then restart the whole game

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
          
            Invoke("TakeLife", 1f);
        
            
        }
        else 
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int addPoint)
    {
        score += addPoint;
        scoreText.text = score.ToString();
    }

    void TakeLife()
    {
        playerLives = playerLives - 1;
        HearthsUpdate();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);


    }

     void ResetGameSession()
    {
        FindFirstObjectByType<ScenePersist>().RestartScenePersist();
        SceneManager.LoadScene(5);
        Destroy(gameObject);
    }

   
    public void HearthsUpdate()
    {
        for (int i = 0; i < hearthsContainer.Length; i++)
        {
            hearthsContainer[i].enabled = i < playerLives;
        }
    }


    void Update()
    {
        
    }
}
