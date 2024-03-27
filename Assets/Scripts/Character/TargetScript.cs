using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStarter;

public class TargetScript : MonoBehaviour
{

    public float moveSpeed = 1f;  // Speed of movement
    public float moveRangeX = 2f;
    public float initialX;
    public bool enablePingPong = false;

    public GameObject brokenEffectPrefab; // Prefab của hiệu ứng vỡ


    // Start is called before the first frame update
    void Start()
    {
        initialX = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("enablePingPong:" + enablePingPong);
        Debug.Log("StateManager.Instance.State:" + StateManager.Instance.State);

        if (StateManager.Instance.State == GameState.Playing && enablePingPong)
        {
            // Calculate the new position
            float newPositionX = initialX + (Mathf.PingPong(Time.time * moveSpeed, moveRangeX) - moveRangeX) / 2;

            // Update the GameObject's position
            transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
        }


    }



    private void OnTriggerEnter(Collider collision)
    {
        // va cham vao target
        if (collision.CompareTag(Common.ballTag))
        {
            // play sound Hit
            InGameManager.instance.playSound(Common.SoundState.HitTarget);

            Debug.Log("Collision Target Ball:" + collision.transform.position);
            GameObject hitEffect = Instantiate(brokenEffectPrefab, transform.position, transform.rotation);


            // remove effect after 1s
            Destroy(hitEffect.gameObject, 1f);

            Destroy(gameObject);
            InGameManager.instance.DecreaseTargetHit();

        }
    }



}
