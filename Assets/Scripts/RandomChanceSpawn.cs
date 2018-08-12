using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChanceSpawn : MonoBehaviour
{
	[SerializeField, Range(0,1)]
	private float ChanceFactor = 0.0f;

	void Awake ()
	{
		float chance = Random.value * (1.0f - ChanceFactor);

		if (chance < GameController.Main.RandomSpawnChance)
			SpawnChild();
		else
			Destroy(gameObject);
	}

	void SpawnChild()
	{
		gameObject.SetActive(true);

		int startIndex = Random.Range(0, transform.childCount);
		for (int n = 0; n < transform.childCount; ++n)
		{
			int i = (n + startIndex) % transform.childCount;
			if (n == 0)
				transform.GetChild(i).gameObject.SetActive(true);
			else
				Destroy(transform.GetChild(i).gameObject);
		}
	}
}
