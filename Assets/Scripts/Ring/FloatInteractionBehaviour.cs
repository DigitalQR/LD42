using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatInteractionBehaviour : MonoBehaviour, IRingInteractableBehaviour
{
	[Header("Targeting")]

	[SerializeField]
	private Transform Target;

	[SerializeField]
	private Vector3 InitialisingOffset = Vector3.up * 3.0f;
	[SerializeField]
	private float InitalisationDuration = 3.0f;

	[Header("Movement")]

	[SerializeField]
	private float Acceleration = 10.0f;
	//[SerializeField]
	//private float MaxSpeed = 100.0f;
	[SerializeField]
	private float DistanceCutOff = 10.0f;


	private Rigidbody m_Body;
	private bool m_IsTrackingTarget;

	private Vector3 m_InitialisationTarget;
	private float m_InteractionTimer;
	private float m_KnownMaxSpeed;


	void Start ()
	{
		// TODO - Get player head transform	
		m_Body = GetComponent<Rigidbody>();
		m_Body.Sleep();

		m_InitialisationTarget = transform.position + InitialisingOffset;
	}

	public Vector3 TargetLocation
	{
		get
		{
			if (IsInitalising)
				return m_InitialisationTarget;
			else
				return Target != null ? Target.position : Vector3.zero;
		}
	}

	public bool IsInitalising
	{
		get { return m_InteractionTimer < InitalisationDuration; }
	}


	void Update ()
	{
		if (m_IsTrackingTarget)
		{
			m_InteractionTimer += Time.deltaTime;

			Vector3 targetOffset = TargetLocation - transform.position;

			// Accellerate towards target but only if withing acceptable range
			if (IsInitalising || targetOffset.sqrMagnitude >= DistanceCutOff * DistanceCutOff)
			{
				m_Body.velocity += targetOffset.normalized * Acceleration * Time.deltaTime;
				//m_Body.velocity += targetOffset.normalized * Acceleration * m_KnownMaxSpeed * 0.5f * Time.deltaTime;

				//float speed = m_Body.velocity.magnitude;
				//if (speed > m_KnownMaxSpeed)
				//	m_KnownMaxSpeed = Mathf.Min(speed, MaxSpeed);

				//m_Body.velocity = m_Body.velocity.normalized * m_KnownMaxSpeed;
			}
		}
	}

	public void OnRingInteraction()
	{
		m_IsTrackingTarget = true;
		m_InteractionTimer = 0.0f;
	}
}
