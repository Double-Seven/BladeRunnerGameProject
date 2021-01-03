﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    [Header("UI elements")]
    public TMP_Text winText;
    public TMP_Text loseText;
    public TMP_Text teleportText;
    public Button playAgainButton;



    private GameObject player;
    private Player playerScript;
    private LevelLoader levelLoader;
    private List<Monster> monsters;
    private bool isGameOver = false;



    // Start is called before the first frame update
    void Awake()
    {
        // hide the cursor
        Cursor.lockState = CursorLockMode.Locked;

        this.player = GameObject.Find("Player");  // get the Player gameobject
        playerScript = player.GetComponent<Player>();  // get the instance of Player script

        // get the level loader instance
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();


        //set win text and lose text and button to invisible at the beginning
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        teleportText.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false);
        

        //setup the button and bind with playAgainButtonOnClick() function
        Button paButton = playAgainButton.GetComponent<Button>();
        paButton.onClick.AddListener(playAgainButtonOnClick);


    }

    // Update is called once per frame
    void Update()
    {
        // check the Dying status of player
        if(playerScript.isDead && !isGameOver)
        {
            isGameOver = true;
            losePrompt();
        }
    }


    /// <summary>
    /// Getter function for player gameobject
    /// </summary>
    /// <returns></returns>
    public GameObject getPlayer()
    {
        return this.player;
    }

    //button action listenner function for PlayAgainButton
    void playAgainButtonOnClick()
    {
        //reload the whole scene when pressing the playAgainButton
        SceneManager.LoadScene("MainScene");
    }

    //set the cursor to be unlocked and visible
    void setCursorUnlocked()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    /*
    //when the winning condition is triggered
    void winPrompt()
    {
        // deactivate the player moving sript
        this.playerController.gameObject.GetComponent<FirstPersonAIO>().enabled = false;
        this.playerController.gameObject.SetActive(false);
        this.gameOverCamera.enabled = true;

        //show the winning text and playAgain button
        winText.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
        setCursorUnlocked();
        Cursor.visible = true;
    }*/

    //when the losing condition is triggered
    void losePrompt()
    {
        //show the losing text and playAgain button
        loseText.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
        setCursorUnlocked();
        Cursor.visible = true;
    }

    /// <summary>
    /// Function to load the next level using level loader
    /// </summary>
    /// <param name="delay"> The delay before enter the loading animation</param>
    public void loadNextLevel(float delay)
    {
        StartCoroutine(LoadNextLevelWithDelay(delay));
    }

    /// <summary>
    /// Coroutine function to load the level (scene) with a delay
    /// </summary>
    /// <param name="sceneIndexToLoad"></param>
    /// <returns></returns>
    IEnumerator LoadNextLevelWithDelay(float delay)
    {
        // wait for 2 seconds before loading the next level (2 seconds to play the animation)
        yield return new WaitForSeconds(delay);

        // load the next level
        levelLoader.LoadNextLevel();
    }

    /// <summary>
    /// Function to display the teleporting prompt
    /// </summary>
    /// <param name="enabled"></param>
    public void enableTeleportText(bool enabled)
    {
        //show the teleport text
        teleportText.gameObject.SetActive(enabled);
    }
}
