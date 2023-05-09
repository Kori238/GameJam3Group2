using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MinionScript : MonoBehaviour
{
   
    [SerializeField] private Structure jobLocation = null;
    private WoodCollectorScript jobScript;
    [SerializeField] private Structure house = null;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float waitTime = 10f;
    [SerializeField] private bool moving;
    [SerializeField] private int currentPathIndex;
    [SerializeField] private bool atJob = false;
    [SerializeField] private bool atResource = false;
    [SerializeField] private bool atHouse = true;
    [SerializeField] private bool goToResource = false;
    private int collectionAmount;
    private string ResourceType;
    [SerializeField] private GameObject woodPU;
    
    private Path _currentPath = null;
    private TimeController time;

    private void Start()
    {
        time = GameObject.Find("Time").GetComponent<TimeController>();
       
    }

    private void FixedUpdate()
    {
        if (time.hourTime == 6 &&time.minutesTime==00)
        {
            goToResource = false;
        }

        if (_currentPath != null) { TraversePath(); return; }
        if ((!time.isNight && atHouse)|| atResource && !goToResource) // moves to resource collector 
        {
            if (jobLocation != null)
            {
                ResetPath();
                Pathfind(jobLocation);
                atHouse = false;
                atJob = true;
                atResource= false;
                StartCoroutine(WaitAt());
            }
        }
        if(!time.isNight && atJob && goToResource) // move to local resource
        {
            ResetPath();
            if(jobLocation!= null &&jobScript.getLocalTreeLength()>0) 
            {
                Structure temp =jobScript.GetLocalTree();
               
                    Pathfind(temp);
                    atHouse = false;
                    atJob = false;
                    atResource = true;
                    StartCoroutine(WaitAt());
               
            }

            else { goToResource = false; }

        }
        if (time.isNight && !atHouse) // move to house at night
        {
           
            
                ResetPath();
                Pathfind(house);
                atJob = false;
                atResource = false;
                atHouse = true;
            
        }
        
    }

    private IEnumerator WaitAt()// time before moving again between collector and resource
    {
        while (moving)
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(waitTime);//Random.Range(waitTime/2,waitTime));
        goToResource = !goToResource;
        if (atResource)
        {
            Debug.Log("add resource");
            if (ResourceType == "wood")
            {
                Init.Instance.resourceManager.AddWood(collectionAmount);
                Vector3 location = transform.position;
                GameObject PopUpInstance = Instantiate(woodPU, location, quaternion.identity);
                resourcePopUp instanceScript = PopUpInstance.GetComponent<resourcePopUp>();
                instanceScript.setImage("wood");
                instanceScript.setText("+" + collectionAmount);

            }
            else
            {
                Init.Instance.resourceManager.AddStone(collectionAmount); Debug.Log("stone");
                Vector3 location = transform.position;
                GameObject PopUpInstance = Instantiate(woodPU, location, quaternion.identity);
                resourcePopUp instanceScript = PopUpInstance.GetComponent<resourcePopUp>();
                instanceScript.setImage("stone");
                instanceScript.setText("+" + collectionAmount);
            }

        }

    }





    private void Pathfind(Structure destination)
    {
        float point = Random.Range(0, destination.collectionPoints.Count);
        var minionPos = Init.Instance.pathfinding.GetGrid().GetWorldCellPosition(transform.position.x, transform.position.y);
        Node node = destination.collectionPoints[(int)point];
        _currentPath = Init.Instance.pathfinding.FindPath((int)minionPos.x, (int)minionPos.y, node.x, node.y, isMinion: true);
        moving = true;
    }

    private void ResetPath()
    {
        moving = false;
        _currentPath = null;
        currentPathIndex = 0;
    }

    private void TraversePath()
    {
        if (currentPathIndex >= _currentPath.nodes.Count)
        {
            moving = false;
            _currentPath = null;
            currentPathIndex = 0;
        }
        if (!moving || _currentPath == null || _currentPath.nodes.Count <= 0) return;
        var targetNode = _currentPath.nodes[currentPathIndex];
        var targetPosition =
            new Vector3((targetNode.x + 0.5f) * 10 / 3, (targetNode.y + 0.5f) * 10 / 3, transform.position.z);
        if (Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            var position = transform.position;
            var moveDir = (targetPosition - position).normalized;
            position += speed * Time.deltaTime * moveDir;
            transform.position = position;
            
        }
        else
        {
            currentPathIndex++;
            if (currentPathIndex >= _currentPath.nodes.Count)
            {
                moving = false;
                _currentPath = null;
                currentPathIndex = 0;
            }
        }
    }



    
    public bool setJobLocation(Structure newJobLocation)
    {
        atJob = false;
        jobLocation = newJobLocation;
        if(jobLocation != null) 
        {
        
        jobScript =newJobLocation.GetComponent<WoodCollectorScript>();
        ResourceType = jobScript.getResourceType();
        
        }
        else {
            ResetPath();
            Pathfind(house);
            atHouse = true;
            atResource = false;
        }
        return true;
    }

    public bool setHouse(Structure newHouse) 
    {
        house= newHouse;
        
        Init.Instance.resourceManager.addSingleMinionToList(this);
        return true;
    }
    public void setCollectionAmount(int newAmount) { collectionAmount = newAmount; }
    public void deHouse()
    {
        if(jobLocation!=null) { jobLocation.GetComponent<WoodCollectorScript>().unassignDeadMinion(this);  }
        Init.Instance.resourceManager.removeFromAvailibleMinionList(this);
        Destroy(gameObject);
    }
}
