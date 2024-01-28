#include "mediapipe_api/tasks/c/components/containers/classification_result.h"

MpReturnCode mp_Packet__GetClassificationResult(mediapipe::Packet* packet, ClassificationResult* value_out) {
  TRY_ALL
    auto proto = packet->Get<mediapipe::tasks::components::containers::proto::ClassificationResult>();
    auto classification_result = mediapipe::tasks::components::containers::ConvertToClassificationResult(proto);
    mediapipe::tasks::c::components::containers::CppConvertToClassificationResult(classification_result, value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__GetClassificationsVector(mediapipe::Packet* packet, ClassificationResult* value_out) {
  TRY_ALL
    // get std::vector<ClassificationList> and convert it to ClassificationResult
    auto proto_vec = packet->Get<std::vector<mediapipe::ClassificationList>>();
    auto vec_size = proto_vec.size();

    auto classifications_vec = std::vector<mediapipe::tasks::components::containers::Classifications>(vec_size);
    for (auto i = 0; i < vec_size; ++i) {
      auto classifications = mediapipe::tasks::components::containers::ConvertToClassifications(proto_vec[i]);
      classifications_vec[i] = classifications;
    }

    mediapipe::tasks::components::containers::ClassificationResult result;
    result.classifications = std::move(classifications_vec);

    mediapipe::tasks::c::components::containers::CppConvertToClassificationResult(result, value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_tasks_c_components_containers_CppCloseClassificationResult(ClassificationResult data) {
  mediapipe::tasks::c::components::containers::CppCloseClassificationResult(&data);
}

MpReturnCode mp_Packet__GetClassificationResultVector(mediapipe::Packet* packet, mp_api::StructArray<ClassificationResult>* value_out) {
  TRY_ALL
    // get std::vector<proto::ClassificationResult> and convert it to ClassificationResult[]
    auto proto_vec = packet->Get<std::vector<mediapipe::tasks::components::containers::proto::ClassificationResult>>();
    auto vec_size = proto_vec.size();

    value_out->size = vec_size;
    if (vec_size > 0) {
      value_out->data = new ClassificationResult[vec_size];

      for (auto i = 0; i < vec_size; ++i) {
        auto classification_result = mediapipe::tasks::components::containers::ConvertToClassificationResult(proto_vec[i]);
        mediapipe::tasks::c::components::containers::CppConvertToClassificationResult(classification_result, &value_out->data[i]);
      }
    } else {
      value_out->data = nullptr;
    }

    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_api_ClassificationResultArray__delete(mp_api::StructArray<ClassificationResult> array) {
  auto classification_result = array.data;
  auto size = array.size;

  for (auto i = 0; i < size; ++i) {
    mediapipe::tasks::c::components::containers::CppCloseClassificationResult(classification_result++);
  }
  delete[] array.data;
}
