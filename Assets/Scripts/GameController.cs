using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	/// <summary>
	/// LevelController (single instance)
	/// </summary>
	public static GameController Main { get; private set; }

	[SerializeField]
	private PlayerController PlayerPrefab;

	[SerializeField]
	private string GameOverScene = "Entry";

	[SerializeField]
	private float RandomSpawnStartValue = 0.1f;
	[SerializeField]
	private float RandomSpawnIncreaseRate = 0.1f;

	private bool m_IsActive = false;
	private float m_RandomSpawnChance;


	void Start ()
	{
		if (Main != this)
		{
			if (Main != null)
				throw new UnityException("Multiple GameController active");
			else
			{
				Main = this;
				Debug.Log("GameController found");
			}
		}
	}

	public float RandomSpawnChance
	{
		get { return m_RandomSpawnChance; }
	}
	
	/// <summary>
	/// Spawn a new player or move existing one to spawn point
	/// </summary>
	private void SpawnPlayer()
	{
		// Find spawn location
		Vector3 spawnPoint = Vector3.zero;
		if (!LevelController.Main.InMenuLevel)
			foreach (GameObject spawn in GameObject.FindGameObjectsWithTag("Respawn"))
			{
				if (spawn.CompareTag("Respawn"))
				{
					spawnPoint = spawn.transform.position;
					break;
				}
			}

		// Spawn in or move player
		if (PlayerController.Main == null)
		{
			PlayerController player = Instantiate(PlayerPrefab, spawnPoint, Quaternion.identity);
			player.PlayerHealthChange += OnPlayerHealthChange;
			DontDestroyOnLoad(player.gameObject);
		}
		else
			PlayerController.Main.transform.position = spawnPoint;
	}

	private void OnPlayerHealthChange(object sender, System.EventArgs e)
	{
		PlayerController player = sender as PlayerController;
		if (player.Health == 0)
		{
			OnGameEnd();
			player.Health = player.MaximumHealth;
			player.DropInventory();
		}
	}
	
	public void OnLevelSwitch()
	{
		SpawnPlayer();

		// Check for new game states
		if (LevelController.Main.InMenuLevel)
		{
			if (m_IsActive)
				OnGameEnd();
		}
		else
		{
			if (!m_IsActive)
				OnGameStart();
			else
				OnGameStateUpdate();
		}
	}
	
	private void OnGameStart()
	{
		m_IsActive = true;
		m_RandomSpawnChance = RandomSpawnStartValue;
	}

	private void OnGameStateUpdate()
	{
		m_RandomSpawnChance += RandomSpawnIncreaseRate;
	}

	public void OnGameEnd()
	{
		m_IsActive = false;
		LevelController.Main.SwitchScene(GameOverScene);

		PopupMessage msg = new PopupMessage("Game Over", "Need some more space?", 5.0f);
		PopupController.Main.PushImmediate(msg);
	}
}
