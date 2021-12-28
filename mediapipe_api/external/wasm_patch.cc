// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/external/wasm_patch.h"

FILE* popen(const char *command, const char *type) {
  LOG(ERROR) << "popen is not supported on Web";
  return nullptr;
}
