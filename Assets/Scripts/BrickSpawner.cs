using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject Brick;

    [SerializeField]
    float spawnSpeed;

    bool spawnAvailable = true;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAvailable)
        {
            Invoke("spawnBricks", spawnSpeed);
            spawnAvailable = false;
        }
    }
    void spawnBricks()
    {
        int[] bricksStatus = new int[6];

        for(int slot = 0; slot < 6; slot++)
        {
            switch(Random.Range(0, 6))
            {
                case 0:
                    bricksStatus[slot] = 2;
                    break;
                case 1:
                case 2:
                    bricksStatus[slot] = 1;
                    break;
                case 3:
                case 4:
                case 5:
                    bricksStatus[slot] = 0;
                    break;
            }
        }

        for(int slot = 0; slot < 6; slot++)
        {
            GameObject SpawnedBrick;
            switch (bricksStatus[slot])
            {
                case 0:
                    break;
                case 1:
                    //We create the brick
                    SpawnedBrick = Instantiate(Brick, transform.position, Brick.transform.rotation);
                    SpawnedBrick.transform.rotation = Quaternion.Euler(Vector3.forward * slot * 30);
                    break;
                case 2:
                    bricksStatus[slot] = 1;
                    for(int slotCheck = 0; slotCheck < 6; slotCheck++)
                    {
                        Debug.Log(slot + slotCheck);
                        if(slot + slotCheck < 6)
                        {
                            if (bricksStatus[slot + slotCheck] < 1)
                            {
                                bricksStatus[slot]++;
                            }
                            else
                            {
                                SpawnedBrick = Instantiate(Brick, transform.position, Brick.transform.rotation);
                                SpawnedBrick.transform.rotation = Quaternion.Euler(Vector3.forward * slot * 30);
                                SpawnedBrick.GetComponent<DrawBrick>().angle = bricksStatus[slot] * 30;
                                SpawnedBrick.GetComponent<LineRenderer>().transform.rotation = Quaternion.Euler(Vector3.forward * slot * 30);
                            }
                        }
                        else
                        {
                            if (bricksStatus[slot + slotCheck -6] < 1)
                            {
                                bricksStatus[slot]++;
                            }
                            else
                            {
                                SpawnedBrick = Instantiate(Brick, transform.position, Brick.transform.rotation);
                                SpawnedBrick.transform.rotation = Quaternion.Euler(Vector3.forward * slot * 30);
                                SpawnedBrick.GetComponent<DrawBrick>().angle = bricksStatus[slot] * 30;
                                SpawnedBrick.GetComponent<LineRenderer>().transform.rotation = Quaternion.Euler(Vector3.forward * slot * 30);
                            }
                        }
                    }
                    break;
            }
        }
        spawnAvailable = true;
    }
}
