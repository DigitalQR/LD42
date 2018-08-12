using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEquipable : EquipableItemBase
{
	[SerializeField]
	private float AppliedForce = 25.0f;

	[SerializeField]
	private Transform VelocityTracker;

	[SerializeField]
	private float VelocityFactor = 0.0f;

	private bool m_ShouldImpact;
	private Vector3 m_LastPosition;
	private Vector3 m_Velocity;
	

	protected override void Start()
	{
		base.Start();

		if (VelocityTracker == null)
			VelocityTracker = transform;

		m_LastPosition = VelocityTracker.position;
	}

	private void Update()
	{
		Vector3 currentPostion = VelocityTracker.position;
		m_Velocity = (currentPostion - m_LastPosition) / Time.deltaTime;
		m_LastPosition = currentPostion;
		Debug.Log(m_Velocity);
	}

	public override void OnDrop()
	{
		m_ShouldImpact = false;
		base.OnDrop();
	}

	public override void OnEquip(PlayerHand hand)
	{
		m_ShouldImpact = true;
		base.OnEquip(hand);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (m_ShouldImpact && collision.gameObject.CompareTag("Interactable"))
		{
			Rigidbody body = collision.gameObject.GetComponent<Rigidbody>();
			if (body != null)
				body.velocity = transform.forward * AppliedForce + m_Velocity * VelocityFactor;
		}
	}
}
