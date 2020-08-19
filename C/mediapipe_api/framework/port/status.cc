#include <utility>
#include "mediapipe_api/framework/port/status.h"

MpStatus* MpStatusCreate(int code, const char* message) {
  auto status_code = static_cast<mediapipe::StatusCode>(code);
  mediapipe::Status status { status_code, message };

  return new MpStatus { std::move(status) };
}

void MpStatusDestroy(MpStatus* status) {
  delete status;
}

bool MpStatusOk(MpStatus* status) {
  return status->impl->ok();
}

int GetMpStatusRawCode(MpStatus* status) {
  return status->impl->raw_code();
}

const char* MpStatusToString(MpStatus* status) {
  auto text = status->impl->ToString();

  char* result = new char[text.size() + 1];
  snprintf(result, text.size() + 1, text.c_str());

  return result;
}
