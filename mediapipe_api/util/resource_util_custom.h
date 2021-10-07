// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_UTIL_RESOURCE_UTIL_CUSTOM_H_
#define MEDIAPIPE_API_UTIL_RESOURCE_UTIL_CUSTOM_H_

#include <string>

#include "mediapipe/util/resource_util_custom.h"
#include "mediapipe_api/common.h"

extern "C" {

typedef bool ResourceProvider(const char* path, std::string* output);
typedef const char* PathResolver(const char* path);

MP_CAPI(void) mp__SetCustomGlobalResourceProvider__P(ResourceProvider* resource_provider);
MP_CAPI(void) mp__SetCustomGlobalPathResolver__P(PathResolver* path_resolver);

}  // extern "C"

#endif  // MEDIAPIPE_API_UTIL_RESOURCE_UTIL_CUSTOM_H_
