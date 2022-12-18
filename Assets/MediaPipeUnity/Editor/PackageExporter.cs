// The MIT License (MIT)
//
// Copyright (c) 2019 Yoshifumi Kawai / Cysharp, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class PackageExporter
{
  [MenuItem("Tools/Export Unitypackage")]
  public static void Export()
  {
    var packageRoot = Path.Combine(Application.dataPath, "..", "Packages", "com.github.homuler.mediapipe");
    var version = GetVersion(packageRoot);

    var fileName = string.IsNullOrEmpty(version) ? "MediaPipeUnity.unitypackage" : $"MediaPipeUnity.{version}.unitypackage";
    var exportPath = "./" + fileName;

    var pluginAssets = EnumerateAssets(Path.Combine("Packages", "com.github.homuler.mediapipe")); // export all the files
    var sampleAssets = EnumerateAssets(Path.Combine("Assets", "MediaPipeUnity", "Samples"), new string[] { ".cs", ".unity" })
      .Where(x => Path.GetFileName(x) != "Start Scene.unity"); // exclude the 'Start Scene'
    var tutorialAssets = EnumerateAssets(Path.Combine("Assets", "MediaPipeUnity", "Tutorial")); // export all the files
    var assets = pluginAssets.Concat(sampleAssets).Concat(tutorialAssets).ToArray();

    Debug.Log("Export below files" + Environment.NewLine + string.Join(Environment.NewLine, assets));

    AssetDatabase.ExportPackage(
        assets,
        exportPath,
        ExportPackageOptions.IncludeDependencies);

    Debug.Log("Export complete: " + Path.GetFullPath(exportPath));
  }

  private static IEnumerable<string> EnumerateAssets(string path)
  {
    var projectRoot = Path.Combine(Application.dataPath, "..");
    var assetRoot = Path.Combine(projectRoot, path);

    return Directory.EnumerateFiles(assetRoot, "*", SearchOption.AllDirectories)
        .Select(x => path + x.Replace(assetRoot, "").Replace(@"\", "/"));
  }

  private static IEnumerable<string> EnumerateAssets(string path, string[] extensions)
  {
    return EnumerateAssets(path).Where(x => Array.IndexOf(extensions, Path.GetExtension(x)) >= 0);
  }

  private static string GetVersion(string packagePath)
  {
    var version = Environment.GetEnvironmentVariable("UNITY_PACKAGE_VERSION");
    var packageJsonPath = Path.Combine(packagePath, "package.json");

    if (File.Exists(packageJsonPath))
    {
      var packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(packageJsonPath));

      if (!string.IsNullOrEmpty(version))
      {
        if (packageJson.version != version)
        {
          var msg = $"package.json and env version are mismatched. UNITY_PACKAGE_VERSION:{version}, package.json:{packageJson.version}";

          if (Application.isBatchMode)
          {
            Console.WriteLine(msg);
            Application.Quit(1);
          }

          throw new Exception("package.json and env version are mismatched.");
        }
      }

      version = packageJson.version;
    }

    return version;
  }

  public class PackageJson
  {
    public string name;
    public string version;
    public string displayName;
    public string description;
    public string unity;
    public Author author;
    public string changelogUrl;
    // public Dictionary<string, string> dependencies;
    public string documentationUrl;
    public bool hideInEditor;
    public List<string> keywords;
    public string license;
    public string licenseUrl;
    public List<Sample> samples;
    public string type;
    public string unityRelease;

    [Serializable]
    public class Author
    {
      public string name;
      public string email;
      public string url;
    }

    [Serializable]
    public class Sample
    {
      public string displayName;
      public string description;
      public string path;
    }
  }
}

#endif
