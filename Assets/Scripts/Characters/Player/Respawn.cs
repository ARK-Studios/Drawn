using UnityEngine;
using System.Collections;
using Assets.Scripts.Utility;

public class Respawn : MonoBehaviour {
    public Vector2 origCharPos;
    public bool dead = false;
    public bool respawn = false;

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
            dead = false;
        }
    }

    void OnGUI()
    {
        if (dead && (GUI.Button(new Rect(200.0f, 100.0f, 300.0f, 100.0f), "Respawn")))
        {
            respawn = true;
        }
    }
}