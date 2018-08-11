using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject)), RequireComponent(typeof(SteamVR_TrackedController))]
public class PlayerHand : MonoBehaviour
{
	private SteamVR_TrackedController m_Controller;

	private IEquipableItem m_CurrentItem;
	private IEquipableItem m_CurrentPrompt;

	void Start ()
	{
		SteamVRController.TriggerClicked += OnTriggerClicked;
		SteamVRController.TriggerUnclicked += OnTriggerUnclicked;
		SteamVRController.Gripped += OnGripped;
	}
	
	public SteamVR_TrackedController SteamVRController
	{
		get
		{
			if (m_Controller == null)
				m_Controller = GetComponent<SteamVR_TrackedController>();
			return m_Controller;
		}
	}

	public Vector3 Velocity
	{
		get { return SteamVR_Controller.Input((int)m_Controller.controllerIndex).velocity; }
	}

	public IEquipableItem CurrentItem
	{
		get { return m_CurrentItem; }
		set
		{
			if (m_CurrentItem != null)
				m_CurrentItem.OnDrop();

			m_CurrentItem = value;

			if (m_CurrentItem != null)
				m_CurrentItem.OnEquip(this);
		}
	}

	private IEquipableItem CurrentPrompt
	{
		get { return m_CurrentPrompt; }
		set
		{
			if (m_CurrentPrompt != null)
				m_CurrentPrompt.OnEquipPrompt(this, false);

			m_CurrentPrompt = value;
			
			if (m_CurrentPrompt != null)
				m_CurrentPrompt.OnEquipPrompt(this, true);
		}
	}
	

	private void OnTriggerClicked(object sender, ClickedEventArgs e)
	{
		if (CurrentItem != null)
			CurrentItem.OnPrimaryButton(true);
	}

	private void OnTriggerUnclicked(object sender, ClickedEventArgs e)
	{
		if (CurrentItem != null)
			CurrentItem.OnPrimaryButton(false);
	}


	private void OnGripped(object sender, ClickedEventArgs e)
	{
		// Switch items
		if (CurrentPrompt != null)
		{
			CurrentItem = CurrentPrompt;
			CurrentPrompt = null;
		}

		// Drop current item
		else if (CurrentItem != null)
			CurrentItem = null;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Pickup"))
		{
			IEquipableItem item = other.gameObject.GetComponent<IEquipableItem>();
			if (item != null && !item.IsCurrentlyEquiped())
				CurrentPrompt = item;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Pickup"))
		{
			IEquipableItem item = other.gameObject.GetComponent<IEquipableItem>();
			if (item != null && CurrentPrompt == item)
				CurrentPrompt = null;
		}
	}

}
