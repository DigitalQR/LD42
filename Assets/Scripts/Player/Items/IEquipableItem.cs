using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEquipableItem
{
	bool IsCurrentlyEquiped();

	void OnEquipPrompt(PlayerHand hand, bool isOpen);

	void OnEquip(PlayerHand hand);
	void OnDrop();

	void OnPrimaryButton(bool isPressed);
}
