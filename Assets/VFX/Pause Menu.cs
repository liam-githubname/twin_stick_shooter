using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isPaused = false;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] public PlayerControls controls;
    void Start()
    {
        pausePanel.SetActive(false);
        controls = FindObjectOfType<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P) && !isPaused || controls.Gameplay.Pause.IsPressed())       //CHANGE or ADDTHIS TO CONTROLLER
        {
            Time.timeScale = 0;
            isPaused = true;
            PauseSequence();
        }

        else if (Input.GetKeyDown(KeyCode.P) && isPaused)
        { //CHANGE OR ADD THIS TO CONTROLLE
            Time.timeScale = 1;
            isPaused = false;
            pausePanel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R) && isPaused)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Q) && isPaused)
        {
            print("Application Quit");
            Application.Quit();
        }
    }

    private void PauseSequence()
    {
        pausePanel.SetActive(true);



    }
}
