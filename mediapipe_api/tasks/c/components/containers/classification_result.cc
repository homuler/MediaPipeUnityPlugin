#include "mediapipe_api/tasks/c/components/containers/classification_result.h"

MpReturnCode mp_Packet__GetClassifications(mediapipe::Packet* packet, Classifications* value_out) {
  TRY_ALL
    // get ClassificationList and convert it to Classifications
    auto proto = packet->Get<mediapipe::ClassificationList>();
    auto classifications_in = mediapipe::tasks::components::containers::ConvertToClassifications(proto);

    // cf. mediapipe::tasks::c::components::containers::CppConvertToClassificationResult
    auto classifications_out = Classifications{};
    classifications_out.categories_count = classifications_in.categories.size();
    classifications_out.categories =
        classifications_out.categories_count
            ? new Category[classifications_out.categories_count]
            : nullptr;
    for (uint32_t j = 0; j < classifications_out.categories_count; ++j) {
      mediapipe::tasks::c::components::containers::CppConvertToCategory(classifications_in.categories[j],
                           &(classifications_out.categories[j]));
    }

    classifications_out.head_index = classifications_in.head_index;
    classifications_out.head_name =
        classifications_in.head_name.has_value()
            ? strdup(classifications_in.head_name->c_str())
            : nullptr;
    *value_out = classifications_out;
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_tasks_c_components_containers_CppCloseClassifications(Classifications data) {
  // cf. mediapipe::tasks::c::components::containers::CppCloseClassificationResult
  for (uint32_t j = 0; j < data.categories_count; ++j) {
    mediapipe::tasks::c::components::containers::CppCloseCategory(&data.categories[j]);
  }
  delete[] data.categories;
  data.categories = nullptr;

  free(data.head_name);
  data.head_name = nullptr;
}

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
