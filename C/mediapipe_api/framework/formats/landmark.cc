#include "mediapipe_api/framework/formats/landmark.h"

MpSerializedProto* MpPacketGetLandmarkList(MpPacket* packet) {
  return MpPacketGetProto<mediapipe::LandmarkList>(packet);
}

MpSerializedProtoVector* MpPacketGetLandmarkListVector(MpPacket* packet) {
  return MpPacketGetProtoVector<mediapipe::LandmarkList>(packet);
}

MpSerializedProto* MpPacketGetNormalizedLandmarkList(MpPacket* packet) {
  return MpPacketGetProto<mediapipe::NormalizedLandmarkList>(packet);
}

MpSerializedProtoVector* MpPacketGetNormalizedLandmarkListVector(MpPacket* packet) {
  return MpPacketGetProtoVector<mediapipe::NormalizedLandmarkList>(packet);
}
