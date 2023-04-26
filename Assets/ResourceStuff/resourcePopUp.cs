using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class resourcePopUp : MonoBehaviour
{

    [SerializeField] Image resourceImage;
    [SerializeField] Sprite woodimage;
    [SerializeField] Sprite stoneimage;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float floatTime;
    private Vector3 startPos;
    private Vector3 endPos;
    private float currentTime=0;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
        startPos=transform.position;
        endPos= new Vector3(startPos.x, startPos.y + 50, startPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        currentTime = currentTime +Time.deltaTime;
       transform.position = Vector3.Lerp(startPos, endPos, currentTime/floatTime);
        
        
    }
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(floatTime);
        Destroy(gameObject);
    }


    public void setImage(string ResourceType)
    {
        if(ResourceType == "wood")
        {
            resourceImage.sprite = woodimage;
        }
        else { resourceImage.sprite = stoneimage; }
    }
    public void setText(string newtext)
    {
        text.SetText(newtext);
    }
}
