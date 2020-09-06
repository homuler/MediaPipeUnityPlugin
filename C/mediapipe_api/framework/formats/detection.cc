#include "mediapipe_api/framework/formats/detection.h"

void MpDetectionVectorDestroy(MpDetectionVector* detection_vec) {
  delete detection_vec;
}

MpDetectionVector* MpPacketGetDetectionVector(MpPacket* packet) {
  auto& detection_vec = packet->impl->Get<std::vector<mediapipe::Detection>>();

  return new MpDetectionVector { detection_vec };
}
