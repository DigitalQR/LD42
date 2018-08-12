using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EquipableItemBase : MonoBehaviour, IEquipableItem
{
	[SerializeField]
	private Vector3 EquipEulerRotation = new Vector3(45.0f, 0.0f, 0.0f);
	[SerializeField]
	private bool DisableCollision = true;

	private Rigidbody m_Body;
	private PlayerHand m_AttachedHand;

	protected virtual void Start()
	{

	}

	public Rigidbody Body
	{
		get
		{
			if (m_Body == null)
				m_Body = GetComponent<Rigidbody>();
			return m_Body;
		}
	}

	public PlayerHand AttachedHand
	{
		get { return m_AttachedHand; }
	}

	public virtual void OnPrimaryButton(bool isPressed)
	{
	}

	public bool IsCurrentlyEquiped()
	{
		return AttachedHand != null;
	}

	public virtual void OnDrop()
	{
		transform.parent = null;

		Body.detectCollisions = true;
		Body.isKinematic = false;
		Body.velocity = m_AttachedHand.Velocity;

		m_AttachedHand = null;
	}

	public virtual void OnEquip(PlayerHand hand)
	{
		m_AttachedHand = hand;

		transform.parent = hand.transform;
		transform.localRotation = Quaternion.Euler(EquipEulerRotation);
		transform.localPosition = Vector3.zero;

		Body.detectCollisions = !DisableCollision;
		Body.isKinematic = true;
	}

	public virtual void OnEquipPrompt(PlayerHand hand, bool isOpen)
	{
	}
}
