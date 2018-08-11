using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField]
	private float Speed = 10.0f;
	[SerializeField]
	private float LifeLength = 5.0f;

	[SerializeField]
	private float AppliedForce = 100.0f;

	private float m_LifeTimer = 0.0f;
	

	void Update()
	{
		m_LifeTimer += Time.deltaTime;

		// Step
		Vector3 oldPos = transform.position;
		Vector3 newPos = oldPos + transform.forward * Speed * Time.deltaTime;

		// Death check
		if (m_LifeTimer > LifeLength || CheckForCollision(oldPos, newPos))
			Destroy(gameObject);
		else
			transform.position = newPos;
	}

	private bool CheckForCollision(Vector3 oldPos, Vector3 newPos)
	{
		RaycastHit hit;
		Vector3 diff = newPos - oldPos;

		if (Physics.Raycast(oldPos, diff, out hit, diff.magnitude, Physics.DefaultRaycastLayers))
		{
			Rigidbody body = hit.collider.GetComponent<Rigidbody>();
			if (body != null)
				body.velocity = transform.forward * AppliedForce;
			
			return true;
		}
		else
			return false;
	}

}
