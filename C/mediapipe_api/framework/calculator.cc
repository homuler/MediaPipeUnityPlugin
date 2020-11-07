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

int mp_CalculatorGraphConfig__ByteSizeLong(mediapipe::CalculatorGraphConfig* config) {
  return config->ByteSizeLong();
}

MpReturnCode mp_CalculatorGraphConfig__SerializeAsString(mediapipe::CalculatorGraphConfig* config, const char** str_out) {
  TRY {
    if (config->IsInitialized()) {
      auto size = config->ByteSizeLong();
      auto buffer = new char[size + 1];
      config->SerializeToArray(buffer, size);
      *str_out = buffer;
    } else {
      *str_out = nullptr;
    }
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraphConfig__DebugString(mediapipe::CalculatorGraphConfig* config, const char** str_out) {
  TRY {
    *str_out = strcpy_to_heap(config->DebugString());
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}
