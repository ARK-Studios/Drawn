using UnityEngine;
using System.Collections;
using Assets.Scripts.Utility;
using ARK.Player;


public class SavePoint : MonoBehaviour
{
    public Sprite active;
    public Sprite deactivated;
    public SpriteRenderer savePtPic;
    public PlayerProfile player;
    public Collider2D savePoint;

    void Start ()
    {
        savePtPic = GetComponent<SpriteRenderer>();
        savePtPic.sprite = active;
    }
    void Update ()
    {
        if (player.currSavePt != savePoint)
        {
            savePtPic.sprite = active;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            savePtPic.sprite = deactivated;
        }
    }

}
