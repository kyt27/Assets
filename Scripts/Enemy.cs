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
    public Transform playerTransform;
    private Vector3 startingPosition;
    private Animator anim;

    public int id;
    public List<string> types = new() { "bat" };
    public List<string> names = new() { "Bat" };
    public List<int> hitpoints = new() { 10 };
    public List<int> maxHitpoints = new() { 10 };

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
    private GameManager gameManager;

    protected override void Start() {
        base.Start();
        gameManager = GameManager.instance;
        playerTransform = gameManager.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetComponent<BoxCollider2D>();
        randVec = Random.insideUnitCircle;
        idleStart = Time.time;
        anim = GetComponent<Animator>();
        move = false;
    }

    public void Move(Vector3 motion)
    {
        Debug.Log("Moving");
        if ((motion).x < 0) {
            anim.SetBool("mirror", true);
        } else {
            anim.SetBool("mirror", false);
        }
        UpdateMotor(motion);
    }

    private void FixedUpdate() {
        // Is player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            Debug.Log(names[0]);
            Debug.Log("Close Enough");
            if (Vector3.Distance(playerTransform.position, transform.position) < triggerLength) {
                chasing = true;
            }
            if (chasing) {
                if (!collidingWithPlayer) {
                    Move((playerTransform.position - transform.position).normalized);
                } else {
                    Move(startingPosition - transform.position);
                }
            } else {
                Move(startingPosition - transform.position);
            }
        } else {
            Move(startingPosition - transform.position);
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

            if (hits[i].tag == "Player")
            {
                collidingWithPlayer = true;
                gameManager.LoadBattle(this);
                break;
            }

            hits[i] = null;
        }
    }

    protected override void Death() {
        Destroy(gameObject);
    }
}
