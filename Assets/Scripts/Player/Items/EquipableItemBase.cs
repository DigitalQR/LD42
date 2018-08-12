using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SoundEmitter))]
public class EquipableItemBase : MonoBehaviour, IEquipableItem
{
	[Header("Equip Settings")]
	[SerializeField]
	private Vector3 EquipEulerRotation = new Vector3(45.0f, 0.0f, 0.0f);
	[SerializeField]
	private bool DisableCollision = true;

	[Header("Sound")]
	[SerializeField]
	private AudioClip EquipSound;
	[SerializeField]
	private AudioClip DropSound;

	private Rigidbody m_Body;
	private PlayerHand m_AttachedHand;

	private SoundEmitter m_SoundEmitter;

	protected virtual void Start()
	{
		m_SoundEmitter = GetComponent<SoundEmitter>();
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

	public SoundEmitter SoundSource
	{
		get { return m_SoundEmitter; }
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

		if(DropSound != null)
			m_SoundEmitter.PlaySound(DropSound);
	}

	public virtual void OnEquip(PlayerHand hand)
	{
		m_AttachedHand = hand;

		transform.parent = hand.transform;
		transform.localRotation = Quaternion.Euler(EquipEulerRotation);
		transform.localPosition = Vector3.zero;

		Body.detectCollisions = !DisableCollision;
		Body.isKinematic = true;

		if (EquipSound != null)
			m_SoundEmitter.PlaySound(EquipSound);
	}

	public virtual void OnEquipPrompt(PlayerHand hand, bool isOpen)
	{
	}
}
