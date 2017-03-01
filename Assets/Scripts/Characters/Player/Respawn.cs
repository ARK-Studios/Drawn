using UnityEngine;
using System.Collections;
using Assets.Scripts.Utility;
using ARK.Player;

public class Respawn : MonoBehaviour
{
    public Vector2 charSpawnPos;    //  Character respawn position
    public bool respawn = false;    // decide if character needs respawn
    private PlayerProfile player;   // current player profile

    private void Awake()
    {
        player = GetComponent<PlayerProfile>();
    }

    // set character postion to the last saved point
    public void RespawnChar(Rigidbody2D character)
    {
        Vector2 respawnPos;
        if (respawn)
        {
            //character.position.Set(origCharPos.x, origCharPos.y + 1);
            respawnPos.x = charSpawnPos.x;
            respawnPos.y = charSpawnPos.y;
            character.position = respawnPos;
            character.velocity = new Vector2(0, 0);
            ARKLogger.LogMessage(eLogCategory.Control,
                eLogLevel.Info,
                "Respawn");
            respawn = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "SavePt")
        {
            charSpawnPos = player.CharacterPosition();

        }
    }
}