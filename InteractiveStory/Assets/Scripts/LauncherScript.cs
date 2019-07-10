using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LauncherScript : MonoBehaviour
{
    private string prefix;

    private static List<GameObject> characters;
    private static List<AudioClip> sounds;
    private static List<Texture> backgrounds;

    private static TextAsset json;

    public static List<GameObject> Characters { get => characters; }
    public static List<AudioClip> Sounds { get => sounds; }
    public static List<Texture> Backgrounds { get => backgrounds; }
    public static TextAsset Json { get => json; }

    public UISlider loadingBar;

    private IEnumerator Start()
    {
        characters = new List<GameObject>();
        sounds = new List<AudioClip>();
        backgrounds = new List<Texture>();

#if UNITY_IOS
        prefix = "IOS";
#elif UNITY_ANDROID
        prefix = "Android";
#endif

        yield return StartCoroutine(LoadingPrefabs());
        yield return StartCoroutine(LoadingMusic());
        yield return StartCoroutine(LoadingBackgrounds());
        yield return StartCoroutine(LoadingJson());

        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator LoadingPrefabs()
    {
        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath + "/" + prefix + "/prefabs"));

        while (!req.isDone)
        {
            loadingBar.value = Mathf.Clamp01(req.progress / 0.9f);
            yield return null;
        }

        loadingBar.value = 1;

        AssetBundle loadedAB = req.assetBundle;
        if (loadedAB == null)
        {
            Debug.Log("Failed to load!");
            yield break;
        }

        var assets = loadedAB.LoadAllAssets();

        foreach (var item in assets)
        {
            characters.Add(item as GameObject);
        }

        loadedAB.Unload(false);
    }

    private IEnumerator LoadingMusic()
    {
        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath + "/" + prefix + "/music"));

        while (!req.isDone)
        {
            loadingBar.value = Mathf.Clamp01(req.progress / 0.9f);
            yield return null;
        }

        loadingBar.value = 1;

        AssetBundle loadedAB = req.assetBundle;
        if (loadedAB == null)
        {
            Debug.Log("Failed to load!");
            yield break;
        }

        var assets = loadedAB.LoadAllAssets();

        foreach (var item in assets)
        {
            sounds.Add(item as AudioClip);
        }

        loadedAB.Unload(false); 
    }

    private IEnumerator LoadingBackgrounds()
    {
        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath + "/" + prefix + "/backgrounds"));

        while (!req.isDone)
        {
            loadingBar.value = Mathf.Clamp01(req.progress / 0.9f);
            yield return null;
        }

        loadingBar.value = 1;

        AssetBundle loadedAB = req.assetBundle;
        if (loadedAB == null)
        {
            Debug.Log("Failed to load!");
            yield break;
        }

        var assets = loadedAB.LoadAllAssets(typeof(Texture));

        foreach (var item in assets)
        {
            backgrounds.Add(item as Texture);
        }

        loadedAB.Unload(false);
    }

    private IEnumerator LoadingJson()
    {
        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath + "/" + prefix + "/json"));

        while (!req.isDone)
        {
            loadingBar.value = Mathf.Clamp01(req.progress / 0.9f);
            yield return null;
        }

        loadingBar.value = 1;

        AssetBundle loadedAB = req.assetBundle;
        if (loadedAB == null)
        {
            Debug.Log("Failed to load!");
            yield break;
        }

        var asset = loadedAB.LoadAsset<TextAsset>("content");

        json = asset;

        loadedAB.Unload(false);
    }
}
