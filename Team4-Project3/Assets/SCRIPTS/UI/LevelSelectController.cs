using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectController : MonoBehaviour
{

    public GameObject levelselectUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelSelect()
    {
        
        

            levelselectUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
         
    }
}
