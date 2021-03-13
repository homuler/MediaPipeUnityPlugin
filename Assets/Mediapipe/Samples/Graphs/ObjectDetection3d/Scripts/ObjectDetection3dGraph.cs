using Mediapipe;
using UnityEngine;

public class ObjectDetection3dGraph : OfficialDemoGraph {
  [SerializeField] Category category;
  [SerializeField] int maxNumObjects = 5;

  [SerializeField] TextAsset objTextureAsset = null;
  [SerializeField] TextAsset boxTextureAsset = null;

  enum Category {
    Camera,
    Chair,
    Cup,
    Sneaker,
  };

  public override Status StartRun(Texture texture) {
    stopwatch.Start();

    sidePacket = new SidePacket();
    sidePacket.Emplace("allowed_labels", new StringPacket(GetAllowedLabels(category)));
    sidePacket.Emplace("model_scale", new FloatArrayPacket(GetModelScale(category)));
    sidePacket.Emplace("model_transformation", new FloatArrayPacket(GetModelTransformation(category)));
    sidePacket.Emplace("box_texture", new ImageFramePacket(GetImageFrameFromImage(boxTextureAsset)));
    sidePacket.Emplace("max_num_objects", new IntPacket(maxNumObjects));
    sidePacket.Emplace("box_asset_name", new StringPacket("box.obj.bytes"));
    sidePacket.Emplace("obj_texture", new ImageFramePacket(GetImageFrameFromImage(objTextureAsset)));
    sidePacket.Emplace("obj_asset_name", new StringPacket(GetObjAssetName(category)));

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

    graph.AddPacketToInputStream("input_width", new IntPacket(textureFrame.width, currentTimestamp)).AssertOk();
    return graph.AddPacketToInputStream("input_height", new IntPacket(textureFrame.height, currentTimestamp));
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

  string GetAllowedLabels(Category category) {
    switch (category) {
      case Category.Camera: {
        return "Camera";
      }
      case Category.Chair: {
        return "Chair";
      }
      case Category.Cup: {
        return "Coffee cup,Mug";
      }
      default: {
        return "Footwear";
      }
    }
  }

  float[] GetModelScale(Category category) {
    switch (category) {
      case Category.Camera: {
        return new float[] { 250, 250, 250 };
      }
      case Category.Chair: {
        return new float[] { 0.1f, 0.05f, 0.1f };
      }
      case Category.Cup: {
        return new float[] { 500, 500, 500 };
      }
      default: {
        return new float[] { 0.25f, 0.25f, 0.12f };
      }
    }
  }

  float[] GetModelTransformation(Category category) {
    switch (category) {
      case Category.Camera: {
        return new float[] {
          1.0f,  0.0f, 0.0f,     0.0f,
          0.0f,  0.0f, 1.0f,     0.0f,
          0.0f, -1.0f, 0.0f, -0.0015f,
          0.0f,  0.0f, 0.0f,     1.0f,
        };
      }
      case Category.Chair: {
        return new float[] {
          1.0f, 0.0f,  0.0f,   0.0f,
          0.0f, 1.0f,  0.0f, -10.0f,
          0.0f, 0.0f, -1.0f,   0.0f,
          0.0f, 0.0f,  0.0f,   1.0f,
        };
      }
      case Category.Cup: {
        return new float[] {
          1.0f,  0.0f, 0.0f,    0.0f,
          0.0f,  0.0f, 1.0f, -0.001f,
          0.0f, -1.0f, 0.0f,    0.0f,
          0.0f,  0.0f, 0.0f,    1.0f,
        };
      }
      default: {
        return new float[] {
          1.0f,  0.0f, 0.0f, 0.0f,
          0.0f,  0.0f, 1.0f, 0.0f,
          0.0f, -1.0f, 0.0f, 0.0f,
          0.0f,  0.0f, 0.0f, 1.0f,
        };
      }
    }
  }

  string GetObjAssetName(Category category) {
    switch (category) {
      case Category.Camera: {
        return "camera.obj.bytes";
      }
      case Category.Chair: {
        return "chair.obj.bytes";
      }
      case Category.Cup: {
        return "cup.obj.bytes";
      }
      default: {
        return "sneaker.obj.bytes";
      }
    }
  }

  string GetModelAssetName(Category category) {
    switch (category) {
      case Category.Camera: {
        return "object_detection_3d_camera.bytes";
      }
      case Category.Chair: {
        return "object_detection_3d_chair.bytes";
      }
      case Category.Cup: {
        return "object_detection_3d_chair.bytes";
      }
      default: {
        return "object_detection_3d_sneakers.bytes";
      }
    }
  }

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("object_detection_ssd_mobilenetv2_oidv4_fp16.bytes");
    PrepareDependentAsset("object_detection_oidv4_labelmap.txt");
    PrepareDependentAsset("box.obj.bytes");
    PrepareDependentAsset(GetObjAssetName(category));
    PrepareDependentAsset(GetModelAssetName(category), "object_detection_3d.bytes", true);
  }
}
