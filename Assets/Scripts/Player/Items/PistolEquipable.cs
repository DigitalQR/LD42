using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolEquipable : EquipableItemBase
{
	[Header("Gun Settings")]
	[SerializeField]
	private Transform MuzzleTransform;

	[SerializeField]
	private Projectile ProjectileType;

	[Header("Sound")]
	[SerializeField]
	private AudioClip ShootSound;

	[SerializeField]
	private AudioClip HitSound;

	protected override void Start()
	{
		base.Start();
	}

	public override void OnPrimaryButton(bool isPressed)
	{
		if (isPressed)
		{
			Projectile projectile = Instantiate(ProjectileType, MuzzleTransform.position, transform.rotation, null);
			projectile.Gun = this;
			SoundSource.PlaySound(ShootSound);
		}
	}

	public void OnShotHit()
	{
		SoundSource.PlaySound(HitSound);
	}
}
