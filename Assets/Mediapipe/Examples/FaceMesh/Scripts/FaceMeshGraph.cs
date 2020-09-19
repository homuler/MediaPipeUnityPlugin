using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class FaceMeshGraph : DemoGraph {
  [SerializeField] private bool useGPU = true;

  private const string landmarksStream = "multi_face_landmarks";
  private const string landmarksPresenceStream = "face_landmarks_presence";

  private OutputStreamPoller<List<NormalizedLandmarkList>> landmarksStreamPoller;
  private OutputStreamPoller<bool> landmarksPresenceStreamPoller;
  private NormalizedLandmarkListVectorPacket landmarkListPacket;
  private BoolPacket landmarksPresencePacket;

  public override Status StartRun(SidePacket sidePacket) {
    landmarksStreamPoller = graph.AddOutputStreamPoller<List<NormalizedLandmarkList>>(landmarksStream).ConsumeValue();
    landmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(landmarksPresenceStream).ConsumeValue();

    landmarkListPacket = new NormalizedLandmarkListVectorPacket();
    landmarksPresencePacket = new BoolPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, Color32[] pixelData) {
    var texture = screenController.GetScreen();
    texture.SetPixels32(pixelData);

    List<NormalizedLandmarkList> landmarkListVec = FetchNextLandmarks();

    if (landmarkListVec != null) {
      for (var i = 0; i < landmarkListVec.Count; i++) {
        Color color = GetLandmarkColor(i);

        foreach (var landmark in landmarkListVec[i].Landmark) {
          landmark.Draw(texture, color);
        }
      }
    }

    texture.Apply();
  }

  private List<NormalizedLandmarkList> FetchNextLandmarks() {
    if (!landmarksPresenceStreamPoller.Next(landmarksPresencePacket)) { // blocks
    Debug.LogWarning($"Failed to fetch next packet from {landmarksPresenceStream}");
      return null;
    }

    bool isLandmarkPresent = landmarksPresencePacket.GetValue();

    if (!isLandmarkPresent) {
      return null;
    }

    if (!landmarksStreamPoller.Next(landmarkListPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {landmarksStream}");
      return null;
    }

    return landmarkListPacket.GetValue();
  }

  private Color GetLandmarkColor(int index) {
    // TODO: change color according to the index
    return Color.green;
  }

  public override bool shouldUseGPU() {
    return useGPU;
  }
}
