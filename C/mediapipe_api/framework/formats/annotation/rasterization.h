#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_ANNOTATION_RASTERIZATION_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_ANNOTATION_RASTERIZATION_H_

#include "mediapipe/framework/formats/annotation/rasterization.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

typedef struct MpInterval {
  int y;
  int left_x;
  int right_x;

  MpInterval() = default;

  inline MpInterval(const mediapipe::Rasterization_Interval& interval) :
    y { interval.y() },
    left_x { interval.left_x() },
    right_x { interval.right_x() } {}
} MpInterval;

typedef struct MpRasterization {
  MpInterval** intervals;
  int intervals_size;

  MpRasterization() = default;

  inline MpRasterization(const mediapipe::Rasterization& rasterization) :
    intervals_size { rasterization.interval_size() }
  {
    LOG(INFO) << "intarvals size: " << intervals_size;
    intervals = new MpInterval*[intervals_size];
    for (auto i = 0; i < intervals_size; ++i) {
      intervals[i] = new MpInterval { rasterization.interval(i) };
    }
  }

  ~MpRasterization() {
    for (auto i = 0; i < intervals_size; ++i) {
      delete intervals[i];
    }

    delete[] intervals;
  }
} MpRasterization;

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_ANNOTATION_RASTERIZATION_H_
