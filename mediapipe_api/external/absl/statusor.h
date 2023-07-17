// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_EXTERNAL_ABSL_STATUSOR_H_
#define MEDIAPIPE_API_EXTERNAL_ABSL_STATUSOR_H_

#include <utility>

#include "absl/status/statusor.h"
#include "mediapipe_api/common.h"

inline void copy_absl_StatusOrString(absl::StatusOr<std::string>&& status_or_string, absl::Status** status_out, const char** string_out) {
  *status_out = new absl::Status{status_or_string.status()};
  if (status_or_string.ok()) {
    *string_out = strcpy_to_heap(status_or_string.value());
  }
}

inline void copy_absl_StatusOrString(absl::StatusOr<std::string>&& status_or_string, absl::Status** status_out, const char** string_out, int* size_out) {
  *status_out = new absl::Status{status_or_string.status()};
  if (status_or_string.ok()) {
    auto& str = status_or_string.value();
    auto length = str.size();
    auto bytes = new char[length];
    memcpy(bytes, str.c_str(), length);

    *string_out = bytes;
    *size_out = length;
  }
}


#endif  // MEDIAPIPE_API_EXTERNAL_ABSL_STATUSOR_H_
