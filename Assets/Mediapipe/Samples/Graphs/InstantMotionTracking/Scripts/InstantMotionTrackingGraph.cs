using Mediapipe;
using System;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class InstantMotionTrackingGraph : OfficialDemoGraph {
  [SerializeField] TextAsset texture3dAsset = null;

  int stickerSentinelId = -1;

  // 0: render GIF
  // 1: render 3D asset
  // This sample only works with 1 (3D asset)
  int renderId = 1;

  Gyroscope gyroscope;
  Sticker currentSticker;
  float[] imuRotationMatrix;

  void Start() {
    if (SystemInfo.supportsGyroscope) {
      gyroscope = Input.gyro;
      gyroscope.enabled = true;
    }
  }

  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit raycastHit;

      if (Physics.Raycast(raycast, out raycastHit) && raycastHit.collider.name == "WebCamScreen") {
        var center = raycastHit.collider.bounds.center;
        var extents = raycastHit.collider.bounds.extents;
        var x = (raycastHit.point.x - center.x) / 2 / extents.x + 0.5f;
        var y = 0.5f - (raycastHit.point.y - center.y) / 2 / extents.y;

        ResetSticker(x, y);
      }
    }

    if (gyroscope != null) {
      UpdateImuRotationMatrix(gyroscope);
    }
  }

  public override Status StartRun(Texture texture) {
    if (!IsGpuEnabled()) {
      return Status.FailedPrecondition("InstantMotionTracking is not supported on CPU");
    }

    stopwatch.Start();
    ResetSticker();
    imuRotationMatrix = new float[] { 0, 0, 1, 1, 0, 0, 0, 1, 0 };

    graph.ObserveOutputStream("asset_3d_matrices", MatrixCallback).AssertOk();

    sidePacket = new SidePacket();
    sidePacket.Emplace("vertical_fov_radians", new FloatPacket(GetVerticalFovRadians()));
    sidePacket.Emplace("aspect_ratio", new FloatPacket(3.0f / 4.0f));

    sidePacket.Emplace("texture_3d", new ImageFramePacket(GetImageFrameFromImage(texture3dAsset)));
    sidePacket.Emplace("asset_3d", new StringPacket("robot.obj.bytes"));

#if UNITY_ANDROID && !UNITY_EDITOR
    SetupOutputPacket(texture);
    sidePacket.Emplace(destinationBufferName, outputPacket);

    return graph.StartRun(sidePacket);
#else
    return StartRun();
#endif
  }

  public override Status PushInput(TextureFrame textureFrame) {
    base.PushInput(textureFrame).AssertOk();

    graph.AddPacketToInputStream("sticker_sentinel", new IntPacket(stickerSentinelId, currentTimestamp)).AssertOk();
    stickerSentinelId = -1;

    var stickerRoll = new StickerRoll();
    stickerRoll.Sticker.Add(currentSticker);
    graph.AddPacketToInputStream("sticker_proto_string", new StringPacket(stickerRoll.ToByteArray(), currentTimestamp)).AssertOk();
    graph.AddPacketToInputStream("imu_rotation_matrix", new FloatArrayPacket(imuRotationMatrix, currentTimestamp)).AssertOk();

    return Status.Ok();
  }

  [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
  static IntPtr MatrixCallback(IntPtr packetPtr) {
    try {
      using (var packet = new TimedModelMatrixProtoListPacket(packetPtr, false)) {
        var matrixProtoList = packet.Get();

        if (matrixProtoList.ModelMatrix.Count > 0) {
          var matrix = Matrix4x4FromBytes(matrixProtoList.ModelMatrix[0].MatrixEntries);
          // Debug.Log(matrix);
        }
      }

      // TODO: ensure the returned status won't be garbage collected prematurely.
      return Status.Ok().mpPtr;
    } catch (Exception e) {
      return Status.FailedPrecondition(e.ToString()).mpPtr;
    }
  }

  static Matrix4x4 Matrix4x4FromBytes(IList<float> matrixEntries) {
    var scale = 5.0f;  // a magic number in MediaPipe

    return new Matrix4x4(
      new Vector4(matrixEntries[0] / scale, -matrixEntries[4] / scale, -matrixEntries[8] / scale, matrixEntries[12]),
      new Vector4(matrixEntries[1] / scale, -matrixEntries[5] / scale, -matrixEntries[9] / scale, matrixEntries[13]),
      new Vector4(matrixEntries[2] / scale, -matrixEntries[6] / scale, -matrixEntries[10] / scale, matrixEntries[14]),
      new Vector4(matrixEntries[3], matrixEntries[7], matrixEntries[11], matrixEntries[15])
    );
  }

  float GetVerticalFovRadians() {
    // TODO: acquire it automatically
    return Mathf.Deg2Rad * 68.0f;
  }

  ImageFrame GetImageFrameFromImage(TextAsset image) {
    var tempTexture = new Texture2D(1, 1);
    tempTexture.LoadImage(image.bytes);

    // ensure that the texture format is RGBA32
    var texture = new Texture2D(tempTexture.width, tempTexture.height, TextureFormat.RGBA32, false);
    texture.SetPixels32(tempTexture.GetPixels32());
    // flip the image vertically to align pixels from top-left to bottom-right
    FlipTexture2D(texture);
    texture.Apply();

    return new ImageFrame(ImageFormat.Format.SRGBA, texture.width, texture.height, 4 * texture.width, texture.GetRawTextureData<byte>());
  }

  void ResetSticker(float x = 0.5f, float y = 0.5f) {
    var sticker = new Sticker();
    var id = currentSticker == null ? 1 : currentSticker.Id + 1;

    sticker.Id = id;
    sticker.X = x;
    sticker.Y = y;
    sticker.Scale = 1;
    sticker.RenderId = renderId;

    stickerSentinelId = id;
    currentSticker = sticker;
  }

  void UpdateImuRotationMatrix(Gyroscope gyroscope) {
    var matrix = Matrix4x4.Rotate(gyroscope.attitude);
    // from right-hand to left-hand (rotate 180 degrees around Y axis)
    var array = new float[] {
      -matrix[0, 0], matrix[0, 1], -matrix[0, 2],
      -matrix[1, 0], matrix[1, 1], -matrix[1, 2],
      -matrix[2, 0], matrix[2, 1], -matrix[2, 2],
    };

    imuRotationMatrix = array;
  }

  void FlipTexture2D(Texture2D texture) {
    var src = texture.GetPixels32();
    var dest = new Color32[src.Length];

    for (var i = 0; i < texture.height; i++) {
      var srcIdx = i * texture.width;
      var destIdx = (texture.height - 1 - i) * texture.width;
      System.Array.Copy(src, srcIdx, dest, destIdx, texture.width);
    }

    texture.SetPixels32(dest);
  }

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("ssdlite_object_detection.bytes");
    PrepareDependentAsset("robot.obj.bytes");
  }
}
