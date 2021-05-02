#ifndef C_MEDIAPIPE_API_MODULES_FACE_GEOMETRY_PROTOS_FACE_GEOMETRY_H_
#define C_MEDIAPIPE_API_MODULES_FACE_GEOMETRY_PROTOS_FACE_GEOMETRY_H_

#include "mediapipe/modules/face_geometry/protos/face_geometry.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetFaceGeometry(mediapipe::Packet* packet, mp_api::SerializedProto** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetFaceGeometryVector(mediapipe::Packet* packet, mp_api::SerializedProtoVector** value_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_MODULES_FACE_GEOMETRY_PROTOS_FACE_GEOMETRY_H_
