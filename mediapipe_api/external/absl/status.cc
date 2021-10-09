// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/external/absl/status.h"

MpReturnCode absl_Status__i_PKc(int code, const char* message, absl::Status** status_out) {
  TRY
    auto status_code = static_cast<absl::StatusCode>(code);
    *status_out = new absl::Status{status_code, message};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void absl_Status__delete(absl::Status* status) { delete status; }

MpReturnCode absl_Status__ToString(absl::Status* status, const char** str_out) {
  TRY
    *str_out = strcpy_to_heap(status->ToString());
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool absl_Status__ok(absl::Status* status) { return status->ok(); }

int absl_Status__raw_code(absl::Status* status) { return status->raw_code(); }
