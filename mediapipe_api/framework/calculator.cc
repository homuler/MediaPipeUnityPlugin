#include "mediapipe_api/framework/calculator.h"
#include "mediapipe/framework/calculator.pb.h"

bool mp_api__ConvertFromCalculatorGraphConfigTextFormat(const char* config_text, mp_api::SerializedProto* value_out) {
  mp_api::SerializedProto output;
  auto result =  ConvertFromTextFormat<mediapipe::CalculatorGraphConfig>(config_text, output);
  *value_out = output;

  return result;
}
