#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_

#include "mediapipe/framework/formats/rect.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"

template<typename T>
struct MpRectVectorBase {
  T* rects;
  int size;

  ~MpRectVectorBase() {
    delete[] rects;
  }
};

extern "C" {

typedef struct {
  int x_center;
  int y_center;
  int height;
  int width;
  float rotation;
  int64 rect_id;
} MpRect;

typedef struct {
  float x_center;
  float y_center;
  float height;
  float width;
  float rotation;
  int64 rect_id;
} MpNormalizedRect;

typedef MpRectVectorBase<MpRect> MpRectVector;
typedef MpRectVectorBase<MpNormalizedRect> MpNormalizedRectVector;

MP_CAPI_EXPORT extern void MpRectDestroy(MpRect* rect);
MP_CAPI_EXPORT extern void MpRectVectorDestroy(MpRectVector* rect_vec);
MP_CAPI_EXPORT extern void MpNormalizedRectDestroy(MpNormalizedRect* rect);
MP_CAPI_EXPORT extern void MpNormalizedRectVectorDestroy(MpNormalizedRectVector* rect_vec);

MP_CAPI_EXPORT extern MpRect* MpPacketGetRect(MpPacket* packet);
MP_CAPI_EXPORT extern MpRectVector* MpPacketGetRectVector(MpPacket* packet);
MP_CAPI_EXPORT extern MpNormalizedRect* MpPacketGetNormalizedRect(MpPacket* packet);
MP_CAPI_EXPORT extern MpNormalizedRectVector* MpPacketGetNormalizedRectVector(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_RECT_H_
