#include "mediapipe_api/framework/calculator.h"
#include "mediapipe/framework/calculator.pb.h"

bool mp_api__ConvertFromCalculatorGraphConfigTextFormat(const char* config_text, mp_api::SerializedProto* value_out) {
  return ConvertFromTextFormat<mediapipe::CalculatorGraphConfig>(config_text, value_out);
}
