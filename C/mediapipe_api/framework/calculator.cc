#include "mediapipe_api/framework/calculator.h"
#include "mediapipe/framework/port/parse_text_proto.h"

MpReturnCode google_protobuf_TextFormat__ParseFromStringAsCalculatorGraphConfig__PKc(const char* input,
                                                                                     mediapipe::CalculatorGraphConfig** config_out) {
  TRY {
    auto config = new mediapipe::CalculatorGraphConfig {};
    auto result = google::protobuf::TextFormat::ParseFromString(input, config);

    *config_out = result ? config : nullptr;
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

void mp_CalculatorGraphConfig__delete(mediapipe::CalculatorGraphConfig* config) {
  delete config;
}
