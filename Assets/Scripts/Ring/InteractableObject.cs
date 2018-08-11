using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
	private IInteractableBehaviour[] m_Interactions;
	private bool m_IsInteractable = false;


	void Start()
	{
		m_Interactions = GetComponents<IInteractableBehaviour>();
	}

	public bool IsInteractable
	{
		get { return m_IsInteractable; }
	}

	public void SignalInteraction()
	{
		if (!m_IsInteractable)
		{
			foreach (IInteractableBehaviour behaviour in m_Interactions)
				behaviour.OnInteraction();
			m_IsInteractable = true;
		}
	}
}
