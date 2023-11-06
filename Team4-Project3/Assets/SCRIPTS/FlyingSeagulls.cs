using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSeagulls : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    public float m_Speed = 5.0f;
    public float waitTime = 10.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        //Fetch the Rigidbody component you attach from your GameObject
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public IEnumerator WaitForTenSeconds()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
        m_Rigidbody.velocity = transform.forward * m_Speed;
    }
}
