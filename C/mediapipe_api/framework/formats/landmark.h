#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_

#include <vector>
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
  int* size_list;
  int size;

  ~MpLandmarkList() {
    delete[] landmarks;
    delete[] size_list;
  }
} MpLandmarkList;

MP_CAPI_EXPORT extern void MpLandmarkListDestroy(MpLandmarkList* landmark_list);
MP_CAPI_EXPORT extern MpLandmark* MpLandmarkListLandmarks(MpLandmarkList* landmark_list);
MP_CAPI_EXPORT extern int* MpLandmarkListSizeList(MpLandmarkList* landmark_list);
MP_CAPI_EXPORT extern int MpLandmarkListSize(MpLandmarkList* landmark_list);

MP_CAPI_EXPORT extern MpLandmarkList* MpPacketGetNormalizedLandmarkList(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LANDMARK_H_
