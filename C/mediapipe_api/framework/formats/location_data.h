#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LOCATION_DATA_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LOCATION_DATA_H_

#include <string>
#include "mediapipe/framework/formats/location_data.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"
#include "mediapipe_api/framework/formats/annotation/rasterization.h"

extern "C" {

typedef struct MpBoundingBox {
  int xmin;
  int ymin;
  int width;
  int height;

  MpBoundingBox() = default;

  inline MpBoundingBox(const mediapipe::LocationData_BoundingBox& bounding_box) :
    xmin { bounding_box.xmin() },
    ymin { bounding_box.ymin() },
    width { bounding_box.width() },
    height { bounding_box.height() } {}
} MpBoundingBox;

typedef struct MpRelativeBoundingBox {
  float xmin;
  float ymin;
  float width;
  float height;

  MpRelativeBoundingBox() = default;

  inline MpRelativeBoundingBox(const mediapipe::LocationData_RelativeBoundingBox& relative_bounding_box) :
    xmin { relative_bounding_box.xmin() },
    ymin { relative_bounding_box.ymin() },
    width { relative_bounding_box.width() },
    height { relative_bounding_box.height() } {}
} MpRelativeBoundingBox;

typedef struct MpBinaryMask {
  int width;
  int height;
  MpRasterization* rasterization;

  MpBinaryMask() = default;

  inline MpBinaryMask(const mediapipe::LocationData_BinaryMask& binary_mask) :
    width { binary_mask.width() },
    height { binary_mask.height() },
    rasterization { new MpRasterization { binary_mask.rasterization() } } {}

  ~MpBinaryMask() {
    delete rasterization;
  }
} MpBinaryMask;

typedef struct MpRelativeKeypoint {
  float x;
  float y;
  const char* keypoint_label;
  float score;

  MpRelativeKeypoint() = default;

  inline MpRelativeKeypoint(const mediapipe::LocationData_RelativeKeypoint& relative_keypoint) :
    x { relative_keypoint.x() },
    y { relative_keypoint.y() },
    keypoint_label { strcpy_to_heap(relative_keypoint.keypoint_label()) },
    score { relative_keypoint.score() } {}

  ~MpRelativeKeypoint() {
    delete[] keypoint_label;
  }
} MpRelativeKeypoint;

typedef struct MpLocationData {
  int format;
  MpBoundingBox* bounding_box;
  MpRelativeBoundingBox* relative_bounding_box;
  MpBinaryMask* mask;
  MpRelativeKeypoint** relative_keypoints;
  int relative_keypoints_size;

  MpLocationData() = default;

  inline MpLocationData(const mediapipe::LocationData& location_data) :
    format { static_cast<int>(location_data.format()) },
    bounding_box {
      new MpBoundingBox { location_data.bounding_box() }
    },
    relative_bounding_box {
      new MpRelativeBoundingBox { location_data.relative_bounding_box() }
    },
    mask { new MpBinaryMask { location_data.mask() } },
    relative_keypoints_size { location_data.relative_keypoints_size() }
  {
    relative_keypoints = new MpRelativeKeypoint*[relative_keypoints_size];

    for (auto i = 0; i < relative_keypoints_size; ++i) {
      relative_keypoints[i] = new MpRelativeKeypoint { location_data.relative_keypoints(i) };
    }
  }

  ~MpLocationData() {
    delete bounding_box;
    delete relative_bounding_box;
    delete mask;

    for (auto i = 0; i < relative_keypoints_size; ++i) {
      delete relative_keypoints[i];
    }

    delete[] relative_keypoints;
  }
} MpLocationData;

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_LOCATION_DATA_H_
