using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover {

    public SpriteRenderer spriteRenderer;

    // Arki - 0
    // Hila - 1
    // Chonk - 2
    // Konkon - 3
    public bool isAlive = true;

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (isAlive) UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapCharacter(int characterID) {
        spriteRenderer.sprite = GameManager.instance.playerSprites[characterID];
        PlayerData.instance.characterID = characterID;
    }

    public int GetCharacterID() {
        return PlayerData.instance.characterID;
    }

    public void Respawn() {
        PlayerData.instance.Respawn();
    }
}
