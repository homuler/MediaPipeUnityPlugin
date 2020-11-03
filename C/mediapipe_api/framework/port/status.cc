#include <utility>
#include "mediapipe_api/framework/port/status.h"

MpReturnCode mp_Status__i_PKc(int code, const char* message, mediapipe::Status** status_out) {
  TRY {
    auto status_code = static_cast<mediapipe::StatusCode>(code);
    *status_out = new mediapipe::Status { status_code, message };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

void mp_Status__delete(mediapipe::Status* status) {
  delete status;
}

MpReturnCode mp_Status__ToString(mediapipe::Status* status, const char** str_out) {
  TRY {
    *str_out = strcpy_to_heap(status->ToString());
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

bool mp_Status__ok(mediapipe::Status* status) {
  return status->ok();
}

int mp_Status__raw_code(mediapipe::Status* status) {
  return status->raw_code();
}
