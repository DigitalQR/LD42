using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatPlayer : MonoBehaviour
{
	
	void Update ()
	{
		if (PlayerController.Main != null)
		{
			Transform target = PlayerController.Main.HeadPosition;
			Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
			transform.rotation = Quaternion.Euler(0, lookRot.eulerAngles.y, 0) * Quaternion.AngleAxis(180.0f, Vector3.up);
		}
	}
}
