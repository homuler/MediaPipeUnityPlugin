#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_

#include <vector>
#include "mediapipe/framework/formats/detection.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

typedef struct MpDetectionVector {
  const char** detections;
  int* size_list;
  int size;

  MpDetectionVector() = default;

  inline MpDetectionVector(const std::vector<mediapipe::Detection>& detection_vec) :
    size { detection_vec.size() }
  {
    detections = new const char*[size];
    size_list = new int[size];

    int i = 0;
    for (auto& detection : detection_vec) {
      auto length = detection.ByteSizeLong();
      size_list[i] = length;

      auto bytes = new char[length];
      detection.SerializeToArray(bytes, length);
      detections[i] = bytes;
      ++i;
    }
  }

  ~MpDetectionVector() {
    for (auto i = 0; i < size; ++i) {
      delete detections[i];
    }

    delete[] detections;
    delete[] size_list;
  }
} MpDetectionVector;

MP_CAPI_EXPORT extern void MpDetectionVectorDestroy(MpDetectionVector* detection_vec);
MP_CAPI_EXPORT extern MpDetectionVector* MpPacketGetDetectionVector(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_
