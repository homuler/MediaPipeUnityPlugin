#ifndef C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_
#define C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_

#include <memory>
#include <utility>
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_api__ConvertFromCalculatorGraphConfigTextFormat(const char* config_text, mp_api::SerializedProto** value_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_


