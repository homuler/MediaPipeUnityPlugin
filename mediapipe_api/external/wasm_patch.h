// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_EXTERNAL_WASM_PATCH_H_
#define MEDIAPIPE_API_EXTERNAL_WASM_PATCH_H_

#include "mediapipe_api/common.h"

#ifdef __EMSCRIPTEN__

#ifdef __cplusplus
extern "C" {
#endif  // __cplusplus

MP_CAPI(FILE*) popen(const char* command, const char* type);

#ifdef __cplusplus
}  // extern "C"
#endif  // __cplusplus

#endif  // __EMSCRIPTEN__

#endif  // MEDIAPIPE_API_EXTERNAL_WASM_PATCH_H_
