using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{   
    public float rotationSpeed;

    [SerializeField] private GameObject player;
    void Update()
    {
       if (player.transform.position.y > -1f){
         transform.position = player.transform.position + new Vector3(0f, 5f, 0f);
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
       }
    }
}
