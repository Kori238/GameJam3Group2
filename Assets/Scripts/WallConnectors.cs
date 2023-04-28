using System.Collections.Generic;
using UnityEngine;

public class WallConnectors : MonoBehaviour
{
    public Transform center;
    public Transform connectorNS;
    public Transform connectorEW;
    public List<Transform> connectors = new List<Transform>();

    public Transform C;

    public Transform CD;

    //Naming scheme = First 2 letters is the connector, following letters is the directions it should connect to, if it ends in D, it is the destroyed version
    public Transform NS;
    public Transform NSS;
    public Transform NSSD;
    public Transform NSN;
    public Transform NSND;
    public Transform NSNS;
    public Transform NSNSD;
    public Transform EW;
    public Transform EWW;
    public Transform EWWD;
    public Transform EWE;
    public Transform EWED;
    public Transform EWEW;
    public Transform EWEWD;
    public bool[] adjacent;


    private void Start()
    {
        updateAllConnectors();
    }
    public void
        updateAllConnectors() //Updates this wall's connectors and also updates walls connectors in all 4 cardinal directions
    {
        var cellPos = gameObject.GetComponent<Wall>().gridPos;
        updateConnectors();

        if (adjacent[0])
        {
            ((GameObject)Init.Instance.grid.gridArray[(int)cellPos.x, (int)cellPos.y + 1].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        }
        if (adjacent[1])
        {
            ((GameObject)Init.Instance.grid.gridArray[(int)cellPos.x + 1, (int)cellPos.y].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        }
        if (adjacent[2])
        {
            ((GameObject)Init.Instance.grid.gridArray[(int)cellPos.x, (int)cellPos.y - 1].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        }
        if (adjacent[3])
        {
            ((GameObject)Init.Instance.grid.gridArray[(int)cellPos.x - 1, (int)cellPos.y].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        }
    }

    private void GetAdjacentWalls() // Checks if there are walls in all 4 cardinal directions
    {
        adjacent = new bool[4];
        var cellPos = gameObject.GetComponent<Wall>().gridPos;
        if (Init.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y + 1) != null && Init.Instance.grid
                .GetStructureAtCell((int)cellPos.x, (int)cellPos.y + 1).CompareTag("Wall"))
            adjacent[0] = true;
        if (Init.Instance.grid.GetStructureAtCell((int)cellPos.x + 1, (int)cellPos.y) != null && Init.Instance.grid
                .GetStructureAtCell((int)cellPos.x + 1, (int)cellPos.y).CompareTag("Wall"))
            adjacent[1] = true;
        if (Init.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y - 1) != null && Init.Instance.grid
                .GetStructureAtCell((int)cellPos.x, (int)cellPos.y - 1).CompareTag("Wall"))
            adjacent[2] = true;
        if (Init.Instance.grid.GetStructureAtCell((int)cellPos.x - 1, (int)cellPos.y) != null && Init.Instance.grid
                .GetStructureAtCell((int)cellPos.x - 1, (int)cellPos.y).CompareTag("Wall"))
            adjacent[3] = true;
    }


    public void updateConnectors() // Destroys all visual parts of the wall and then rebuilds appropriate ones
    {
        GetAdjacentWalls();

        //Destroy(center.gameObject);
        //Destroy(connectorNS.gameObject);
        //Destroy(connectorEW.gameObject);
        foreach (Transform connector in connectors)
        {
            if (connector == null) continue;
            Destroy(connector.gameObject);
        }

        connectors = new List<Transform>();

        if (gameObject.GetComponent<Wall>().destroyed) //Destroyed Sprites
        {
            connectors.Add(Instantiate(CD, transform.position + C.transform.position, Quaternion.identity, transform));
            if (adjacent[0])
                connectors.Add(Instantiate(NSND, transform.position + NSND.transform.position, Quaternion.identity, transform));
            if (adjacent[2])
                connectors.Add(Instantiate(NSSD, transform.position + NSSD.transform.position, Quaternion.identity, transform));
            if (!adjacent[0] && !adjacent[2]) connectors.Add(Instantiate(NS, transform.position, Quaternion.identity, transform));

            if (adjacent[1])
                connectors.Add(Instantiate(EWED, transform.position + EWED.transform.position, Quaternion.identity, transform));
            if (adjacent[3])
                connectors.Add(Instantiate(EWWD, transform.position + EWWD.transform.position, Quaternion.identity, transform));
            if(!adjacent[1] && !adjacent[3]) connectors.Add(Instantiate(EW, transform.position, Quaternion.identity, transform));
        }
        else //Not Destroyed Sprites
        {
            connectors.Add(Instantiate(C, transform.position + C.transform.position, Quaternion.identity, transform));

            if (adjacent[0])
                connectors.Add(Instantiate(NSN, transform.position + NSN.transform.position, Quaternion.identity, transform));
            if (adjacent[2])
                connectors.Add(Instantiate(NSS, transform.position + NSS.transform.position, Quaternion.identity, transform));
            if (!adjacent[0] && !adjacent[2]) connectors.Add(Instantiate(NS, transform.position, Quaternion.identity, transform));

            if (adjacent[1])
                connectors.Add(Instantiate(EWE, transform.position + EWE.transform.position, Quaternion.identity, transform));
            if (adjacent[3])
                connectors.Add(Instantiate(EWW, transform.position + EWW.transform.position, Quaternion.identity, transform));
            if(!adjacent[1] && !adjacent[3]) connectors.Add(Instantiate(EW, transform.position, Quaternion.identity, transform));
        }
        transform.GetComponent<Wall>().UpdateFindOccupiedSpace();
    }
}
