﻿using UnityEngine;
using System.Collections;

namespace BallThrowController
{
	public class Ball : MonoBehaviour
	{
		//set position and rotation
		private Vector3 initialPosition;
		private Quaternion initialRotation;
		private bool isWind = false;
		private bool isNormalThrow = false;
		private bool isRotateLeft = true;

		//setup floats
		private float curveWind = 1f;
		private float curveForce = 0f;
		public float objectScale = 1.0f;
		private float throwPos = 0f;

		private Transform childBall;
		private bool isCurve = false;
		private Rigidbody rigid;
		private bool ballThrowed = false;
		public Vector3 initialScale;
		private IEnumerator jumpCoroutine;
		private bool isRotate = false;
		public Camera cam;



		void OnEnable()
		{
			//set default position when object enable
			initialPosition = transform.position;
		}

		void Start()
		{
			rigid = GetComponent<Rigidbody>();
			//initalize gradually jump coroutine
			jumpCoroutine = JumpAround(0.3f);
			childBall = transform.Find("Ball");
			StartCoroutine(jumpCoroutine);

			//set initial scale, 
			initialScale = transform.localScale;

			// if no cam attached, use the default camera
			if (cam == null)
				cam = Camera.main;
		}

		void FixedUpdate()
		{
			//apply wind after certain distance
			float dist = 0f;

			//if throw is cureve then apply wind
			if (isCurve && dist <= (throwPos - (throwPos / 9.5f)))
			{
				if (!isWind)
					StartCoroutine(wind(0.5f));
				transform.Translate(Vector3.right * -curveForce * curveWind * Time.deltaTime);
			}
		}



		private void isThrow(bool flag)
		{
			if (jumpCoroutine != null)
			{
				StopCoroutine(jumpCoroutine);
				jumpCoroutine = null;
			}
			ballThrowed = flag;
			if (ThrowBallController.Instance.IsGettingDirection)
				isRotate = false;
			else
				isRotate = true;
		}


		//Resets the ball.
		public void ResetBall()
		{
			ballThrowed = false;
			StopAllCoroutines();
			//ball move to initial position
			StartCoroutine(GobackToDefaultPosition(0.3f));
		}

		//Sets the curve.
		private void SetCurve(float cFactore)
		{
			curveForce = cFactore;
			isCurve = true;
			Debug.Log("This is a Curve Throw");
		}


		//the wind logic
		IEnumerator wind(float t)
		{
			isWind = true;
			float rate = 1.0f / t;
			float i = 0f;
			while (i < 1.0f)
			{
				i += rate * Time.deltaTime;
				curveWind = Mathf.Lerp(1, 26, i);
				yield return 0;
			}
		}


		/// Winds the reverse.
		IEnumerator windReverse(float t)
		{
			isWind = true;
			float rate = 1.0f / t;
			float i = 0f;
			while (i < 1.0f)
			{
				i += rate * Time.deltaTime;
				curveWind = Mathf.Lerp(26, 0, i);
				yield return 0;
			}
		}

		//ball jumping animation
		IEnumerator JumpAround(float tm)
		{

			while (!ballThrowed)
			{

				yield return new WaitForSeconds(0.4f);

				transform.position = initialPosition;

				if (ThrowBallController.Instance.IsGameStart)
				{


					isRotateLeft = !isRotateLeft;
					isRotate = true;
					float i = 0f;
					float rate = 1.0f / tm;
					Vector3 from = initialPosition;
					Vector3 to = new Vector3(from.x, from.y + 0.05f, from.z);

					while (i < 1.0f)
					{
						i += rate * Time.deltaTime;
						transform.position = Vector3.Lerp(from, to, i);
						yield return 0f;
					}
					i = 0f;
					rate = 1.0f / (tm / 0.7f);

					Vector3 bump = from;
					bump.y -= 0.05f;

					while (i < 1.0f)
					{
						i += rate * Time.deltaTime;
						transform.position = Vector3.Lerp(to, bump, i);
						yield return 0f;
					}

					isRotate = false;

					i = 0f;
					rate = 1.0f / (tm / 1.1f);

					while (i < 1.0f)
					{
						i += rate * Time.deltaTime;
						transform.position = Vector3.Lerp(bump, from, i);
						yield return 0f;
					}

				}
			}
		}


		//The back to initial position
		IEnumerator GobackToDefaultPosition(float tm)
		{
			float i = 0f;
			float rate = 1.0f / tm;
			Vector3 from = transform.position;
			Vector3 to = initialPosition;
			while (i < 1.0f)
			{
				i += rate * Time.deltaTime;
				transform.position = Vector3.Lerp(from, to, i);
				yield return 0f;
			}
			transform.position = initialPosition;
			childBall.localRotation = Quaternion.identity;
			isRotate = false;
			jumpCoroutine = JumpAround(0.3f);

			StartCoroutine(jumpCoroutine);
		}

		//when ball hit something - or target
		void OnCollisionEnter(Collision obj)
		{
			if (obj.gameObject.CompareTag("target"))
			{
				Debug.Log("Nice hit!");
				Destroy(gameObject);
			}

		}

	}

}

