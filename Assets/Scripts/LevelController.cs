using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

	/// <summary>
	/// LevelController (single instance)
	/// </summary>
	public static LevelController Main { get; private set; }

	[SerializeField]
	private PlayerController PlayerPrefab;
	[SerializeField]
	private string[] AlwaysActiveObjectNames;

	[SerializeField]
	private string GameOverScene = "Entry";

	private Scene m_MainScene;
	private string m_ActiveSceneName;


	void Start ()
	{
		if (Main != this)
		{
			if (Main != null)
				throw new UnityException("Multiple LevelController active");
			else
			{
				Main = this;
				Debug.Log("LevelController found");
			}
		}

		DontDestroyOnLoad(gameObject);
		m_MainScene = SceneManager.GetSceneAt(0);
		SpawnPlayer();
	}

	private bool IsAlwaysActive(GameObject obj)
	{
		foreach (string name in AlwaysActiveObjectNames)
			if (obj.name == name)
				return true;
		return false;
	}

	public bool InMenuLevel
	{
		get { return m_ActiveSceneName == null; }
	}

	public bool IsUIActive
	{
		get { return PauseMenuController.Main.IsPaused || InMenuLevel; }
	}

	/// <summary>
	/// Spawn a new player or move existing one to spawn point
	/// </summary>
	public void SpawnPlayer()
	{
		// Find spawn location
		Vector3 spawnPoint = Vector3.zero;
		if(!InMenuLevel)
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


		// Destroy all items
		if (InMenuLevel)
		{
			foreach (EquipableItemBase item in FindObjectsOfType<EquipableItemBase>())
				Destroy(item);
		}
	}

	public void SwitchScene(string scene)
	{
		// Unload old scene
		if (InMenuLevel)
		{
			// Just disabled entry scene (It should never be unloaded)
			foreach (GameObject obj in m_MainScene.GetRootGameObjects())
				if(!IsAlwaysActive(obj))
					obj.SetActive(false);
		}
		else
			StartCoroutine(UnloadLevel(m_ActiveSceneName));


		// Load new scene
		if (scene == "Entry")
		{
			// Just re-enabled entry scene (It should never be unloaded)
			foreach (GameObject obj in m_MainScene.GetRootGameObjects())
				if (!IsAlwaysActive(obj))
					obj.SetActive(true);

			m_ActiveSceneName = null;
			SpawnPlayer();
		}
		else if (scene != null)
			StartCoroutine(LoadLevel(scene));
	}

	private IEnumerator UnloadLevel(string sceneName)
	{
		AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

		while (!operation.isDone)
		{
			yield return null;
		}
	}

	private IEnumerator LoadLevel(string sceneName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		while (!operation.isDone)
		{
			m_ActiveSceneName = sceneName;
			yield return null;
		}
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void OnPlayerHealthChange(object sender, System.EventArgs e)
	{
		PlayerController player = sender as PlayerController;
		if (player.Health == 0)
			SwitchScene(GameOverScene);
	}
}
