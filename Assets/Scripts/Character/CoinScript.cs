using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public GameObject getCoinEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        // va cham vao coin
        if (collision.CompareTag(Common.ballTag))
        {
            // play sound get coin
            InGameManager.instance.playSound(Common.SoundState.GetCoin);


            // play effect
            GameObject hitEffect = Instantiate(getCoinEffectPrefab, transform.position, transform.rotation);

            // remove effect after 1s
            Destroy(hitEffect.gameObject, 1f);


            Debug.Log("Collision Coin!!");
            Destroy(gameObject);
            InGameManager.instance.AddBonusScore();
        }
    }

    
}
