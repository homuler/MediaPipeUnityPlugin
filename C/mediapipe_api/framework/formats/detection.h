#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_

#include "mediapipe/framework/formats/detection.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI_EXPORT MpSerializedProto* MpPacketGetDetection(MpPacket* packet);
MP_CAPI_EXPORT MpSerializedProtoVector* MpPacketGetDetectionVector(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_
