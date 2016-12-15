using UnityEngine;
using System.Collections;
using Assets.Scripts.Utility;
using ARK.Player;


public class SavePoint : MonoBehaviour
{
    private SpriteRenderer savePtPic;
    public Sprite active;
    public Sprite deactivated;

    void Start ()
    {
        savePtPic = GetComponent<SpriteRenderer>();
        if (savePtPic.sprite != active)
            savePtPic.sprite = active;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            savePtPic.sprite = deactivated;
        }
    }

}
