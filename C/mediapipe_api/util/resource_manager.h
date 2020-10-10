#ifndef C_MEDIAPIPE_API_UTIL_RESOURCE_MANAGER_H_
#define C_MEDIAPIPE_API_UTIL_RESOURCE_MANAGER_H_

#include <string>
#include "mediapipe/util/resource_manager.h"
#include "mediapipe_api/common.h"

extern "C" {

typedef const char* CacheFilePathResolver(const char* path);
typedef bool ReadFileHandler(const char* path, std::string* output);

MP_CAPI_EXPORT extern void MpResourceManagerInitialize(CacheFilePathResolver* resolver, ReadFileHandler* handler);
MP_CAPI_EXPORT extern void MpStringCopy(std::string* dst, const char* src, int size);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_UTIL_RESOURCE_MANAGER_H_
