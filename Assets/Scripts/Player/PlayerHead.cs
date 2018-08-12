using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour, IRingInteractableBehaviour
{
	private PlayerController m_Player;
	
	public PlayerController Player
	{
		get { return m_Player; }
		set { m_Player = value; }
	}

	public void OnRingInteraction()
	{
		// Kill player when they leave the ring
		Player.Health = 0;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Interactable"))
			Player.OnHitByDamage(collision.gameObject);
	}
}
