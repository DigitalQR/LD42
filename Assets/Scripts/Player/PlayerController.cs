using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	/// <summary>
	/// PlayerController (single instance)
	/// </summary>
	public static PlayerController Main { get; private set; }

	[SerializeField]
	private GameObject HeadObject;

	[SerializeField]
	private PlayerHand LeftHand;
	[SerializeField]
	private PlayerHand RightHand;


	[SerializeField]
	private float JumpThreshold = 2.0f;
	[SerializeField]
	private float JumpDurationThreshold = 1.5f;

	private Vector3 m_PreviousHeadLocation;
	private float m_JumpTimer;
	private bool m_HasAscended;

	public event System.EventHandler PlayerJumped;

	void Awake()
	{
		if (Main != this)
		{
			if (Main != null)
				throw new UnityException("Multiple PlayerControllers active");
			else
			{
				Main = this;
				Debug.Log("PlayerController found");
			}
		}
	}

	void Start ()
	{
		LeftHand.SteamVRController.MenuButtonClicked += OnMenuClicked;
		RightHand.SteamVRController.MenuButtonClicked += OnMenuClicked;

		m_PreviousHeadLocation = HeadPosition.position;
	}
	
	public Transform HeadPosition
	{
		get { return HeadObject.transform; }
	}


	void Update ()
	{
		DetectJump();
	}

	private void OnMenuClicked(object sender, ClickedEventArgs e)
	{
		Debug.Log("Menu");
	}

	private void DetectJump()
	{
		Vector3 currentLocation = HeadPosition.position;
		Vector3 diff = currentLocation - m_PreviousHeadLocation;

		// Potentially started jump
		if (diff.y >= JumpThreshold)
		{
			m_HasAscended = true;
			m_JumpTimer = JumpDurationThreshold;
		}

		if (m_HasAscended)
		{
			m_JumpTimer -= Time.deltaTime;

			// Wasn't a jump
			if (m_JumpTimer < 0.0f)
				m_HasAscended = false;

			// Was a jump!
			else if (diff.y <= JumpThreshold)
			{
				if (PlayerJumped != null)
					PlayerJumped(this, new System.EventArgs());
			}
		}

		m_PreviousHeadLocation = currentLocation;
	}
}
