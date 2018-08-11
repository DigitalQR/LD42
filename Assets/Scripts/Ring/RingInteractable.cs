using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingInteractable : MonoBehaviour
{
	private IRingInteractableBehaviour[] m_Interactions;
	private bool m_IsInteractable = false;


	void Start()
	{
		m_Interactions = GetComponents<IRingInteractableBehaviour>();
	}

	public bool IsInteractable
	{
		get { return m_IsInteractable; }
	}

	public void SignalInteraction()
	{
		if (!m_IsInteractable)
		{
			foreach (IRingInteractableBehaviour behaviour in m_Interactions)
				behaviour.OnRingInteraction();
			m_IsInteractable = true;
		}
	}
}
