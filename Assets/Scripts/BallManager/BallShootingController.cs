using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GameStarter;

namespace BallManager
{
    public class BallShootingController: MonoBehaviour
    {

        //public Rigidbody actualBall;
        
        public LineRenderer linePrefab;
        
        //how fast to moving by Z axis
        public float zVelocity = 50f;
        
        //moving up by Y axis
        public float factorUpConstant = 45;

        //moving down by Y axis
        public float factorDownConstant;

        // moving by X axis
        public float factorLeftRightConstant = 50;


        // moving by Z axis
        public float factorForwardConstant = 30;


        //allow control ball after shoot in limit distance
        public float ballControlLimit = 30f;

        public float minSwipeConstant = 10f;

        //shoot ball in second
        public float shootTime = 1f; 
        
        private bool _enableTouch = true;
        private bool _isRecord; //default false

        private bool _isShoot;
        
        private Vector3 _beginPos;
        private Vector3 _prePos, _curPos;
        
        private Rigidbody _ballRb;
        private Transform _ballParent;
        
        //ball position at init stage
        private Vector3 _ballInitPosition;
        
        //ball position at start shoot
        private Vector3 _ballStartPosition;
        
        //calculate moving up with screen height
        private float _factorUp;

        //calculate moving down with screen height
        private float _factorDown;

        //calculate moving by X axis with screen width
        private float _factorLeftRight;


        //calculate moving by Z axis with screen width
        private float _factorForward;


        //swipe min distance
        private float _minSwipeDistance;


        
        
        private LineRenderer currentTrail = null;
        private Queue<Vector3> pointList = new Queue<Vector3>();
        private Queue<Vector3> velocityList = new Queue<Vector3>();
        private Queue<Vector3> angularVelocityList = new Queue<Vector3>();

        void Start()
        {
            _ballRb = GetComponent<Rigidbody>();
            _ballParent = transform.parent;
            _ballInitPosition = transform.position - _ballParent.position;
            CalculateFactors();
        }
        void Update()
        {
            if (StateManager.Instance.State != GameState.Playing) return;

            
            if (!_enableTouch) return;
            
            // Track a single touch as a direction control.
            if (Input.GetMouseButtonDown(0))
            {
                MouseBegin(Input.mousePosition);
            }
            else if(Input.GetMouseButton(0))
            {
                MouseMoving(Input.mousePosition);
            }else if (Input.GetMouseButtonUp(0))
            {
                MouseEnd();
            }

         

        }

        private void MouseBegin(Vector3 pos)
        {
            _prePos = _curPos = _beginPos = pos;
            _ballStartPosition = transform.position;
            // CreateNewLine();
        }

        private void MouseEnd()
        {
            Debug.Log("mouse end _isRecord: " + _isRecord);
            if (_isRecord)
            {
                _isRecord = false;
                StartShoot();
                StartCoroutine(ResetCoroutine());
                DisableTouch();
            }
        }

        private void StartShoot()
        {
            Debug.Log("StartShoot");
            Debug.Log("Start velocityList.Count: " + velocityList.Count);
            _isShoot = true;
        }

        private void DoShoot()
        {
            if (pointList.Count > 0)
            {
                //actualBall.transform.position = pointList.Dequeue();
                //actualBall.velocity = velocityList.Dequeue();
                //actualBall.angularVelocity = angularVelocityList.Dequeue();
            }
        }

        private void MouseMoving(Vector3 pos)
        {
            if (_curPos == pos)
            {
                return;
            }

            _prePos = _curPos;
            _curPos = pos;
            


            Vector3 distance = _curPos - _beginPos;
            
            Vector3 ballDistance = transform.position - _ballStartPosition;
            
            if (_isRecord == false )
            {
                //start shoot the ball
                StartRecord(distance);
            }
            else  if (ballDistance.magnitude < ballControlLimit)
            {                
                //ball is shooting
                //allow control ball in ballControlLimit
                //update ball velocity between current position and previous position
                
                DoRecord(_curPos - _prePos);
                AddPoint();
            }
            
          
        }
        
        IEnumerator ResetCoroutine()
        {
            //Reset ball position after shootTime params
            yield return new WaitForSeconds(shootTime);
            Reset();
            EnableTouch();
        }
        
        private void EnableTouch()
        {
            _enableTouch = true;
        }

        private void DisableTouch()
        {
            StartCoroutine(_disableTouch());
        }

        private IEnumerator _disableTouch()
        {
            yield return new WaitForEndOfFrame();
            _enableTouch = false;
        }
        
        private void CalculateFactors()
        {
            _minSwipeDistance = (minSwipeConstant * Screen.height) / 960f;
            Debug.Log("_minSwipeDistance: " + _minSwipeDistance);
            
            _factorUp = factorUpConstant / Screen.height;
            _factorDown = factorDownConstant / Screen.height;
            _factorLeftRight = factorLeftRightConstant / Screen.width;
            _factorForward = factorForwardConstant / Screen.height;

            Debug.Log("_factorForward: " + _factorForward);
        }


        private void StartRecord(Vector3 distance)
        {
            // Vector3 distance = _curPos - _beginPos;
            
            if (distance.y > 0 && distance.magnitude > _minSwipeDistance)
            {
                Debug.Log("StartRecord");
                _isRecord = true;
                //update ball velocity between current position and begin shoot position
                DoRecord(distance);
                //CreateNewLine();
            }
        }
        
        private void DoRecord(Vector3 distance)
        {
            // Vector3 distance = _curPos - _prePos;

            Debug.Log("zVelocity:" + zVelocity + "-" + zVelocity * distance.y * _factorForward);

            //Moving the ball
            Vector3 speed = _ballParent.InverseTransformDirection (_ballRb.velocity);
            speed.y += distance.y * ((distance.y > 0) ? _factorUp : _factorDown);
            speed.x += distance.x * _factorLeftRight;
            speed.z = zVelocity;
            Debug.Log("speed: " + speed + "-" + Time.deltaTime);
            _ballRb.velocity = _ballParent.TransformDirection (speed);
            velocityList.Enqueue(_ballRb.velocity);

            //Rotate the ball
            speed = _ballRb.angularVelocity;
            speed.y += distance.x * _factorLeftRight;
            _ballRb.angularVelocity = speed;
            angularVelocityList.Enqueue(_ballRb.angularVelocity);
            
        }


        //Reset ball state and position
        private void Reset()
        {


            Debug.Log("Start Reset");
            _isRecord = false;
            _ballRb.velocity = Vector3.zero;
            _ballRb.angularVelocity = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.position = _ballParent.position + _ballInitPosition;


            _isShoot = false;
            //actualBall.velocity = Vector3.zero;
            //actualBall.angularVelocity = Vector3.zero;
            //actualBall.transform.localEulerAngles = Vector3.zero;
            
            //actualBall.transform.position = _ballParent.position + _ballInitPosition;
            Debug.Log("destroy current line render");
            Destroy(currentTrail);
            
            pointList.Clear();
            
            angularVelocityList.Clear();
            // angularVelocityList.Enqueue(Vector3.zero);
            
            Debug.Log("Clear velocityList " + velocityList.Count);
            velocityList.Clear();
            Debug.Log("End clear velocityList " + velocityList.Count);


            // decrease ball number avaiable
            InGameManager.instance.DecreaseNumberBall();
            // handle check state game
            InGameManager.instance.HandleEndGame();
        }
        
        private void CreateNewLine()
        {
            if (currentTrail is null || currentTrail.IsDestroyed())
            {
                Debug.Log("create new line render");
                currentTrail = Instantiate(linePrefab);
            }
            
        }
        
        private void UpdateLinePoints()
        {
            if(currentTrail != null && pointList.Count > 1)
            {
                currentTrail.positionCount = pointList.Count;
                currentTrail.SetPositions(pointList.ToArray());
            }
        }

        private void AddPoint()
        {
            pointList.Enqueue(transform.position);
            UpdateLinePoints();

        }
        
    
        
        
    }
}