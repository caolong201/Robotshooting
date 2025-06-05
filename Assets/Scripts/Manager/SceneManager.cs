using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using DG.Tweening;

public class SceneLoadManager
{

    private static string mLastScene;

    public static void LoadLevel(string scene)
    {
        //UIManager.Instance.HideAllDialog();
        LastScene = SceneManager.GetActiveScene().name;
        Debug.Log("LoadLevel " + scene);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);

    }

    public static IEnumerator LoadLevelAsync(string scene, Action callback = null)
    {
        DOTween.Clear(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        callback?.Invoke();
    }

    public static void ReLoadLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public static string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static string LastScene
    {
        private set { mLastScene = value; }
        get { return mLastScene; }
    }

}

public class SceneName
{
    public const string Loading = "scene_loading";
    public const string Home = "scene_home";
    public const string Gameplay = "scene_gameplay";
    public const string Result = "scene_result";
    public const string Rank = "scene_rank"; 
    public const string Rank_Highscore = "scene_rank_highscore";
    public const string Inventory_Plants = "scene_inventory_plants";
    public const string History = "scene_history";
    public const string GardenHome = "scene_garden_home";
    public const string GardenChangePlant = "scene_garden_change_plant";
}