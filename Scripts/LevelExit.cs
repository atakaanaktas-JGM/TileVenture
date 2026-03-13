using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float loadDelayTime = 2.0f;
    [SerializeField] AudioClip teleportSound;
    AudioSource teleportSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("LoadNextLevel");
    }

    IEnumerator LoadNextLevel()
    {
        teleportSource.PlayOneShot(teleportSound); 
        yield return new WaitForSecondsRealtime(loadDelayTime);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        FindFirstObjectByType<ScenePersist>().RestartScenePersist();
        SceneManager.LoadScene(currentScene+1);

    }

    private void Start()
    {
        teleportSource = GetComponent<AudioSource>();
    }
}
