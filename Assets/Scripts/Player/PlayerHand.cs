using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(SteamVR_LaserPointer)), RequireComponent(typeof(SteamVR_TrackedController))]
public class PlayerHand : MonoBehaviour
{
	private SteamVR_TrackedController m_Controller;
	private SteamVR_LaserPointer m_LaserPointer;

	private bool m_InUI = false;
	private IEquipableItem m_CurrentItem;
	private IEquipableItem m_CurrentPrompt;

	void Start ()
	{
		m_LaserPointer = GetComponent<SteamVR_LaserPointer>();
		m_LaserPointer.PointerIn += OnLaserPointerIn;
		m_LaserPointer.PointerOut += OnLaserPointerOut;

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
		if (m_InUI)
		{
			if(EventSystem.current.currentSelectedGameObject != null)
				ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
		else
		{
			if (CurrentItem != null)
				CurrentItem.OnPrimaryButton(true);
		}
	}

	private void OnTriggerUnclicked(object sender, ClickedEventArgs e)
	{
		if (m_InUI)
		{

		}
		else
		{
			if (CurrentItem != null)
				CurrentItem.OnPrimaryButton(false);
		}
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

	private void OnLaserPointerIn(object sender, PointerEventArgs e)
	{
		Selectable selectable = e.target.GetComponent<Selectable>();

		if (selectable != null)
		{
			m_InUI = true;
			selectable.Select();
			m_LaserPointer.pointer.SetActive(true);
		}
	}

	private void OnLaserPointerOut(object sender, PointerEventArgs e)
	{
		m_LaserPointer.pointer.SetActive(false);
		m_InUI = false;

		EventSystem.current.SetSelectedGameObject(null);
	}

}
