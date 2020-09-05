#include <vector>
#include "mediapipe_api/framework/formats/landmark.h"

void MpLandmarkListDestroy(MpLandmarkList* landmark_list) {
  delete landmark_list;
}

void MpLandmarkListVectorDestroy(MpLandmarkListVector* landmark_list_vec) {
  delete landmark_list_vec;
}

MpLandmarkList* MpPacketGetNormalizedLandmarkList(MpPacket* packet) {
  auto& landmark_list = packet->impl->Get<mediapipe::NormalizedLandmarkList>();
  int size = landmark_list.landmark_size();

  MpLandmark* landmarks = new MpLandmark[size];

  for (int i = 0; i < size; ++i) {
    const auto& landmark = landmark_list.landmark(i);
    landmarks[i] = MpLandmark { landmark.x(), landmark.y(), landmark.z(), landmark.visibility() };
  }

  return new MpLandmarkList { landmarks, size };
}

MpLandmarkListVector* MpPacketGetNormalizedLandmarkListVector(MpPacket* packet) {
  auto& landmark_list_vec = packet->impl->Get<std::vector<mediapipe::NormalizedLandmarkList>>();
  int size = landmark_list_vec.size();
  int* size_list = new int[size];
  int* size_list_ptr = size_list;
  int landmark_size {};

  for (const auto& landmark_list : landmark_list_vec) {
    *size_list_ptr = landmark_list.landmark_size();
    landmark_size += *size_list_ptr;
    ++size_list_ptr;
  }

  MpLandmark* landmarks = new MpLandmark[landmark_size];
  MpLandmark* landmarks_ptr = landmarks;

  for (const auto& landmark_list : landmark_list_vec) {
    for (int i = 0; i < landmark_list.landmark_size(); ++i) {
      const auto& landmark = landmark_list.landmark(i);
      landmarks[i] = MpLandmark { landmark.x(), landmark.y(), landmark.z(), landmark.visibility() };
    }
  }

  return new MpLandmarkListVector {
    landmarks,
    size_list,
    size,
  };
}
