using Mediapipe;
using System;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class BoxTrackingGraph : OfficialDemoGraph {
  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("ssdlite_object_detection");
    PrepareDependentAsset("ssdlite_object_detection_labelmap");
  }
}
