using UnityEngine;
using System.Collections;
using Assets.Scripts.Utility;
using ARK.Player;

public class Respawn : MonoBehaviour {
    public Vector2 origCharPos;
    public bool respawn = false;
    public PlayerProfile player;

    public void RespawnChar(Rigidbody2D character)
    {
        Vector2 respawnPos;
        if (respawn)
        {
            //character.position.Set(origCharPos.x, origCharPos.y + 1);
            respawnPos.x = origCharPos.x;
            respawnPos.y = origCharPos.y;
            character.position = respawnPos;
            character.velocity = new Vector2 (0,0);
            ARKLogger.LogMessage(eLogCategory.Control,
                eLogLevel.Info,
                "Respawn");
            respawn = false;
        }
    }
}