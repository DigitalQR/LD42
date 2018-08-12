using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInfrontOfPlayer : MonoBehaviour
{
	[SerializeField]
	private float Distance = 3.0f;

	[SerializeField]
	private float Snappiness = 5.0f;

	
	void Update ()
	{
		if (PlayerController.Main != null)
		{
			Transform target = PlayerController.Main.HeadPosition;
			Vector3 targetLocation = target.position + new Vector3(target.forward.x, 0.0f, target.forward.z) * Distance;

			transform.position = Vector3.Lerp(transform.position, targetLocation, Snappiness * Time.fixedUnscaledDeltaTime);
		}
	}
}
