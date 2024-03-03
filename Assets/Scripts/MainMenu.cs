using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject levelSelect;

    private bool isFirstTime = true; // Flag to track if it's the first time the game is turned on

    void Start()
    {
        // Check if "UnlockedLevel" key exists in PlayerPrefs
        if (!PlayerPrefs.HasKey("UnlockedLevel"))
        {
            // Set default value for "UnlockedLevel" only if it's the first time
            if (isFirstTime)
            {
                PlayerPrefs.SetInt("UnlockedLevel", 1);
                PlayerPrefs.Save();
                isFirstTime = false; // Set the flag to false after setting the default value
            }
        }
    }

    public void GoToMain()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        levelSelect.SetActive(false);
    }
    public void GoToOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        levelSelect.SetActive(false);
    }
    public void OnPlayClicked()
    {
        if(PlayerPrefs.GetInt("ReachedIndex") < 1)
        {
            PlayerPrefs.SetInt("ReachedIndex", 1);
        }
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        levelSelect.SetActive(true);
        
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    List<int> widths = new List<int>() {2560, 1920, 1600, 1280, 960, 640};
    List<int> heights = new List<int>() {1440, 1080, 900, 720, 540, 360};

    public void SetScreenSize (int index)
    {
        bool fullscreen = Screen.fullScreen;
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, fullscreen);
    }

    public void SetFullScreen(bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
    }
}
