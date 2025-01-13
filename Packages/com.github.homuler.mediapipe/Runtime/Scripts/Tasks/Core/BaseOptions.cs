// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Core
{
  public sealed class BaseOptions
  {
    public enum Delegate
    {
      CPU = 0,
      GPU = 1,
      // Edge TPU acceleration using NNAPI delegate.
      EDGETPU_NNAPI = 2,
    }

    public Delegate delegateCase { get; } = Delegate.CPU;
    public string modelAssetPath { get; } = string.Empty;
    public byte[] modelAssetBuffer { get; } = null;

    public BaseOptions(Delegate delegateCase = Delegate.CPU, string modelAssetPath = null, byte[] modelAssetBuffer = null)
    {
      this.delegateCase = delegateCase;
      this.modelAssetPath = modelAssetPath;
      this.modelAssetBuffer = modelAssetBuffer;
    }

    private Proto.Acceleration acceleration
    {
      get
      {
        switch (delegateCase)
        {
          case Delegate.CPU:
            return new Proto.Acceleration
            {
              Tflite = new InferenceCalculatorOptions.Types.Delegate.Types.TfLite { },
            };
          case Delegate.GPU:
            return new Proto.Acceleration
            {
              Gpu = new InferenceCalculatorOptions.Types.Delegate.Types.Gpu
              {
                UseAdvancedGpuApi = true,
              },
            };
          case Delegate.EDGETPU_NNAPI:
            return new Proto.Acceleration
            {
              Nnapi = new InferenceCalculatorOptions.Types.Delegate.Types.Nnapi
              {
                AcceleratorName = "google-edgetpu",
              },
            };
          default:
            return null;
        }
      }
    }

    private Proto.ExternalFile modelAsset
    {
      get
      {
        var file = new Proto.ExternalFile { };

        if (modelAssetPath != null)
        {
          file.FileName = modelAssetPath;
        }
        if (modelAssetBuffer != null)
        {
          file.FileContent = Google.Protobuf.ByteString.CopyFrom(modelAssetBuffer);
        }

        return file;
      }
    }

    internal Proto.BaseOptions ToProto()
    {
      return new Proto.BaseOptions
      {
        ModelAsset = modelAsset,
        Acceleration = acceleration,
      };
    }
  }
}
