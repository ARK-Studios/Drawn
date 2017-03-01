using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARK.Player;

public class LivesManager : MonoBehaviour {
    public PlayerProfile player;
    public Sprite[] reg_nums_sprites;
    public Image lives_ones_ui;
    public Image lives_tenths_ui;

	// Use this for initialization
	void Start () {
        UpdatePlayerLivesUi();
    }
	
	// Update is called once per frame
	void Update () {
        UpdatePlayerLivesUi();
    }

    private int[] SplitLives (int lives)
    {
        int[] split_lives = new int[2];

        split_lives[0] = lives % 10;
        split_lives[1] = lives / 10;

        return split_lives;
    }

    private void UpdatePlayerLivesUi()
    {
        int[] lives = SplitLives(player.lives);
        lives_ones_ui.sprite = reg_nums_sprites[lives[0]];
        lives_tenths_ui.sprite = reg_nums_sprites[lives[1]];
    }
}
