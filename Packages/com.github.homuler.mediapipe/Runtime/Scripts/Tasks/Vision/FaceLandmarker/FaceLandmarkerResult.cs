// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;
using UnityEngine;

namespace Mediapipe.Tasks.Vision.FaceLandmarker
{
  /// <summary>
  ///   The face landmarks result from FaceLandmarker, where each vector element represents a single face detected in the image.
  /// </summary>
  public readonly struct FaceLandmarkerResult
  {
    /// <summary>
    ///   Detected face landmarks in normalized image coordinates.
    /// </summary>
    public readonly List<NormalizedLandmarks> faceLandmarks;
    /// <summary>
    ///   Optional face blendshapes results.
    /// </summary>
    public readonly List<Classifications> faceBlendshapes;
    /// <summary>
    ///   Optional facial transformation matrix.
    /// </summary>
    public readonly List<Matrix4x4> facialTransformationMatrixes;

    internal FaceLandmarkerResult(List<NormalizedLandmarks> faceLandmarks,
        List<Classifications> faceBlendshapes, List<Matrix4x4> facialTransformationMatrixes)
    {
      this.faceLandmarks = faceLandmarks;
      this.faceBlendshapes = faceBlendshapes;
      this.facialTransformationMatrixes = facialTransformationMatrixes;
    }

    public static FaceLandmarkerResult Alloc(int capacity, bool outputFaceBlendshapes = false, bool outputFaceTransformationMatrixes = false)
    {
      var faceLandmarks = new List<NormalizedLandmarks>(capacity);
      var faceBlendshapes = outputFaceBlendshapes ? new List<Classifications>(capacity) : null;
      var facialTransformationMatrixes = outputFaceTransformationMatrixes ? new List<Matrix4x4>(capacity) : null;
      return new FaceLandmarkerResult(faceLandmarks, faceBlendshapes, facialTransformationMatrixes);
    }

    public void CloneTo(ref FaceLandmarkerResult destination)
    {
      if (faceLandmarks == null)
      {
        destination = default;
        return;
      }

      var dstFaceLandmarks = destination.faceLandmarks ?? new List<NormalizedLandmarks>(faceLandmarks.Count);
      dstFaceLandmarks.Clear();
      dstFaceLandmarks.AddRange(faceLandmarks);

      var dstFaceBlendshapes = destination.faceBlendshapes;
      if (faceBlendshapes != null)
      {
        dstFaceBlendshapes ??= new List<Classifications>(faceBlendshapes.Count);
        dstFaceBlendshapes.Clear();
        dstFaceBlendshapes.AddRange(faceBlendshapes);
      }

      var dstFacialTransformationMatrixes = destination.facialTransformationMatrixes;
      if (facialTransformationMatrixes != null)
      {
        dstFacialTransformationMatrixes ??= new List<Matrix4x4>(facialTransformationMatrixes.Count);
        dstFacialTransformationMatrixes.Clear();
        dstFacialTransformationMatrixes.AddRange(facialTransformationMatrixes);
      }

      destination = new FaceLandmarkerResult(dstFaceLandmarks, dstFaceBlendshapes, dstFacialTransformationMatrixes);
    }

    public override string ToString()
      => $"{{ \"faceLandmarks\": {Util.Format(faceLandmarks)}, \"faceBlendshapes\": {Util.Format(faceBlendshapes)}, \"facialTransformationMatrixes\": {Util.Format(facialTransformationMatrixes)} }}";
  }

  internal static class MatrixDataExtension
  {
    public static Matrix4x4 ToMatrix4x4(this MatrixData matrixData)
    {
      var matrix = new Matrix4x4();
      var data = matrixData.PackedData;
      // NOTE: z direction is inverted
      if (matrixData.Layout == MatrixData.Types.Layout.RowMajor)
      {
        matrix.SetRow(0, new Vector4(data[0], data[1], data[2], data[3]));
        matrix.SetRow(1, new Vector4(data[4], data[5], data[6], data[7]));
        matrix.SetRow(2, new Vector4(-data[8], -data[9], -data[10], -data[11]));
        matrix.SetRow(3, new Vector4(data[12], data[13], data[14], data[15]));
      }
      else
      {
        matrix.SetColumn(0, new Vector4(data[0], data[1], -data[2], data[3]));
        matrix.SetColumn(1, new Vector4(data[4], data[5], -data[6], data[7]));
        matrix.SetColumn(2, new Vector4(data[8], data[9], -data[10], data[11]));
        matrix.SetColumn(3, new Vector4(data[12], data[13], -data[14], data[15]));
      }
      return matrix;
    }
  }
}
