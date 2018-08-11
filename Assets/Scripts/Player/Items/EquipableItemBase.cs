using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EquipableItemBase : MonoBehaviour, IEquipableItem
{
	[SerializeField]
	private Quaternion EquipRotation = Quaternion.AngleAxis(-45.0f, Vector3.left);

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

	public abstract void OnPrimaryButton(bool isPressed);


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
		transform.localRotation = EquipRotation;
		transform.localPosition = Vector3.zero;

		Body.detectCollisions = false;
		Body.isKinematic = true;
	}

	public virtual void OnEquipPrompt(PlayerHand hand, bool isOpen)
	{
	}
}
