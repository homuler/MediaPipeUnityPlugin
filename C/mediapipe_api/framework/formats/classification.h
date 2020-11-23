#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_

#include "mediapipe/framework/formats/classification.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetClassificationList(mediapipe::Packet* packet, mp_api::SerializedProto** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetClassificationListVector(mediapipe::Packet* packet, mp_api::SerializedProtoVector** value_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_
