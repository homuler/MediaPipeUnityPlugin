#include "mediapipe_api/tasks/c/components/containers/landmark.h"

MpReturnCode mp_Packet__GetNormalizedLandmarksVector(mediapipe::Packet* packet, mp_api::StructArray<NormalizedLandmarks>* value_out) {
  TRY_ALL
    // get std::vector<NormalizedLandmarkList> and convert it to NormalizedLandmarks*
    auto proto_vec = packet->Get<std::vector<mediapipe::NormalizedLandmarkList>>();
    auto vec_size = proto_vec.size();
    auto data = new NormalizedLandmarks[vec_size];

    for (auto i = 0; i < vec_size; ++i) {
      auto landmarks = mediapipe::tasks::components::containers::ConvertToNormalizedLandmarks(proto_vec[i]);
      mediapipe::tasks::c::components::containers::CppConvertToNormalizedLandmarks(landmarks.landmarks, &data[i]);
    }

    value_out->data = data;
    value_out->size = static_cast<int>(vec_size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_api_NormalizedLandmarksArray__delete(NormalizedLandmarks* data, int size) {
  auto landmarks = data;
  for (auto i = 0; i < size; ++i) {
    mediapipe::tasks::c::components::containers::CppCloseNormalizedLandmarks(landmarks++);
  }
  delete[] data;
}
