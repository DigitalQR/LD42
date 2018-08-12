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
	private string[] AlwaysActiveObjectNames;

	[SerializeField]
	private string[] RandomLevels = new string[1];

	private Scene m_MainScene;
	private string m_ActiveSceneName;


	void Start ()
	{
		Random.InitState((int)System.DateTime.Now.Ticks);

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
		GameController.Main.OnLevelSwitch();
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

	public void SwitchScene(string scene)
	{
		// Find real level name (if Random given)
		if (scene == "Random")
		{
			if (RandomLevels.Length == 1)
				scene = RandomLevels[0];
			else
			{
				int startIndex = Random.Range(0, RandomLevels.Length);
				for (int n = 0; n < RandomLevels.Length; ++n)
				{
					int i = (startIndex + n) % RandomLevels.Length;
					if (RandomLevels[i] != m_ActiveSceneName)
					{
						scene = RandomLevels[i];
						break;
					}
				}
			}
		}


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
			GameController.Main.OnLevelSwitch();
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
}
