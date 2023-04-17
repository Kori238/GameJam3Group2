using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class resourcePopUp : MonoBehaviour
{

    [SerializeField] Image resourceImage;
    [SerializeField] Sprite woodimage;
    [SerializeField] Sprite stoneimage;
    [SerializeField] TextMeshProUGUI text;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Vector3 temp = transform.position;
        transform.position=new Vector3(temp.x, temp.y+1, temp.z);
        
        
    }
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
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
