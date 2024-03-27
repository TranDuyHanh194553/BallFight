using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraScript : MonoBehaviour
{
    private Vector3 _offset;
    private Vector3 startPos;
    private Vector3 targetPosition;
    [SerializeField]
    private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;



    private void Awake()
    {
        startPos = new Vector3(0.703f, 0.127f, -4.45f);
        _offset = transform.position - startPos;
    }


    private void LateUpdate()
    {


        if (InGameManager.instance.ballInstance.transform.position.z > 15)
        {
            Debug.Log("restart Camera");
            targetPosition = startPos + _offset;
            transform.position = targetPosition;
            return;
         }
       
         targetPosition = InGameManager.instance.ballInstance.transform.position + _offset;
         transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
