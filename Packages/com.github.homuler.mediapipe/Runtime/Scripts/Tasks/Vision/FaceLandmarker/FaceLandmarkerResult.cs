// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.Linq;
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
    public readonly IReadOnlyList<NormalizedLandmarks> faceLandmarks;
    /// <summary>
    ///   Optional face blendshapes results.
    /// </summary>
    public readonly IReadOnlyList<Classifications> faceBlendshapes;
    /// <summary>
    ///   Optional facial transformation matrix.
    /// </summary>
    public readonly IReadOnlyList<Matrix4x4> facialTransformationMatrixes;

    internal FaceLandmarkerResult(IReadOnlyList<NormalizedLandmarks> faceLandmarks,
        IReadOnlyList<Classifications> faceBlendshapes, IReadOnlyList<Matrix4x4> facialTransformationMatrixes)
    {
      this.faceLandmarks = faceLandmarks;
      this.faceBlendshapes = faceBlendshapes;
      this.facialTransformationMatrixes = facialTransformationMatrixes;
    }

    // TODO: add parameterless constructors
    internal static FaceLandmarkerResult Empty()
      => new FaceLandmarkerResult(new List<NormalizedLandmarks>(), new List<Classifications>(), new List<Matrix4x4>());

    internal static FaceLandmarkerResult CreateFrom(IReadOnlyList<NormalizedLandmarkList> faceLandmarksProto,
        IReadOnlyList<ClassificationList> faceBlendshapesProto, IReadOnlyList<FaceGeometry.Proto.FaceGeometry> facialTransformationMatrixesProto)
    {
      var faceLandmarks = faceLandmarksProto.Select(NormalizedLandmarks.CreateFrom).ToList();
      var faceBlendshapes = faceBlendshapesProto == null ? new List<Classifications>() :
          faceBlendshapesProto.Select(x => Classifications.CreateFrom(x)).ToList();
      var facialTransformationMatrixes = facialTransformationMatrixesProto == null ? new List<Matrix4x4>() :
          facialTransformationMatrixesProto.Select(x => x.PoseTransformMatrix.ToMatrix4x4()).ToList();

      return new FaceLandmarkerResult(faceLandmarks, faceBlendshapes, facialTransformationMatrixes);
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
