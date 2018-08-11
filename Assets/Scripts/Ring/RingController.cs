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

	[SerializeField]
	private string NextLevel = "None";

	private bool m_IsActive = true;
	private float m_Radius;
	private Vector3 m_StartScale;



	void Start()
	{
		m_Radius = StartRadius;
		m_StartScale = transform.localScale;

		LevelController.Main.SpawnPlayer();
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
		if (m_IsActive)
		{
			m_Radius -= RingReducitonRate * Time.deltaTime;

			// TODO - Notify
			if (m_Radius < 0.0f)
			{
				m_Radius = 0.0f;
				m_IsActive = false;
				LevelController.Main.SwitchScene(NextLevel);
			}

			if (ScaleWithRadius)
				transform.localScale = new Vector3(m_StartScale.x * NormalisedRadius, m_StartScale.y, m_StartScale.z * NormalisedRadius);
		}
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
			RingInteractable interactor = other.gameObject.GetComponent<RingInteractable>();
			if (interactor != null && !interactor.IsInteractable)
				interactor.SignalInteraction();
		}
	}
}
