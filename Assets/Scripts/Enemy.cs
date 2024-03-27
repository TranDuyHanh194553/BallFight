using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    private Rigidbody enemyRb, playerRb;
    private GameObject player;
    private GameObject powerupIndicator;
    private GameManager gameManager;
    public float speed;
    private int pointValue = 5;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody>();;
        powerupIndicator = GameObject.FindWithTag("PowerupIndicator");
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {   
        if (transform.position == null){
            return;
        }
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")){
            Rigidbody enemyRigidbody =  collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromEnemy = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromEnemy * 7 , ForceMode.Impulse);
        }
    }
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Barrier")){
            Destroy(gameObject);
            Debug.Log("-1 enemy");
            gameManager.UpdateScore(pointValue);
            player.transform.localScale = player.transform.localScale * 1.05f; 
            playerRb.mass = playerRb.mass * 1.1f;
            powerupIndicator.transform.localScale =  powerupIndicator.transform.localScale * 1.2f; 
        }
    }    
}
