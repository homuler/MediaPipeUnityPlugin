#ifndef C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_
#define C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_

#include <memory>
#include <utility>
#include "mediapipe/framework/calculator.pb.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) google_protobuf_TextFormat__ParseFromStringAsCalculatorGraphConfig__PKc(
    const char* input,
    mediapipe::CalculatorGraphConfig** config_out);

MP_CAPI(void) mp_CalculatorGraphConfig__delete(mediapipe::CalculatorGraphConfig* config);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_


