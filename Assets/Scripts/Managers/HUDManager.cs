using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARK.Player;

namespace ARK.Managers
{
    public class HUDManager : MonoBehaviour
    {
        public Player.PlayerProfile player;
        public float restartDelay = 5f;         // Time to wait before restarting the level
        public GameObject[] gameoverMenuButtons;
        private Animator anim;                          // Reference to the animator component.
        float restartTimer;                     // Timer to count up to restarting the level
        bool isPaused = false;
        public GameObject[] pauseMenu;
        
        void Awake()
        {
            // Set up the reference.
            anim = GetComponent<Animator>();
            //pauseMenu = GameObject.FindGameObjectsWithTag("PauseMenu");
            Time.timeScale = 1;
            setObjects(pauseMenu, false);
            setObjects(gameoverMenuButtons, false);
        }


        void Update()
        {

            GameOverCheck();
            PauseCheck();

        }

        private void GameOverCheck()
        {
            // If the player has run out of health...
            if (player.lives <= 0)
            {
                setObjects(gameoverMenuButtons, true);
                // ... tell the animator the game is over.
                anim.SetTrigger("GameOver");
                /*
                // .. increment a timer to count up to restarting.
                restartTimer += Time.deltaTime;

                // .. if it reaches the restart delay...
                if (restartTimer >= restartDelay)
                {
                    // .. then reload the currently loaded level.
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            */
            }
        }
        private void PauseCheck()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
                if (isPaused)
                {
                    Time.timeScale = 0;
                    setObjects(pauseMenu,true);
                }
                else
                {
                    Time.timeScale = 1;
                    setObjects(pauseMenu, false);
                }
            }
        }
        private void setObjects(GameObject[] a, bool isVisible)
        {
            foreach (GameObject g in a)
            {
                g.SetActive(isVisible);
            }
        }

    }
}
