using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
	[SerializeField]
	private GameObject[] Overlays;

	private bool m_Initialised = false;

	void Start()
	{
		foreach (GameObject obj in Overlays)
			obj.SetActive(false);
	}

	void Update ()
	{
		if (!m_Initialised && PlayerController.Main != null)
		{
			PlayerController.Main.PlayerHealthChange += OnPlayerHealthChange;
			m_Initialised = true;
		}
	}

	private void OnPlayerHealthChange(object sender, System.EventArgs e)
	{
		PlayerController player = sender as PlayerController;
		int healthIndex = player.MaximumHealth - player.Health - 1;

		for (int i = 0; i < Overlays.Length; ++i)
			Overlays[i].SetActive(i == healthIndex);
	}
}
