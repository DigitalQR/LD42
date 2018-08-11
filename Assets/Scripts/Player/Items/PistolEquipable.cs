using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolEquipable : EquipableItemBase
{
	[SerializeField]
	private Transform MuzzleTransform;

	[SerializeField]
	private GameObject ProjectileType;

	protected override void Start()
	{
		base.Start();
	}

	public override void OnPrimaryButton(bool isPressed)
	{
		if (isPressed)
			Instantiate(ProjectileType, MuzzleTransform.position, transform.rotation, null);

		Debug.Log("Fire " + isPressed);
	}
}
