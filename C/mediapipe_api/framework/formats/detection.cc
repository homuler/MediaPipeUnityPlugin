#include "mediapipe_api/framework/formats/detection.h"

MpSerializedProtoVector* MpPacketGetDetectionVector(MpPacket* packet) {
  return MpPacketGetProtoVector<mediapipe::Detection>(packet);
}
