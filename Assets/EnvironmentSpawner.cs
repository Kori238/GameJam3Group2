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
        

        for (int i = 0; i < 50; i++)
        {
            int x = Random.Range(0, 50);
            int y = Random.Range(0, 50);
            int TorS = Random.Range(0, 2);

            if (TorS == 0)
            {
                Init.Instance.grid.BuildAtCell(x, y, tree);
            }
            else
            {
                Init.Instance.grid.BuildAtCell(x, y, stone);
            }
            
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
