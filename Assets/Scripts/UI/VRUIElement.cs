using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class VRUIElement : MonoBehaviour
{
	private BoxCollider m_Collider;
	private RectTransform m_RectTransform;


	private void OnEnable()
	{
		ValidateCollider();
	}

	private void OnValidate()
	{
		ValidateCollider();
	}

	private void ValidateCollider()
	{
		m_RectTransform = GetComponent<RectTransform>();

		m_Collider = GetComponent<BoxCollider>();
		if (m_Collider == null)
			m_Collider = gameObject.AddComponent<BoxCollider>();

		m_Collider.size = m_RectTransform.sizeDelta;
	}
}
