using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallConnectors : MonoBehaviour
{
    public Transform tree;
    public Transform connectorNS;
    public Transform connectorEW;
    public Transform NS;
    public Transform NSS;
    public Transform NSN;
    public Transform NSNS;
    public Transform EW;
    public Transform EWW;
    public Transform EWE;
    public Transform EWEW;
    public bool[] adjacent;
    // Start is called before the first frame update
    public void updateAllConnectors()
    {
        Vector2 cellPos = GridInit.Instance.grid.GetWorldCellPosition(transform.position.x, transform.position.y);
        bool[] adjacentWalls = GridInit.Instance.grid.GetAdjacentWalls((int)cellPos.x, (int)cellPos.y);
        updateConnectors(adjacentWalls);
        if (adjacentWalls[0]) ((GameObject)GridInit.Instance.grid.gridArray[(int)cellPos.x, (int)cellPos.y + 1].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors(GridInit.Instance.grid.GetAdjacentWalls((int)cellPos.x, (int)cellPos.y + 1));
        if (adjacentWalls[1]) ((GameObject)GridInit.Instance.grid.gridArray[(int)cellPos.x + 1, (int)cellPos.y].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors(GridInit.Instance.grid.GetAdjacentWalls((int)cellPos.x + 1, (int)cellPos.y));
        if (adjacentWalls[2]) ((GameObject)GridInit.Instance.grid.gridArray[(int)cellPos.x, (int)cellPos.y - 1].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors(GridInit.Instance.grid.GetAdjacentWalls((int)cellPos.x, (int)cellPos.y - 1));
        if (adjacentWalls[3]) ((GameObject)GridInit.Instance.grid.gridArray[(int)cellPos.x - 1, (int)cellPos.y].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors(GridInit.Instance.grid.GetAdjacentWalls((int)cellPos.x - 1, (int)cellPos.y));
    }


    public void updateConnectors(bool[] adjacentWalls)
    {
        adjacent = adjacentWalls;

        Destroy(connectorNS.gameObject);
        if (adjacentWalls[0] & adjacentWalls[2]) connectorNS = Instantiate(NSNS, transform.position + NSNS.transform.position, Quaternion.identity, transform);
        else if (adjacentWalls[0]) connectorNS = Instantiate(NSN, transform.position + NSN.transform.position, Quaternion.identity, transform);
        else if (adjacentWalls[2]) connectorNS = Instantiate(NSS, transform.position + NSS.transform.position, Quaternion.identity, transform);
        else connectorNS = Instantiate(NS, transform.position, Quaternion.identity, transform);

        Destroy(connectorEW.gameObject);
        if (adjacentWalls[1] & adjacentWalls[3]) connectorEW = Instantiate(EWEW, transform.position + EWEW.transform.position, Quaternion.identity, transform);
        else if (adjacentWalls[1]) connectorEW = Instantiate(EWE, transform.position + EWE.transform.position, Quaternion.identity, transform);
        else if (adjacentWalls[3]) connectorEW = Instantiate(EWW, transform.position + EWW.transform.position, Quaternion.identity, transform);
        else connectorEW = Instantiate(EW, transform.position, Quaternion.identity, transform);
    }


    void Start()
    {
        updateAllConnectors();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateAllConnectors();
    }
}
