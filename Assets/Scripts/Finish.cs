using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    private bool levelCompleted = false;
    [SerializeField] private AudioSource finishSoundEffect;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !levelCompleted)
        {
            UnlockNewLevel();
            levelCompleted = true;
            finishSoundEffect.Play();
            Invoke("CompleteLevel", 2f);
        }
    }

    void UnlockNewLevel()
    {
        if((SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex")) && (SceneManager.GetActiveScene().buildIndex < 5))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel") + 1);
            PlayerPrefs.Save();
        }
    }
    private void CompleteLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}