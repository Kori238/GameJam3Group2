using UnityEngine;

public class WallConnectors : MonoBehaviour
{
    public Transform center;
    public Transform connectorNS;
    public Transform connectorEW;

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
    public void updateAllConnectors() //Updates this wall's connectors and also updates walls connectors in all 4 cardinal directions
    {
        Vector2 cellPos = gameObject.GetComponent<Wall>().gridPos;
        updateConnectors();

        if (adjacent[0]) ((GameObject)Init.Instance.grid.gridArray[(int)cellPos.x, (int)cellPos.y + 1].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        if (adjacent[1]) ((GameObject)Init.Instance.grid.gridArray[(int)cellPos.x + 1, (int)cellPos.y].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        if (adjacent[2]) ((GameObject)Init.Instance.grid.gridArray[(int)cellPos.x, (int)cellPos.y - 1].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();
        if (adjacent[3]) ((GameObject)Init.Instance.grid.gridArray[(int)cellPos.x - 1, (int)cellPos.y].Values["structure"])
                .transform.GetComponent<WallConnectors>().updateConnectors();


    }

    private void GetAdjacentWalls() // Checks if there are walls in all 4 cardinal directions
    {
        adjacent = new bool[4];
        Vector2 cellPos = gameObject.GetComponent<Wall>().gridPos;
        if (Init.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y + 1) != null && Init.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y + 1).CompareTag("Wall"))
            adjacent[0] = true;
        if (Init.Instance.grid.GetStructureAtCell((int)cellPos.x + 1, (int)cellPos.y) != null && Init.Instance.grid.GetStructureAtCell((int)cellPos.x + 1, (int)cellPos.y).CompareTag("Wall"))
            adjacent[1] = true;
        if (Init.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y - 1) != null && Init.Instance.grid.GetStructureAtCell((int)cellPos.x, (int)cellPos.y - 1).CompareTag("Wall"))
            adjacent[2] = true;
        if (Init.Instance.grid.GetStructureAtCell((int)cellPos.x - 1, (int)cellPos.y) != null && Init.Instance.grid.GetStructureAtCell((int)cellPos.x - 1, (int)cellPos.y).CompareTag("Wall"))
            adjacent[3] = true;
    }


    public void updateConnectors() // Destroys all visual parts of the wall and then rebuilds appropriate ones
    {
        GetAdjacentWalls();

        Destroy(center.gameObject);
        Destroy(connectorNS.gameObject);
        Destroy(connectorEW.gameObject);

        if (gameObject.GetComponent<Wall>().destroyed) //Destroyed Sprites
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
        }
        else //Not Destroyed Sprites
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
        transform.GetComponent<Wall>().UpdateFindOccupiedSpace();
    }


    void Start()
    {
        updateAllConnectors();
    }
}
