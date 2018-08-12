using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundEmitter))]
public class PlayerController : MonoBehaviour
{
	/// <summary>
	/// PlayerController (single instance)
	/// </summary>
	public static PlayerController Main { get; private set; }


	[Header("Tracked Parts")]
	[SerializeField]
	private PlayerHead Head;

	[SerializeField]
	private PlayerHand LeftHand;
	[SerializeField]
	private PlayerHand RightHand;
	

	[Header("Jumping")]
	[SerializeField]
	private float JumpThreshold = 2.0f;
	[SerializeField]
	private float JumpDurationThreshold = 1.5f;

	private Vector3 m_PreviousHeadLocation;
	private float m_JumpTimer;
	private bool m_HasAscended;


	[Header("Health")]
	[SerializeField]
	private int MaxHealth = 6;
	[SerializeField]
	private float RegenRate = 4.0f;

	[SerializeField]
	private float ExplosiveForce = 10.0f;
	[SerializeField]
	private float ExplosiveRadius = 5.0f;

	private int m_Health;
	private float m_RegenTimer;

	[Header("Audio")]
	[SerializeField]
	private AudioClip DamageSound;

	[SerializeField]
	private AudioClip TeleportSound;

	private SoundEmitter m_SoundEmitter;


	public event System.EventHandler PlayerJumped;
	public event System.EventHandler PlayerHealthChange;

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
		Head.Player = this;
		m_Health = MaxHealth;

		LeftHand.SteamVRController.MenuButtonClicked += OnMenuClicked;
		RightHand.SteamVRController.MenuButtonClicked += OnMenuClicked;

		m_PreviousHeadLocation = HeadPosition.position;
		m_SoundEmitter = GetComponent<SoundEmitter>();
	}

	public Transform HeadPosition
	{
		get { return Head.transform; }
	}

	public int Health
	{
		get { return m_Health; }
		set
		{
			int oldHealth = m_Health;
			m_Health = Mathf.Clamp(value, 0, MaxHealth);
			if(oldHealth != m_Health)
				PlayerHealthChange(this, new System.EventArgs());
			if (m_Health < oldHealth)
				m_SoundEmitter.PlaySound(DamageSound);
		}
	}

	public int MaximumHealth
	{
		get { return MaxHealth; }
	}

	public SoundEmitter SoundSource
	{
		get { return m_SoundEmitter; }
	}

	public AudioClip TeleportSoundEffect
	{
		get { return TeleportSound; }
	}

	void Update ()
	{
		// Detect death by ring
		RingController ringController = FindObjectOfType<RingController>();
		if (ringController != null && ringController.OutsideRange(HeadPosition.position))
		{
			Health = 0;
			return;
		}

		// Regen health
		if (m_Health < MaxHealth)
		{
			m_RegenTimer -= Time.deltaTime;
			if (m_RegenTimer < 0.0f)
			{
				Health++;
				m_RegenTimer = RegenRate;
			}
		}
		
		DetectJump();
	}

	private void OnMenuClicked(object sender, ClickedEventArgs e)
	{
		PauseMenuController.Main.Toggle();
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

	public void OnHitByDamage(GameObject obj)
	{
		foreach (Rigidbody body in GameObject.FindObjectsOfType<Rigidbody>())
			if (body.gameObject.CompareTag("Interactable"))
			{
				Vector3 diff = body.transform.position - Head.transform.position;
				if(diff.sqrMagnitude <= ExplosiveRadius * ExplosiveRadius)
					body.velocity = diff.normalized * ExplosiveForce;
			}
		
		Health--;
		m_RegenTimer = RegenRate;
	}

	public void DropInventory()
	{
		LeftHand.CurrentItem = null;
		RightHand.CurrentItem = null;
	}
}
