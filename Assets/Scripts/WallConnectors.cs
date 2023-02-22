using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallConnectors : MonoBehaviour
{
    public Transform center;
    public Transform connectorNS;
    public Transform connectorEW;
    public Transform C;
    public Transform CD;
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
    public Vector2 cellPos;
    // Start is called before the first frame update
    public void updateAllConnectors()
    {
        cellPos = GridInit.Instance.grid.GetWorldCellPosition(transform.position.x, transform.position.y);
        updateConnectors();

        if (adjacent[0]) ((GameObject)GridInit.Instance.grid.gridArray[(int)cellPos.x, (int)cellPos.y + 1].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        if (adjacent[1]) ((GameObject)GridInit.Instance.grid.gridArray[(int)cellPos.x + 1, (int)cellPos.y].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        if (adjacent[2]) ((GameObject)GridInit.Instance.grid.gridArray[(int)cellPos.x, (int)cellPos.y - 1].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        if (adjacent[3]) ((GameObject)GridInit.Instance.grid.gridArray[(int)cellPos.x - 1, (int)cellPos.y].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
    }

    private void GetAdjacentWalls()
    {
        adjacent = new bool[4];
        cellPos = GridInit.Instance.grid.GetWorldCellPosition(transform.position.x, transform.position.y);
        if (GridInit.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y + 1) != null && GridInit.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y + 1).CompareTag("Wall"))
            adjacent[0] = true;
        if (GridInit.Instance.grid.GetStructureAtCell((int)cellPos.x + 1, (int)cellPos.y) != null && GridInit.Instance.grid.GetStructureAtCell((int)cellPos.x + 1, (int)cellPos.y).CompareTag("Wall"))
            adjacent[1] = true;
        if (GridInit.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y - 1) != null && GridInit.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y - 1).CompareTag("Wall"))
            adjacent[2] = true;
        if (GridInit.Instance.grid.GetStructureAtCell((int)cellPos.x - 1, (int)cellPos.y) != null && GridInit.Instance.grid.GetStructureAtCell((int)cellPos.x - 1, (int)cellPos.y).CompareTag("Wall"))
            adjacent[3] = true;
    }


    public void updateConnectors()
    { 
        GetAdjacentWalls();

        Destroy(center.gameObject);
        Destroy(connectorNS.gameObject);
        Destroy(connectorEW.gameObject);

        if (gameObject.GetComponent<Wall>().destroyed)
        {
            center = Instantiate(CD, transform.position + NSNS.transform.position, Quaternion.identity, transform);
            if (adjacent[0] & adjacent[2]) connectorNS = Instantiate(NSNSD, transform.position + NSNS.transform.position, Quaternion.identity, transform);
            else if (adjacent[0]) connectorNS = Instantiate(NSND, transform.position + NSN.transform.position, Quaternion.identity, transform);
            else if (adjacent[2]) connectorNS = Instantiate(NSSD, transform.position + NSS.transform.position, Quaternion.identity, transform);
            else connectorNS = Instantiate(NS, transform.position, Quaternion.identity, transform);

            if (adjacent[1] & adjacent[3]) connectorEW = Instantiate(EWEWD, transform.position + EWEW.transform.position, Quaternion.identity, transform);
            else if (adjacent[1]) connectorEW = Instantiate(EWED, transform.position + EWE.transform.position, Quaternion.identity, transform);
            else if (adjacent[3]) connectorEW = Instantiate(EWWD, transform.position + EWW.transform.position, Quaternion.identity, transform);
            else connectorEW = Instantiate(EW, transform.position, Quaternion.identity, transform);
        } else
        {
            center = Instantiate(C, transform.position + NSNS.transform.position, Quaternion.identity, transform);
            if (adjacent[0] & adjacent[2]) connectorNS = Instantiate(NSNS, transform.position + NSNS.transform.position, Quaternion.identity, transform);
            else if (adjacent[0]) connectorNS = Instantiate(NSN, transform.position + NSN.transform.position, Quaternion.identity, transform);
            else if (adjacent[2]) connectorNS = Instantiate(NSS, transform.position + NSS.transform.position, Quaternion.identity, transform);
            else connectorNS = Instantiate(NS, transform.position, Quaternion.identity, transform);

            if (adjacent[1] & adjacent[3]) connectorEW = Instantiate(EWEW, transform.position + EWEW.transform.position, Quaternion.identity, transform);
            else if (adjacent[1]) connectorEW = Instantiate(EWE, transform.position + EWE.transform.position, Quaternion.identity, transform);
            else if (adjacent[3]) connectorEW = Instantiate(EWW, transform.position + EWW.transform.position, Quaternion.identity, transform);
            else connectorEW = Instantiate(EW, transform.position, Quaternion.identity, transform);
        }
    }


    void Start()
    {
        updateAllConnectors();
    }
}
