using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JunkRatDeath : Ability
{
    public float playerHealth = 80;
    public float numOfBombs;
    public float timeToDetonate;
    public KeyCode debugHealth = KeyCode.E;

    private float resetTime;
    private Color bombFlash;

    private bool hasBlown;
    private bool toSpawn;
    private bool startTimer;

    private Transform player;
    public GameObject bombsToSpawn;

    private Vector2 spawnBombPos;

    private List<GameObject> bombs = new List<GameObject>();
    

    void Start()
    {
        //player = this.gameObject.GetComponent<Transform>();
        spawnBombPos = new Vector2(player.position.x, player.position.y);
        resetTime = timeToDetonate;

        //CHECKS FOR FUNCTIONS SO THEY DON'T REPEAT
        hasBlown = false;
        toSpawn = true;
        startTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(debugHealth))
        {
            playerHealth -= 10;
        }

        if(playerHealth <= 0)
        {
            //if (gameObject.GetComponent<SpriteRenderer>() != null && gameObject.GetComponent<BoxCollider2D>() != null)
            //{
            //    SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
            //    BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            //
            //    sprite.enabled = false;
            //    collider.enabled = false;
            //}

            //CHECKS IF THE PLAYER IS DEAD
            if (toSpawn)
            {
                SpawnBombs(bombs);
                toSpawn = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (startTimer)
        {
            timeToDetonate -= Time.deltaTime;
        }

        if (timeToDetonate <= 0)
        {
            if (!hasBlown)
            {
                Detonate(bombs);
                hasBlown = true;
            }
        }
    }

    public void SpawnBombs(List<GameObject> bombs)
    {
        startTimer = true;
        float spaceBtw = 1f;

        for (int i = 0; i < numOfBombs; i++)
        {
            //MOVE EACH BOMB AWAY FROM EACH OTHER BY THIS DISTANCE
            spawnBombPos.x -= (spaceBtw / (numOfBombs/2)); //SPAWNS BOMBS EQUALLY IN THE MIDDLE OF JUNKRAT

            bombs.Add(Instantiate(bombsToSpawn, spawnBombPos, Quaternion.identity));//INSTANTIATE 'BOMBS' AT PLAYER LOCATION, WITH A SPACING OF 'spaceBtw'
        }
    }

    public void Detonate(List<GameObject> bombs)
    {
        if (bombs.Count > 0 && bombs != null)
        {
            foreach(GameObject bomb in bombs)
            {
                //DETONATE BOMBS
                    //CODE
                
                //AFTER BOMBS DETONATE, 
                //THEN DESTORY GAMEOBJECTS
                bombs.Remove(bomb);
                Destroy(bomb);
            }
            Debug.Log("BOOM");
        }
        startTimer = false;
    }
}
