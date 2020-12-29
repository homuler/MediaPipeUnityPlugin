#include "mediapipe_api/framework/calculator.h"
#include "mediapipe/framework/calculator.pb.h"

MpReturnCode mp_api__ConvertFromCalculatorGraphConfigTextFormat(const char* config_text, mp_api::SerializedProto** value_out) {
  TRY {
    *value_out = ConvertFromTextFormat<mediapipe::CalculatorGraphConfig>(config_text);
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}
