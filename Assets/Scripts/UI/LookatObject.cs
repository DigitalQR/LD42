using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatObject : MonoBehaviour
{
	[SerializeField]
	private Transform Target;

	void Update ()
	{
		Quaternion lookRot = Quaternion.LookRotation(Target.position - transform.position, Vector3.up);
		transform.rotation = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);
	}
}
