#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_

#include "mediapipe/framework/formats/rect.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetRect(mediapipe::Packet* packet, mp_api::SerializedProto** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetRectVector(mediapipe::Packet* packet, mp_api::SerializedProtoVector** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetNormalizedRect(mediapipe::Packet* packet, mp_api::SerializedProto** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetNormalizedRectVector(mediapipe::Packet* packet, mp_api::SerializedProtoVector** value_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_
