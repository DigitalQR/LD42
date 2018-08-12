using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{ 
	/// <summary>
	/// PauseMenuController (single instance)
	/// </summary>
	public static PauseMenuController Main { get; private set; }
	
	private bool m_IsPaused;
	private float m_PreviousTimeScale;

	void Start ()
	{
		if (Main != this)
		{
			if (Main != null)
				throw new UnityException("Multiple PauseMenuController active");
			else
			{
				Main = this;
				Debug.Log("PauseMenuController found");
			}
		}

		gameObject.SetActive(false);
		m_IsPaused = false;
	}

	public bool IsPaused
	{
		get { return m_IsPaused; }
		set
		{
			if (m_IsPaused != value)
			{
				m_IsPaused = value;
				gameObject.SetActive(m_IsPaused);

				if (m_IsPaused)
				{
					m_PreviousTimeScale = Time.timeScale;
					Time.timeScale = 0.0f;
				}
				else
					Time.timeScale = m_PreviousTimeScale;
			}
		}
	}

	public void Toggle()
	{
		IsPaused = !IsPaused;
	}

	public void Pause()
	{
		IsPaused = true;
	}

	public void Unpause()
	{
		IsPaused = false;
	}

}
