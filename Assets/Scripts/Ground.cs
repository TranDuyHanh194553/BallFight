using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{   
    public GameObject groundBarrier;
    private Rigidbody groundButtonRb; 
    // Start is called before the first frame update
    void Start()
    {
        groundButtonRb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Barrier activate button
    private void OnCollisionEnter(Collision collision){
        
    }
}
