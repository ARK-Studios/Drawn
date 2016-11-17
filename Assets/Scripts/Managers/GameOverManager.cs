using UnityEngine;
using ARK.Player;

namespace ARK.Managers
{
    public class GameOverManager : MonoBehaviour
    {
        public Player.PlayerProfile player;
        public float restartDelay = 5f;         // Time to wait before restarting the level

        Animator anim;                          // Reference to the animator component.
        float restartTimer;                     // Timer to count up to restarting the level


        void Awake()
        {
            // Set up the reference.
            anim = GetComponent<Animator>();
        }


        void Update()
        {
            // If the player has run out of health...
            if (player.lives <= 0)
            {
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
    }
}
