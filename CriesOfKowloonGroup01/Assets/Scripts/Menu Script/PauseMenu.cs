using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused;
    public static bool MenuOpened;
    
    public GameObject pauseMenuUi;
    public GameObject MenuImage;
    public GameObject ComboList;
    public GameObject ControlScheme;

     void Start() {
        Debug.Log("Game is not paused");
        GamePaused = false;
        MenuOpened = false;
    }

    // Update is called once per frame
    void Update() {
        if(!GamePaused) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                Pause();
            }
        }

        if (GamePaused) {
            //unpause the game
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Resume();
            }
            
            //goes to main menu
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            }
            
            //shows combo list
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Debug.Log("Combo List is opened");
                pauseMenuUi.SetActive(false);
                ComboList.SetActive(true);
                MenuOpened = true;
            }
            
            //shows controls
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                Debug.Log("ControlScheme Open");
                pauseMenuUi.SetActive(false);
                ControlScheme.SetActive(true);
                MenuOpened = true;
            }
        }

        if (MenuOpened) {
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                Debug.Log("Combo List is closed");
                ComboList.SetActive(false);
                ControlScheme.SetActive(false);
                pauseMenuUi.SetActive(true);
                MenuOpened = false;
            }
        }
    }

    void Resume() {
        Debug.Log(" Game Is Not Paused");
        pauseMenuUi.SetActive(false);
        MenuImage.SetActive(false);
        ComboList.SetActive(false);
        ControlScheme.SetActive(false);
        
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause() {
        Debug.Log("Game Is Paused");
        pauseMenuUi.SetActive(true);
        MenuImage.SetActive(true);
        
        Time.timeScale = 0f;
        GamePaused = true;
    }
}
