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

    var targetExts = new string[] {
      ".json", ".meta",
      ".asmdef", ".cs",
      ".mat", ".prefab", ".shader",
      ".aar", ".dll", ".dylib", ".so", ".framework",
    };
    var assets = Directory.EnumerateFiles(packageRoot, "*", SearchOption.AllDirectories)
        .Where(x => Array.IndexOf(targetExts, Path.GetExtension(x)) >= 0)
        .Select(x => "Packages/com.github.homuler.mediapipe" + x.Replace(packageRoot, "").Replace(@"\", "/"))
        .ToArray();

    Debug.Log("Export below files" + Environment.NewLine + string.Join(Environment.NewLine, assets));

    AssetDatabase.ExportPackage(
        assets,
        exportPath,
        ExportPackageOptions.Default);

    Debug.Log("Export complete: " + Path.GetFullPath(exportPath));
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
    public IDictionary<string, string> dependencies;
    public string documentationUrl;
    public bool hideInEditor;
    public IList<string> keywords;
    public string license;
    public string licenseUrl;
    public IList<Sample> samples;
    public string type;
    public string unityRelease;

    public class Author
    {
      public string name;
      public string email;
      public string url;
    }

    public class Sample
    {
      public string displayName;
      public string description;
      public string path;
    }
  }
}

#endif
