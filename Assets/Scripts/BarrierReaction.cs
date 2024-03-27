using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierReaction : MonoBehaviour
{   
    public int count = 2;
    private GameManager gameManager;
    private GameObject player;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
     private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            gameManager.GameOver();
            Debug.Log("Lose");
            Destroy(other.gameObject);
  
        }

        if (other.CompareTag("Enemy") || other.CompareTag("GroundBarrier") ){
            Debug.Log("-1 enemy");
            Destroy(other.gameObject);
            count = count -1;
        }
    }
}
