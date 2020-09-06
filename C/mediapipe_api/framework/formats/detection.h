#ifndef C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_
#define C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_

#include <vector>
#include "mediapipe/framework/formats/detection.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"
#include "mediapipe_api/framework/formats/location_data.h"

extern "C" {

typedef struct MpAssociatedDetection {
  int id;
  float confidence;

  MpAssociatedDetection() = default;

  inline MpAssociatedDetection(const mediapipe::Detection_AssociatedDetection& associated_detection) :
    id { associated_detection.id() },
    confidence { associated_detection.confidence() } {}
} MpAssociatedDetection;

typedef struct MpDetection {
  const char** label;
  int* label_id;
  float* score;
  MpLocationData* location_data;
  const char* feature_tag;
  const char* track_id;
  int64 detection_id;
  MpAssociatedDetection** associated_detections;
  const char** display_name;
  int64 timestamp_usec;
  int score_size;
  int associated_detections_size;

  MpDetection() = default;

  inline MpDetection(const mediapipe::Detection& detection) :
    score_size { detection.score_size() },
    location_data { new MpLocationData { detection.location_data() } },
    feature_tag { strcpy_to_heap(detection.feature_tag()) },
    track_id { strcpy_to_heap(detection.track_id()) },
    detection_id { detection.detection_id() },
    associated_detections_size { detection.associated_detections_size() },
    timestamp_usec { detection.timestamp_usec() }
  {
    bool is_label_used = detection.label_size() > 0;
    bool is_label_id_used = detection.label_id_size() > 0;
    bool is_display_name_used = detection.display_name_size() > 0;

    // i-th label or label_id has a score encoded by the i-th element in score.
    // Either string or integer labels must be used but not both at the same time.
    label = is_label_used ? new const char*[score_size] : nullptr;
    label_id = is_label_id_used ? new int[score_size] : nullptr;
    score = new float[score_size];
    display_name = is_display_name_used ? new const char*[score_size] : nullptr;

    associated_detections = new MpAssociatedDetection*[associated_detections_size];

    for (auto i = 0; i < score_size; ++i) {
      if (is_label_used) {
        label[i] = strcpy_to_heap(detection.label(i));
      }

      if (is_label_id_used) {
        label_id[i] = detection.label_id(i);
      }

      score[i] = detection.score(i);

      if (is_display_name_used) {
        display_name[i] = strcpy_to_heap(detection.display_name(i));
      }
    }

    for (auto i = 0; i < associated_detections_size; ++i) {
      associated_detections[i] = new MpAssociatedDetection { detection.associated_detections(i) };
    }
  }

  ~MpDetection() {
    delete[] label_id;
    delete[] score;
    delete location_data;
    delete[] feature_tag;
    delete[] track_id;

    for (auto i = 0; i < associated_detections_size; ++i) {
      delete associated_detections[i];
    }

    delete[] associated_detections;

    bool is_label_used = label != nullptr;
    bool is_display_name_used = display_name != nullptr;

    for (auto i = 0; i < score_size; ++i) {
      if (is_label_used) { delete[] label[i]; }
      if (is_display_name_used) { delete[] display_name[i]; }
    }

    delete[] label;
    delete[] display_name;
  }
} MpDetection;

typedef struct MpDetectionVector {
  MpDetection** detections;
  int size;

  MpDetectionVector() = default;

  inline MpDetectionVector(const std::vector<mediapipe::Detection>& detection_vec) :
    size { detection_vec.size() }
  {
    detections = new MpDetection*[size];

    int i = 0;
    for (auto& detection : detection_vec) {
      detections[i++] = new MpDetection { detection };
    }
  }

  ~MpDetectionVector() {
    for (auto i = 0; i < size; ++i) {
      delete detections[i];
    }

    delete[] detections;
  }
} MpDetectionVector;

MP_CAPI_EXPORT extern void MpDetectionVectorDestroy(MpDetectionVector* detection_vec);
MP_CAPI_EXPORT extern MpDetectionVector* MpPacketGetDetectionVector(MpPacket* packet);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_FORMATS_DETECTION_H_
