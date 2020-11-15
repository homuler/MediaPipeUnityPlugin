#include "mediapipe_api/framework/formats/landmark.h"

MpReturnCode mp_Packet__GetLandmarkList(mediapipe::Packet* packet, mp_api::SerializedProto** value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::LandmarkList>(packet, value_out);
}

MpReturnCode mp_Packet__GetLandmarkListVector(mediapipe::Packet* packet, mp_api::SerializedProtoVector** value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::LandmarkList>(packet, value_out);
}

MpReturnCode mp_Packet__GetNormalizedLandmarkList(mediapipe::Packet* packet, mp_api::SerializedProto** value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::NormalizedLandmarkList>(packet, value_out);
}

MpReturnCode mp_Packet__GetNormalizedLandmarkListVector(mediapipe::Packet* packet, mp_api::SerializedProtoVector** value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::NormalizedLandmarkList>(packet, value_out);
}
