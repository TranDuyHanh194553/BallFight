using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStarter;

public class EnemyScript : MonoBehaviour
{
    public float moveSpeed = 1f;  // Speed of movement
    public float moveRangeY = 1f;
    public float initialY;
    public bool enablePingPong = false;


    public float flySpeed = 100000f;

    public GameObject hitEffectPrefab; // Prefab của hiệu ứng hit


    public Transform controlPropObject;


    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("enablePingPong:" + enablePingPong);
        Debug.Log("StateManager.Instance.State:" + StateManager.Instance.State);
        controlPropObject.Rotate(Vector3.up, flySpeed);


        if (StateManager.Instance.State == GameState.Playing && enablePingPong)
        {
            // Calculate the new position
            float newPositionY = initialY + (Mathf.PingPong(Time.time * moveSpeed, moveRangeY) - moveRangeY) / 2;

            // Update the GameObject's position
            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }


    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemy OnCollisionEnter:" + collision.gameObject.tag);
        // va cham vao target
        if (collision.gameObject.tag == Common.ballTag)
        {
            // play sound Hit
            InGameManager.instance.playSound(Common.SoundState.HitBarrier);

            Debug.Log("Collision Target Ball:" + collision.transform.position);
            GameObject hitEffect = Instantiate(hitEffectPrefab, collision.gameObject.transform.position, collision.gameObject.transform.rotation);

            // remove effect after 1s
            Destroy(hitEffect.gameObject, 1f);


            // edit enemy mass 100 -> 1 cho nhe, tac dong 1 luc vao qua bong
            Rigidbody gameObjectsRigidBody = gameObject.GetComponent<Rigidbody>();
            if (gameObjectsRigidBody ==  null)
            {
                gameObjectsRigidBody = gameObject.AddComponent<Rigidbody>(); // Add the rigidbody.
                gameObjectsRigidBody.mass = 1;
            }

            gameObjectsRigidBody.AddForce(Vector3.forward * 22.0f, ForceMode.Impulse);

            // distroy enemy after 6s
            Destroy(gameObject, 6f);

       
        }
    }


}
