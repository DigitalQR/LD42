using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct PopupMessage
{
	public string Header;
	public string Description;
	public float Duration;

	public PopupMessage(string header, string description, float duration)
	{
		Header = header;
		Description = description;
		Duration = duration;
	}
}


public class PopupText : MonoBehaviour
{
	[SerializeField]
	private Text HeaderText;
	[SerializeField]
	private Text DecriptionText;

	[SerializeField]
	private float m_FadeDuration = 2.0f;

	private float m_Timer = 0.0f;


	public bool InUse
	{
		get { return m_Timer > 0.0f; }
	}

	private void Start()
	{
		HeaderText.color = new Color(HeaderText.color.r, HeaderText.color.g, HeaderText.color.b, 0);
		DecriptionText.color = new Color(DecriptionText.color.r, DecriptionText.color.g, DecriptionText.color.b, 0);
	}

	private void Update()
	{
		if (InUse)
		{
			m_Timer -= Time.deltaTime;
			m_Timer = Mathf.Max(0, m_Timer);

			if (m_Timer <= m_FadeDuration)
			{
				float alpha = m_Timer / m_FadeDuration;
				HeaderText.color = new Color(HeaderText.color.r, HeaderText.color.g, HeaderText.color.b, alpha);
				DecriptionText.color = new Color(DecriptionText.color.r, DecriptionText.color.g, DecriptionText.color.b, alpha);
			}
		}
	}

	public void Push(PopupMessage message)
	{
		HeaderText.text = message.Header;
		DecriptionText.text = message.Description;
		
		HeaderText.color = new Color(HeaderText.color.r, HeaderText.color.g, HeaderText.color.b, 1.0f);
		DecriptionText.color = new Color(DecriptionText.color.r, DecriptionText.color.g, DecriptionText.color.b, 1.0f);

		m_Timer = message.Duration + m_FadeDuration;
	}
}
