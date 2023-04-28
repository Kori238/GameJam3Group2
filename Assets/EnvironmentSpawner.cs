using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public Transform tree;
    public Transform stone;

    // Start is called before the first frame update
    void Start()
    {


        for (int i = 0; i < 10;)
        {
            int x = Random.Range(5,44);
            int y = Random.Range(5, 44);
            int TorS = Random.Range(0, 2);
            int size = 0;
            int size2 = 0;
            int start = 0;
            int start2 = 0;
            


            if ((x <= 30 && x >= 20) && (y <= 30 && y >= 20))
            {

            }
            else
            {
                if (TorS == 0)
                {

                    size2 = Random.Range(3, 5);
                    start2 = Random.Range(0, 2);
                    for (int r = start2; r < size2; r++)
                    {
                        size = Random.Range(3, 5);
                        start = Random.Range(0, 2);
                        for (int c = start; c < size; c++)
                        {
                            Init.Instance.grid.BuildAtCell(x + c, y + r, tree);
                        }

                    }
                }
                else
                {
                    size2 = Random.Range(3, 5);
                    start2 = Random.Range(0, 2);
                    for (int r = start2; r < size2; r++)
                    {
                        size = Random.Range(3, 5);
                        start = Random.Range(0, 2);
                        for (int c = start; c < size; c++)
                        {
                            Init.Instance.grid.BuildAtCell(x + c, y + r, stone);
                        }

                    }
                }
                i++;
            }


        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
