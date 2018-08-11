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
	}
	
	public Transform HeaderPosition
	{
		get { return HeadObject.transform; }
	}


	void Update ()
	{

	}

	private void OnMenuClicked(object sender, ClickedEventArgs e)
	{
		Debug.Log("Menu");
	}
}
