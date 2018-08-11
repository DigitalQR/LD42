using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[SerializeField]
	private float Speed = 10.0f;
	[SerializeField]
	private float LifeLength = 5.0f;

	private float m_LifeTimer = 0.0f;

	void Start ()
	{
		Rigidbody body = GetComponent<Rigidbody>();
		body.velocity = transform.forward * Speed;

		m_LifeTimer = LifeLength;
	}

	private void Update()
	{
		m_LifeTimer -= Time.deltaTime;

		if (m_LifeTimer < 0.0f)
			Destroy(gameObject);
	}
}
