using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Enemy : Mover {
    // Logic
    public float triggerLength = 0.4f;
    public float chaseLength = 1.0f;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    public int hitpoint = 10;
    public int maxHitpoint = 10;

    private bool move;
    private float moveTime = 0.8f;
    private float idleTime = 0.8f;
    private float moveStart;
    private float idleStart;
    private Vector3 randVec;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start() {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        randVec = Random.insideUnitCircle;
        idleStart = Time.time;
        move = false;
    }

    private void FixedUpdate() {
        // Is player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength) {
            if (Vector3.Distance(playerTransform.position, transform.position) < triggerLength) {
                chasing = true;
            }
            if (chasing) {
                if (!collidingWithPlayer) {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                } else {
                    UpdateMotor(startingPosition - transform.position);
                }
            } else {
                UpdateMotor(startingPosition - transform.position);
            }
        } else {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
            /*
            if (move) {
                if (Time.time - moveStart < moveTime) {
                    if (Vector3.Distance(transform.position, startingPosition) < triggerLength) {
                        UpdateMotor(randVec);
                    }
                } else {
                    idleStart = Time.time;
                    move = false;
                }
            } else {
                if (Time.time - idleStart > idleTime) {
                    randVec = Random.insideUnitCircle;
                    moveStart = Time.time;
                    move = true;
                }
            }
            */
        }

        // Check collide with player
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null) continue;

            if (hits[i].tag == "Player") collidingWithPlayer = true;

            hits[i] = null;
        }
    }

    protected override void Death() {
        Destroy(gameObject);
    }
}
