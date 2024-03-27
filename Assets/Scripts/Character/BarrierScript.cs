using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    public GameObject hitEffectPrefab; // Prefab của hiệu ứng va cham 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnCollisionEnter(Collision collision)
    {
        // va cham vao target
        if (collision.gameObject.tag == Common.ballTag)
        {
            // play sound Hit
            InGameManager.instance.playSound(Common.SoundState.HitBarrier);

            Debug.Log("Collision Target Ball:" + collision.transform.position);
            GameObject hitEffect = Instantiate(hitEffectPrefab, collision.gameObject.transform.position, collision.gameObject.transform.rotation);

            // remove effect after 1s
            Destroy(hitEffect.gameObject, 1f);

        }
    }
}
