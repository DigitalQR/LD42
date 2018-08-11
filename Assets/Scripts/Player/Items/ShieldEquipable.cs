using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEquipable : EquipableItemBase
{
	[SerializeField]
	private float AppliedForce = 100.0f;

	private bool m_ShouldImpact;


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
		if (collision.gameObject.CompareTag("Interactable"))
		{
			Rigidbody body = collision.gameObject.GetComponent<Rigidbody>();
			if (body != null)
				body.velocity = transform.forward * AppliedForce;
		}
	}
}
