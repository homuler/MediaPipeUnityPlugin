#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_

#include "mediapipe/framework/formats/landmark.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

typedef struct {
  float x;
  float y;
  float z;
  float visibility;
} MpLandmark;

typedef struct MpLandmarkList {
  MpLandmark* landmarks;
  int size;

  ~MpLandmarkList() {
    delete[] landmarks;
  }
} MpLandmarkList;

typedef struct MpLandmarkListVector {
  MpLandmark* landmarks;
  int* size_list;
  int size;

  ~MpLandmarkListVector() {
    delete[] landmarks;
    delete[] size_list;
  }
} MpLandmarkListVector;

MP_CAPI_EXPORT extern void MpLandmarkListDestroy(MpLandmarkList* landmark_list);
MP_CAPI_EXPORT extern void MpLandmarkListVectorDestroy(MpLandmarkListVector* landmark_list_vec);

MP_CAPI_EXPORT extern MpLandmarkList* MpPacketGetNormalizedLandmarkList(MpPacket* packet);
MP_CAPI_EXPORT extern MpLandmarkListVector* MpPacketGetNormalizedLandmarkListVector(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_
