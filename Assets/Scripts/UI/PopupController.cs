using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
	/// <summary>
	/// PopupController (single instance)
	/// </summary>
	public static PopupController Main { get; private set; }

	[SerializeField]
	private PopupText Text;

	private Queue<PopupMessage> m_MessageQueue = new Queue<PopupMessage>();

	void Start ()
	{
		if (Main != this)
		{
			if (Main != null)
				throw new UnityException("Multiple PopupController active");
			else
			{
				Main = this;
				Debug.Log("PopupController found");
			}
		}
	}
	
	void Update ()
	{
		if(m_MessageQueue.Count != 0 && !Text.InUse)
		{
			PopupMessage message = m_MessageQueue.Dequeue();
			Text.Push(message);
		}
	}

	public void Push(PopupMessage message)
	{
		m_MessageQueue.Enqueue(message);
	}

	public void PushImmediate(PopupMessage message)
	{
		Text.Push(message);
	}
}
