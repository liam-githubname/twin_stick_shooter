using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update[SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverPanel;
    private GameObject player;
    private GameObject ship;
    private bool isGameOver = false;
    private DamageSystem health;
    // Start is called before the first frame update
    void Start()
    {
        //Disables panel if active
        
        player=GameObject.FindWithTag("ship"); //If name of ship changes. THis line is fucked
        if (player==null){
            Debug.Log("Player is NULL");
        }
        GameObject ship=player.transform.GetChild(0).gameObject;
        if (ship==null){
            Debug.Log("Player is NULL");
        }
        health=player.GetComponentInChildren<DamageSystem>();
        if (health==null){
            Debug.Log("Health is NULL");
        }
        gameOverPanel.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        
        //Trigger game over manually and check with bool so it isn't called multiple times
        if (health.getHealth()<=0 && !isGameOver)
        {
            isGameOver = true;
            
            StartCoroutine(GameOverSequence());
        }
        
        //If game is over
        if (isGameOver)
        {
            //If R is hit, restart the current scene
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale=1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                
            }
            
            //If Q is hit, quit the game
            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Application Quit");
                Application.Quit();
            }
        }
        
        
    }

    //controls game over canvas and there's a brief delay between main Game Over text and option to restart/quit text
    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2.0f);
        gameOverPanel.SetActive(true);
        
        

    }
}
