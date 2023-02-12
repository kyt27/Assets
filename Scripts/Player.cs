using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover {
    private int characterID;
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    public int[] hitpoint;
    public int[] maxHitpoint;
    public bool[] upgraded;

    // Arki - 0
    // Hila - 1
    // Chonk - 2
    // Konkon - 3

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitpoint = new int[] { 10, 10, 10, 10 };
        maxHitpoint = new int[] { 10, 10, 10, 10 };
        upgraded = new bool[] { false, false, false, false };
    }

    private void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (isAlive) UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapCharacter(int characterID) {
        spriteRenderer.sprite = GameManager.instance.playerSprites[characterID];
        this.characterID = characterID;
    }

    public int GetCharacterID() {
        return characterID;
    }

    public void Respawn() {
        isAlive = true;
        hitpoint = maxHitpoint;
    }
}
