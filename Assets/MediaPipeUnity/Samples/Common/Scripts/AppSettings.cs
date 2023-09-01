// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using UnityEngine;
using UnityEngine.Video;

namespace Mediapipe.Unity.Sample
{
  [Serializable]
  public class AppSettings : ScriptableObject
  {
    [Serializable]
    public enum AssetLoaderType
    {
      StreamingAssets,
      AssetBundle,
      Local,
    }

    [SerializeField] private ImageSourceType _defaultImageSource;
    [SerializeField] private InferenceMode _preferableInferenceMode;
    [SerializeField] private AssetLoaderType _assetLoaderType;
    [SerializeField] private Logger.LogLevel _logLevel = Logger.LogLevel.Debug;

    [Header("Glog settings")]
    [SerializeField] private int _glogMinloglevel = Glog.Minloglevel;
    [SerializeField] private int _glogStderrthreshold = Glog.Stderrthreshold;
    [SerializeField] private int _glogV = Glog.V;

    public ImageSourceType defaultImageSource => _defaultImageSource;
    public InferenceMode preferableInferenceMode => _preferableInferenceMode;
    public AssetLoaderType assetLoaderType => _assetLoaderType;
    public Logger.LogLevel logLevel => _logLevel;

    public void ResetGlogFlags()
    {
      Glog.Logtostderr = true;
      Glog.Minloglevel = _glogMinloglevel;
      Glog.Stderrthreshold = _glogStderrthreshold;
      Glog.V = _glogV;
    }
  }
}
