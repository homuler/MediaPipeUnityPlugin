#include "mediapipe_api/framework/formats/detection.h"

MpSerializedProto* MpPacketGetDetection(MpPacket* packet) {
  return MpPacketGetProto<mediapipe::Detection>(packet);
}

MpSerializedProtoVector* MpPacketGetDetectionVector(MpPacket* packet) {
  return MpPacketGetProtoVector<mediapipe::Detection>(packet);
}
