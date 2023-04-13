using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MinionScript : MonoBehaviour
{
    private bool occupation = false;
    [SerializeField] private Structure jobLocation = null;
    [SerializeField] private Structure house = null;
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool moving;
    [SerializeField] private int currentPathIndex;
    [SerializeField] private bool atJob = false;
    [SerializeField] private bool atResource = false;
    [SerializeField] private bool atHouse = true;
    
    private Path _currentPath = null;
    private TimeController time;

    private void Start()
    {
        time = GameObject.Find("Time").GetComponent<TimeController>();
        
    }

    private void FixedUpdate()
    {
        if (!time.isNight && atHouse)
        {
            if (jobLocation != null)
            {
                ResetPath();
                Pathfind(jobLocation);
                atHouse = false;
                atJob = true;
            }
        }
        if (time.isNight && !atHouse)
        {
            if (house != null)
            {
                ResetPath();
                Pathfind(house);
                atJob = false;
                atResource = false;
                atHouse = true;
            }
        }
        if (_currentPath != null) TraversePath();
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



    public bool setOccupation(bool newOccupation)
    {
        occupation= newOccupation;
        return true;
    }
    public bool setJobLocation(Structure newJobLocation)
    {
        jobLocation = newJobLocation;
        Debug.Log(jobLocation.ToString());
        Debug.Log("Setting job location");
        return true;
    }

    public bool setHouse(Structure newHouse) 
    {
        house= newHouse;
        return true;
    }
}
