#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_

#include "mediapipe/framework/formats/classification.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

typedef struct MpClassification {
  int index;
  float score;
  const char* label;

  ~MpClassification() {
    delete[] label;
  }
} MpClassification;

typedef struct MpClassificationList {
  MpClassification* classifications;
  int size;

  ~MpClassificationList() {
    delete[] classifications;
  }
} MpClassificationList;

MP_CAPI_EXPORT extern void MpClassificationListDestroy(MpClassificationList* classification_list);
MP_CAPI_EXPORT extern MpClassification* MpClassificationListClassifications(MpClassificationList* classification_list);
MP_CAPI_EXPORT extern int MpClassificationListSize(MpClassificationList* classification_list);

MP_CAPI_EXPORT extern MpClassificationList* MpPacketGetClassificationList(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_CLASSIFICATION_H_
