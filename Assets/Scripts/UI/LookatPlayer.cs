using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatPlayer : MonoBehaviour
{
	[SerializeField]
	private bool m_BillboardLook = true;

	void Update ()
	{
		if (PlayerController.Main != null)
		{
			Transform target = PlayerController.Main.HeadPosition;

			if (m_BillboardLook)
			{
				if (transform.parent == target)
					transform.parent = null;

				Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
				transform.rotation = Quaternion.Euler(0, lookRot.eulerAngles.y, 0) * Quaternion.AngleAxis(180.0f, Vector3.up);
			}
			else
			{
				if (transform.parent != target)
				{
					Vector3 pos = transform.localPosition;
					transform.parent = target;
					transform.localPosition = pos;
				}
			}
		}
	}
}
