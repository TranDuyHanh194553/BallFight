using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public FixedJoystick joystick;
    // Status
    private Rigidbody playerRb;
    public float speed = 5.0f; 
    public float playerWeight = 5.0f;
    private GameObject focalPoint;
    public bool hasPowerup;
    public bool hasSpeedup;
    public bool hasDisArm;
    public bool isALive;
    private float powerupStrength = 15.0f;
    //Ground Barrier
    public Rigidbody groundBarrierRB;
    //Buff and debuff
    [SerializeField] private GameObject powerupIndicator, speedupIndicator, speedupParticle;
    [SerializeField] private GameObject snow, disArm, slowDown; 
    //Charge
    public float minForce = 10f; 
    public float maxForce = 30f; 
    public float chargeRate = 5.0f; 
    private float currentForce; 
    private bool isCharging = false;
    //Ground barrier
    private GameObject groundBarrier;
    private Vector3 gBStartPos, gBEndPos, playerScale;

    void Start()
    {   
        playerRb =  GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        groundBarrier = GameObject.FindWithTag("GroundBarrier");
        groundBarrierRB = groundBarrier.GetComponent<Rigidbody>();
        DisableRagdoll();

        gBStartPos = groundBarrier.transform.localPosition;
        gBEndPos = gBStartPos + new Vector3(0f, 0f, 2.2f);
        isALive = true;

    }

    // Update is called once per frame
    void Update()
    {   
        //Control
        //playerRb.velocity = new Vector3(joystick.Horizontal * speed * playerWeight, playerRb.velocity.y, joystick.Vertical * speed * playerWeight);
        
        playerRb.AddForce(focalPoint.transform.forward * speed * joystick.Vertical * playerWeight);
        playerRb.AddForce( Vector3.right * speed * joystick.Horizontal * playerWeight);

        // Interacting
        powerupIndicator.transform.position = transform.position + new Vector3(0f, 5f, 0f);
        speedupIndicator.transform.position = transform.position + new Vector3(0f, 6f, 0f);
        
        snow.transform.position = transform.position;
        disArm.transform.position = transform.position;
        slowDown.transform.position = transform.position;

        // Space to shoot
        if (Input.GetKeyDown(KeyCode.Space)){
            StartCharging();
        }

        if (Input.GetKey(KeyCode.Space) && isCharging){
            Charge();
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging){
            Shoot();
        }
    }
    
    //  Buff
    private void OnTriggerEnter(Collider other){        
        //PowerUp
        if (other.CompareTag("Powerup") && hasDisArm == false){
            hasPowerup = true;
            other.gameObject.SetActive(false);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        } 
        //SpeedUp
        if(other.CompareTag("Speedup")){
            hasSpeedup= true;
            speed = speed * 1.5f;
            other.gameObject.SetActive(false);
            StartCoroutine(SpeedupCountdownRoutine());
            speedupIndicator.gameObject.SetActive(true);
            speedupParticle.gameObject.SetActive(true);
        }
        
        
        if (other.CompareTag("Powerup") && hasDisArm == true)
        {
            hasPowerup = false;
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Barrier")){
            isALive = false;
        }

    }
  

    //Knockback
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("GroundBarrier") && hasPowerup){
            Rigidbody enemyRigidbody =  collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromPlayer * 2 * powerupStrength, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("Button") ){
            collision.gameObject.SetActive(false);
    //Barrier Up
            EnableRagdoll();
            groundBarrier.transform.localPosition = Vector3.Lerp (gBStartPos, gBEndPos, 4f);
        }
        
        if (collision.gameObject.CompareTag("BlueEnemy") ){
            Debug.Log("Frozen!");
            snow.gameObject.SetActive(true);         
            StartCoroutine(SnowCountdownRoutine()); 
            speed = speed - 5f;
        }

        if (collision.gameObject.CompareTag("OrangeEnemy") ){    
            Debug.Log("HasDisArm!");   
            hasPowerup = false;
            hasDisArm = true;
            StartCoroutine(disArmCountdownRoutine());
            disArm.gameObject.SetActive(true);
            powerupIndicator.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("GreenEnemy") ){
            Debug.Log("Slowed!");
            slowDown.gameObject.SetActive(true);         
            StartCoroutine(SlowDownCountdownRoutine()); 
            speed = speed - Time.deltaTime * 150f;
        }

    }

    //Ground Barrier envolved function
    void EnableRagdoll()
    {
        groundBarrierRB.isKinematic = false;
        groundBarrierRB.detectCollisions = true;
    }

    // Let animation control the rigidbody and ignore collisions.
    void DisableRagdoll()
    {
        groundBarrierRB.isKinematic = true;
        groundBarrierRB.detectCollisions = false;
    }

    //Buff Countdown
        IEnumerator PowerupCountdownRoutine(){
            yield return new WaitForSeconds(7);
            hasPowerup = false;
            powerupIndicator.gameObject.SetActive(false);
            Debug.Log("Cooled down!");
        }

        IEnumerator SpeedupCountdownRoutine(){
            yield return new WaitForSeconds(5);
            hasSpeedup = false;
            speedupIndicator.gameObject.SetActive(false);
            speedupParticle.gameObject.SetActive(false);
            Debug.Log("Speed down!");
        }


        //Debuff countdown
        IEnumerator SnowCountdownRoutine(){
            yield return new WaitForSeconds(1);
            snow.gameObject.SetActive(false);
            speed = speed + 5f;
            Debug.Log("Defrost!");
        }

        IEnumerator disArmCountdownRoutine(){
            yield return new WaitForSeconds(2.5f);
            hasDisArm =  false;
            disArm.gameObject.SetActive(false);
            Debug.Log("UnDisArm!"); 
        }

        IEnumerator SlowDownCountdownRoutine(){
            yield return new WaitForSeconds(3.5f);
            slowDown.gameObject.SetActive(false);
            speed = 5.0f;
            Debug.Log("UnSlowed!");
        }

    // Shooting with Space functions
        void StartCharging(){
            currentForce = minForce;
            isCharging = true;
        }

        void Charge(){
            if (currentForce < maxForce)
            {
                currentForce += chargeRate * Time.deltaTime;
            }
        }

        void Shoot(){
            playerRb.AddForce(transform.up * currentForce, ForceMode.Impulse);
            // Recharge
            isCharging = false;
            currentForce = minForce;
        }

    
}

