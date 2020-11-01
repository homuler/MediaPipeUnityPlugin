#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_

#include "mediapipe/framework/formats/classification.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI_EXPORT extern MpSerializedProto* MpPacketGetClassificationList(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_
