using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class egg : MonoBehaviour
{
    public Transform player;
    public AudioSource audio;
    public SpriteRenderer playerRender;
    private bool inEgg = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(inEgg == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                player.position = new Vector3(344, 141, 0);
                audio.mute = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inEgg = true;
                Debug.Log("EGG!!");
                player.position = new Vector3(-261, 246, 0);
                audio.mute = true;
                playerRender.color = Color.magenta;
            }
        }

    }
}
