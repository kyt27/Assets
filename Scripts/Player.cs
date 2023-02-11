using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover {
    private int skinID;
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (isAlive) UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinID) {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinID];
    }
    // Arki - 0
    // Hila - 1
    // Chonk - 2
    // Konkon - 3

    public int getSkinID() {
        return skinID;
    }

    public void Respawn() {
        isAlive = true;
        hitpoint = maxHitpoint;
    }
}
