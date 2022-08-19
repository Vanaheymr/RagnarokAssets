using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ContentBuilder
{
    private static readonly string PackageFolder = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Package");

    [MenuItem("Tools/Build AssetBundles", priority = 2)]
    public static void BuildAssetBundles()
    {
        var buildPath = Path.Combine(PackageFolder, "AssetBundles");
        ClearTargetDirectory(buildPath);

        BuildPipeline.BuildAssetBundles(buildPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);

        var directoryName = Path.GetFileNameWithoutExtension(buildPath);
        var buildBundlePath = Path.Combine(buildPath, $"{directoryName}");
        var buildBundleManifestPath = $"{buildBundlePath}.manifest";

        var filesToDelete = new List<string>();
        filesToDelete.Add(buildBundlePath);
        filesToDelete.Add(buildBundleManifestPath);

        foreach (var assetBundleName in AssetDatabase.GetAllAssetBundleNames())
        {
            var bundlePath = Path.Combine(buildPath, assetBundleName);
            var manifestPath = $"{bundlePath}.manifest";
            filesToDelete.Add(manifestPath);

            if (File.Exists(bundlePath))
                File.Move(bundlePath, $"{bundlePath}.assets");
            else
                Debug.LogWarning($"Cannot find assetbundle {assetBundleName}");
        }

        filesToDelete.ForEach(path =>
        {
            if (File.Exists(path))
                File.Delete(path);
        });

        AssetDatabase.Refresh();

        Debug.Log($"AssetBundles built successfully: {buildPath}\nAssetBundles: {string.Join(", ", AssetDatabase.GetAllAssetBundleNames())}");
    }

    private static void ClearTargetDirectory(string path)
    {
        if (Directory.Exists(path))
            Directory.Delete(path, true);

        Directory.CreateDirectory(path);
    }
}
