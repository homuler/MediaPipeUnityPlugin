#include "mediapipe_api/framework/formats/detection.h"

MpReturnCode mp_Packet__GetDetection(mediapipe::Packet* packet, mp_api::SerializedProto** value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::Detection>(packet, value_out);
}

MpReturnCode mp_Packet__GetDetectionVector(mediapipe::Packet* packet, mp_api::SerializedProtoVector** value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::Detection>(packet, value_out);
}
