using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameCollision : MonoBehaviour
{
    private Fishing fishing;
    private bool inCatchZone;

    public float force = 225f;

    // Start is called before the first frame update
    void Start()
    {
        fishing = transform.parent.parent.parent.gameObject.GetComponent<Fishing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fishing.inMinigame)
        {
            if (!inCatchZone) { fishing.catchTime -= Time.deltaTime; }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, force);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CatchZone"))
        {
            inCatchZone = true;
            other.gameObject.GetComponent<Image>().color = new Color32(59, 255, 89, 255);
        }
    }

    private void OnTriggerStay2D(Collider2D other) // If the needle is inside the catch-zone...
    {
        if (other.CompareTag("CatchZone") && fishing.inMinigame) { fishing.catchTime += Time.deltaTime; }
    }

    private void OnTriggerExit2D(Collider2D other) // If the needle is outside the catch-zone...
    {
        if (other.CompareTag("CatchZone"))
        {
            inCatchZone = false;
            other.gameObject.GetComponent<Image>().color = new Color32(253, 255, 100, 255);
        }
    }
}
