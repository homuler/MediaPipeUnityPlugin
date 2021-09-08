#ifndef C_MEDIAPIPE_API_MODULES_OBJECTRON_CALCULATORS_ANNOTATION_DATA_H_
#define C_MEDIAPIPE_API_MODULES_OBJECTRON_CALCULATORS_ANNOTATION_DATA_H_

#include "mediapipe/modules/objectron/calculators/annotation_data.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetFrameAnnotation(mediapipe::Packet* packet, mp_api::SerializedProto** value_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_MODULES_OBJECTRON_CALCULATORS_ANNOTATION_DATA_H_
