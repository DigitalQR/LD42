using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RingController : MonoBehaviour
{
	[SerializeField]
	private float StartRadius = 20.0f;
	[SerializeField]
	private float RingReducitonRate = 1.0f;
	[SerializeField]
	private bool ScaleWithRadius = true;

	private float m_Radius;
	private Vector3 m_StartScale;

	void Start()
	{
		m_Radius = StartRadius;
		m_StartScale = transform.localScale;
	}

	private float Radius
	{
		get { return m_Radius; }
	}

	private float NormalisedRadius
	{
		get { return m_Radius / StartRadius; }
	}

	void Update()
	{
		m_Radius -= RingReducitonRate * Time.deltaTime;

		// TODO - Notify
		if (m_Radius < 0.0f)
			m_Radius = 0.0f;

		if (ScaleWithRadius)
			transform.localScale = new Vector3(m_StartScale.x * NormalisedRadius, m_StartScale.y, m_StartScale.z * NormalisedRadius);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		DrawDebugRing();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		DrawDebugRing();
	}

	private void DrawDebugRing()
	{
		Gizmos.DrawWireSphere(transform.position, StartRadius);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Interactable"))
		{
			InteractableObject interactor = other.gameObject.GetComponent<InteractableObject>();
			if (interactor != null && !interactor.IsInteractable)
				interactor.SignalInteraction();
		}
	}
}
