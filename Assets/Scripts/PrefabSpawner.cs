using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject Prefab;
	
	void Awake ()
	{
		Instantiate(Prefab, transform);
	}
}
