using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public Transform player;

    public GameObject box = default;
    public GameObject bullet = default;
    public GameObject floor;

    public Vector2 whereSpawn;
    public Vector2 whereSpawnB;

    public KeyCode spawnBox = KeyCode.E;
    public KeyCode shoot = KeyCode.Q;
    public KeyCode delBox = KeyCode.Space;

    public int maxAmt = 10;
    public int bulletSpeed = 10;
    private int count;

    private List<GameObject> boxesSpawned = new List<GameObject>();
    private List<GameObject> bulletsSpawned = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        player = this.transform;
        whereSpawn = new Vector2(player.position.x, player.position.y + 10);
        whereSpawnB = new Vector2(player.position.x + 1f, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(spawnBox) && boxesSpawned.Count <= maxAmt)
        {
            aSpawnBox();
        }

        if (Input.GetKeyDown(shoot))
        {
            aShoot();
        }
    }

    private void aSpawnBox()
    {
        boxesSpawned.Add(Instantiate(box, whereSpawn, Quaternion.identity));

        if (boxesSpawned.Count != 0 && boxesSpawned != null)
        {
            if (Input.GetKeyDown(delBox))
            {
                Object.Destroy(boxesSpawned[count]);
                count++;

                if(count > boxesSpawned.Count)
                    count = 0;
            }
        }
    }

    private void aShoot()
    {
        bulletsSpawned.Add(Instantiate(bullet, whereSpawn, Quaternion.identity));
        Rigidbody2D rb;
        foreach(GameObject bullet in bulletsSpawned)
        {
            rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            rb.AddForce(transform.forward * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}
