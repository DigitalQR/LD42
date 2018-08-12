using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RingController : MonoBehaviour
{
	[Header("Radius")]
	[SerializeField]
	private float StartRadius = 20.0f;
	[SerializeField]
	private float RingReducitonRate = 1.0f;
	[SerializeField]
	private bool ScaleWithRadius = true;

	[Header("Pass Settings")]
	[SerializeField]
	private float PassRadius = 2.5f;

	[SerializeField]
	private string NextLevel = "None";

	[SerializeField]
	private GameObject NonPassedMesh;
	[SerializeField]
	private GameObject PassedMesh;

	private bool m_IsActive = true;
	private bool m_WasPassed = false;
	private float m_Radius;



	void Start()
	{
		m_Radius = StartRadius;
		
		GameController.Main.OnLevelSwitch();
		PlayerController.Main.PlayerJumped += OnPlayerJump;

		NonPassedMesh.SetActive(true);
		PassedMesh.SetActive(true);

		if (ScaleWithRadius)
			transform.localScale = new Vector3(m_Radius, 1.0f, m_Radius);
	}

	void OnDestroy()
	{
		PlayerController.Main.PlayerJumped -= OnPlayerJump;
	}

	private float Radius
	{
		get { return m_Radius; }
	}

	private float NormalisedRadius
	{
		get { return m_Radius / StartRadius; }
	}

	public bool HasPassed
	{
		get { return m_Radius <= PassRadius; }
	}

	void Update()
	{
		if (m_IsActive)
		{
			m_Radius -= RingReducitonRate * Time.deltaTime;
			
			if (m_Radius < 0.0f)
			{
				m_Radius = 0.0f;
				m_IsActive = false;
			}
			
			if (!m_WasPassed && HasPassed)
			{
				PopupMessage msg = new PopupMessage("JUMP!", "", 5.0f);
				PopupController.Main.PushImmediate(msg);
			}

			NonPassedMesh.SetActive(!HasPassed);
			PassedMesh.SetActive(HasPassed);
			m_WasPassed = HasPassed;

			if (ScaleWithRadius)
				transform.localScale = new Vector3(m_Radius, 1.0f, m_Radius);
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

	private void OnPlayerJump(object sender, System.EventArgs e)
	{
		if (HasPassed)
			LevelController.Main.SwitchScene(NextLevel);
	}
}
