using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
	private PlayerController m_Player;
	
	public PlayerController Player
	{
		get { return m_Player; }
		set { m_Player = value; }
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Interactable"))
			Player.OnHitByDamage(collision.gameObject);
	}
}
