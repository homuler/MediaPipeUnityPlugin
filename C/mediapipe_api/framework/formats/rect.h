#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_

#include "mediapipe/framework/formats/rect.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI_EXPORT extern MpSerializedProto* MpPacketGetRect(MpPacket* packet);
MP_CAPI_EXPORT extern MpSerializedProtoVector* MpPacketGetRectVector(MpPacket* packet);
MP_CAPI_EXPORT extern MpSerializedProto* MpPacketGetNormalizedRect(MpPacket* packet);
MP_CAPI_EXPORT extern MpSerializedProtoVector* MpPacketGetNormalizedRectVector(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_
