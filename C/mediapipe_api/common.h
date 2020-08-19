#ifndef C_MEDIAPIPE_API_COMMON_H_
#define C_MEDIAPIPE_API_COMMON_H_

#ifdef DLL_EXPORTS
#define MP_CAPI_EXPORT __declspec(dllexport)
#else
#define MP_CAPI_EXPORT
#endif

#endif  // C_MEDIAPIPE_API_COMMON_H_
