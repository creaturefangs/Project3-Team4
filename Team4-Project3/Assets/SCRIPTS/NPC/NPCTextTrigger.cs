using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextTrigger : MonoBehaviour
{
    public Dialogue dialogueComp;
    public GameObject Textbox;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PLAYER")
        {
            Textbox.SetActive(true);
            dialogueComp.PlayerIsNearTrue();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "PLAYER")
        {
            Textbox.SetActive(false);
            dialogueComp.PlayerIsNearFalse();
        }
    }
}
