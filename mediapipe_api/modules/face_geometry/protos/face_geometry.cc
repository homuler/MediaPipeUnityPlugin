#include "mediapipe_api/modules/face_geometry/protos/face_geometry.h"

MpReturnCode mp_Packet__GetFaceGeometry(mediapipe::Packet* packet, mp_api::SerializedProto** value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::face_geometry::FaceGeometry>(packet, value_out);
}

MpReturnCode mp_Packet__GetFaceGeometryVector(mediapipe::Packet* packet, mp_api::SerializedProtoVector** value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::face_geometry::FaceGeometry>(packet, value_out);
}
