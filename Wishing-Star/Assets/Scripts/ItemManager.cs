using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{

    [SerializeField] GameObject[] items;

    Scene currentScene;

    private Vector2[] itemSpawnPos;

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
            //spawned = new bool[itemSpawnPos.Length];
            itemSpawnPos[0] = new Vector2(-9.5f, -3.1f);
            itemSpawnPos[1] = new Vector2(-7.5f, 2.85f);
            itemSpawnPos[2] = new Vector2(4.5f, 9.85f);
            itemSpawnPos[3] = new Vector2(6.5f, -5.1f);
            //itemSpawnPos[4] = new Vector2(0f, 0f);
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
        }
    }

    private void ItemSpawn()
    {
        //getting randomItems
        int ranItem = RandomNum(1, items.Length);

        //getting all objects in scene
        GameObject[] itemsInScene = GameObject.FindGameObjectsWithTag("Pick Up");

        //finding possible spawn locations
        List<Vector2> possibleSpots = new List<Vector2>(itemSpawnPos);

        for (int i = 0; i < possibleSpots.Count; i++)
        {
            Debug.Log(i);
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
            /*int*/
            ranIndex = RandomNum(0, possibleSpots.Count - 1);

            //spawning object
            GameObject ip;
            ip = Instantiate(items[ranItem], possibleSpots[ranIndex], Quaternion.identity);
            //spawned[ranIndex] = true;
        }
    }

    private int RandomNum(int min, int max)
    {
        return Random.Range(min, max);
    }

}
