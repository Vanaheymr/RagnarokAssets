using UnityEditor;
using UnityEngine;

public static class AssetBundleLoader
{
    [MenuItem("Tools/View AssetBundle")]
    public static void LoadAndViewAssetBundle()
    {
        var path = EditorUtility.OpenFilePanel("Select AssetBundle", "", "");
        if (!string.IsNullOrEmpty(path))
        {
            var assetBundle = AssetBundle.LoadFromFile(path);
            Debug.Log($"AssetBundle {assetBundle.name}:\n{string.Join("\n", assetBundle.GetAllAssetNames())}");

            var root = new GameObject(assetBundle.name);
            foreach (var asset in assetBundle.LoadAllAssets())
            {
                if (asset is GameObject)
                {
                    Object.Instantiate(asset, root.transform);
                }
            }
            assetBundle.Unload(false);
        }
    }
}
