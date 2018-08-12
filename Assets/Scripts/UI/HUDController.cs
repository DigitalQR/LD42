using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
	/// <summary>
	/// HUDController (single instance)
	/// </summary>
	public static HUDController Main { get; private set; }
	

	void Start ()
	{
		if (Main != this)
		{
			if (Main != null)
				throw new UnityException("Multiple HUDController active");
			else
			{
				Main = this;
				Debug.Log("HUDController found");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
