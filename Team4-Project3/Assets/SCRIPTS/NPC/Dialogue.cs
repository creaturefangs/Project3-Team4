using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    public bool playerIsNear = false;

    public bool updateCheck = false;

    private int index;

    public AudioSource voice;
     
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsNear == true && updateCheck == true)
        {
            StartDialogue();
            updateCheck = false;
        }
        
        if(Input.GetMouseButtonDown(0) && playerIsNear == true)
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
                voice.Stop();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];  
            }
        }   
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    public IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            voice.Play();
            yield return new WaitForSeconds(textSpeed);
            voice.Stop();
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            voice.Stop();
            StartCoroutine(TypeLine());
        }
        else
        {
            index = 0;
            StartDialogue();
            //gameObject.SetActive(false);
        }
    }

    public void PlayerIsNearTrue()
    {
        updateCheck = true;
        playerIsNear = true;
    }

    public void PlayerIsNearFalse()
    {
        updateCheck = false;
        playerIsNear = false;
        StopCoroutine(TypeLine());
    }
}
