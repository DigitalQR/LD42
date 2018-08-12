using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDispatcher : MonoBehaviour
{
	[SerializeField]
	private string Header;

	[SerializeField, Multiline]
	private string Message;

	[SerializeField]
	private float Duration = 5.0f;


	void Start ()
	{
		TryPush();
	}
	
	void Update ()
	{
		TryPush();
	}

	private void TryPush()
	{
		if (PopupController.Main != null)
		{
			PopupMessage msg = new PopupMessage(Header, Message, Duration);
			PopupController.Main.Push(msg);
			Destroy(this);
		}
	}
}
