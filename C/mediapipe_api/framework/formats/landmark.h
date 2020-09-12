#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_

#include "mediapipe/framework/formats/landmark.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI_EXPORT extern MpSerializedProto* MpPacketGetLandmarkList(MpPacket* packet);
MP_CAPI_EXPORT extern MpSerializedProtoVector* MpPacketGetLandmarkListVector(MpPacket* packet);
MP_CAPI_EXPORT extern MpSerializedProto* MpPacketGetNormalizedLandmarkList(MpPacket* packet);
MP_CAPI_EXPORT extern MpSerializedProtoVector* MpPacketGetNormalizedLandmarkListVector(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_
