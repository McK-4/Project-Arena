using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{

    [SerializeField] GameObject[] items;

    [SerializeField] GameObject[] powerUps;

    Scene currentScene;

    private Vector2[] itemSpawnPos;

    private Vector2[] powerUpSpawnPos;

    private bool[] spawned;

    private int ranIndex;

    [SerializeField] float spawntimer = 0;
    [SerializeField] float spawnCooldownTime = 30f;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        //If the map is the Marygold mines
        if (currentScene.buildIndex == 1)
        {
            itemSpawnPos = new Vector2[4];

            powerUpSpawnPos = new Vector2[2];

            //spawned = new bool[itemSpawnPos.Length];
            itemSpawnPos[0] = new Vector2(-9.5f, -3.1f);
            itemSpawnPos[1] = new Vector2(-7.5f, 2.85f);
            itemSpawnPos[2] = new Vector2(4.5f, 9.85f);
            itemSpawnPos[3] = new Vector2(6.5f, -5.1f);

            powerUpSpawnPos[0] = new Vector2(-6.5f, -9f);
            powerUpSpawnPos[1] = new Vector2(6f, 1.5f);
        }

        spawntimer = 30f;
        spawnCooldownTime = 30f;

    }

    // Update is called once per frame
    void Update()
    {
        //May not be nessesary:
        //currentScene = SceneManager.GetActiveScene();

        if (spawntimer < spawnCooldownTime)
        {
            spawntimer += Time.deltaTime;
        }
        else if (spawntimer >= spawnCooldownTime)
        {
            spawntimer = 0;
            foreach (Vector2 itemSpawn in itemSpawnPos)
            {
                ItemSpawn();
            }
            /*
            foreach(Vector2 powerUpSpawn in powerUpSpawnPos)
            {

            }
            */
        }
    }

    private void ItemSpawn()
    {
        //getting randomItems
        int ranItem = RandomNum(0, items.Length);

        //getting all objects in scene
        GameObject[] itemsInScene = GameObject.FindGameObjectsWithTag("Pick Up");

        //finding possible spawn locations
        List<Vector2> possibleSpots = new List<Vector2>(itemSpawnPos);

        for (int i = 0; i < possibleSpots.Count; i++)
        {
            //Debug.Log(i);
            bool remove = false;
            foreach (GameObject item in itemsInScene)
            {
                if (new Vector2(item.transform.position.x, item.transform.position.y) == possibleSpots[i])
                {
                    remove = true;
                }
            }
            if (remove)
            {
                possibleSpots.RemoveAt(i);
                i--;
            }
        }

        if (possibleSpots.Count > 0)
        {
            //getting random location out of possible locations
            ranIndex = RandomNum(0, possibleSpots.Count - 1);

            //spawning object
            GameObject ip;
            ip = Instantiate(items[ranItem], possibleSpots[ranIndex], Quaternion.identity);
            NameChange(ip);
            //spawned[ranIndex] = true;
        }
    }

    private int RandomNum(int min, int max)
    {
        return Random.Range(min, max);
    }

    private void NameChange(GameObject ip)
    {
        if(ip.name.Substring(0, 3) == "Bow")
        {
            ip.name = "Bow";
        }
        else if(ip.name.Substring(0, 4) == "Bomb")
        {
            ip.name = "Bomb";
        }
        else if(ip.name.Substring(0, 10) == "Dark Leech")
        {
            ip.name = "Dark Leech";
        }
        else if(ip.name.Substring(0, 16) == "Glove of Thunder")
        {
            ip.name = "Glove of Thunder";
        }
    }

}
