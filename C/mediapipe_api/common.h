#ifndef C_MEDIAPIPE_API_COMMON_H_
#define C_MEDIAPIPE_API_COMMON_H_

#ifdef DLL_EXPORTS
#define MP_CAPI_EXPORT __declspec(dllexport)
#else
#define MP_CAPI_EXPORT
#endif

#include <string>

extern inline const char* strcpy_to_heap(const std::string& str) {
  if (str.empty()) { return nullptr; }

  auto str_ptr = new char[str.length() + 1];
  snprintf(str_ptr, str.length() + 1, str.c_str());

  return str_ptr;
}

#endif  // C_MEDIAPIPE_API_COMMON_H_
