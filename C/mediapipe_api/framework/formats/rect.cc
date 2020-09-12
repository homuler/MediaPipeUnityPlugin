#include "mediapipe_api/framework/formats/rect.h"

MpSerializedProto* MpPacketGetRect(MpPacket* packet) {
  return MpPacketGetProto<mediapipe::Rect>(packet);
}

MpSerializedProtoVector* MpPacketGetRectVector(MpPacket* packet) {
  return MpPacketGetProtoVector<mediapipe::Rect>(packet);
}

MpSerializedProto* MpPacketGetNormalizedRect(MpPacket* packet) {
  return MpPacketGetProto<mediapipe::NormalizedRect>(packet);
}

MpSerializedProtoVector* MpPacketGetNormalizedRectVector(MpPacket* packet) {
  return MpPacketGetProtoVector<mediapipe::NormalizedRect>(packet);
}
