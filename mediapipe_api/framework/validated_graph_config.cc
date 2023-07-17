#include "mediapipe_api/framework/validated_graph_config.h"
#include "mediapipe_api/external/absl/statusor.h"

MpReturnCode mp_ValidatedGraphConfig__(mediapipe::ValidatedGraphConfig** config_out) {
  TRY
    *config_out = new mediapipe::ValidatedGraphConfig();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_ValidatedGraphConfig__delete(mediapipe::ValidatedGraphConfig* config) { delete config; }

MpReturnCode mp_ValidatedGraphConfig__Initialize__Rcgc(mediapipe::ValidatedGraphConfig* config, const char* serialized_config, int size,
                                                       absl::Status** status_out) {
  TRY
    auto graph_config = ParseFromStringAsProto<mediapipe::CalculatorGraphConfig>(serialized_config, size);
    auto status = config->Initialize(graph_config);
    *status_out = new absl::Status(std::move(status));
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ValidatedGraphConfig__Initialize__PKc(mediapipe::ValidatedGraphConfig* config, const char* graph_type, absl::Status** status_out) {
  TRY
    auto status = config->Initialize(graph_type);
    *status_out = new absl::Status(std::move(status));
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool mp_ValidatedGraphConfig__Initialized(mediapipe::ValidatedGraphConfig* config) { return config->Initialized(); }

MpReturnCode mp_ValidatedGraphConfig__ValidateRequiredSidePackets__Rsp(mediapipe::ValidatedGraphConfig* config, SidePackets* side_packets,
                                                                       absl::Status** status_out) {
  TRY
    auto status = config->ValidateRequiredSidePackets(*side_packets);
    *status_out = new absl::Status(std::move(status));
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ValidatedGraphConfig__Config(mediapipe::ValidatedGraphConfig* config, mp_api::SerializedProto* value_out) {
  TRY
    SerializeProto(config->Config(), value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ValidatedGraphConfig__InputStreamInfos(mediapipe::ValidatedGraphConfig* config, mp_api::StructArray<mp_api::EdgeInfo>* value_out) {
  TRY
    mp_api::CopyEdgeInfoVector(config->InputStreamInfos(), value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ValidatedGraphConfig__OutputStreamInfos(mediapipe::ValidatedGraphConfig* config, mp_api::StructArray<mp_api::EdgeInfo>* value_out) {
  TRY
    mp_api::CopyEdgeInfoVector(config->OutputStreamInfos(), value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ValidatedGraphConfig__InputSidePacketInfos(mediapipe::ValidatedGraphConfig* config, mp_api::StructArray<mp_api::EdgeInfo>* value_out) {
  TRY
    mp_api::CopyEdgeInfoVector(config->InputSidePacketInfos(), value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ValidatedGraphConfig__OutputSidePacketInfos(mediapipe::ValidatedGraphConfig* config, mp_api::StructArray<mp_api::EdgeInfo>* value_out) {
  TRY
    mp_api::CopyEdgeInfoVector(config->OutputSidePacketInfos(), value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

int mp_ValidatedGraphConfig__OutputStreamIndex__PKc(mediapipe::ValidatedGraphConfig* config, const char* name) { return config->OutputStreamIndex(name); }

int mp_ValidatedGraphConfig__OutputSidePacketIndex__PKc(mediapipe::ValidatedGraphConfig* config, const char* name) {
  return config->OutputSidePacketIndex(name);
}

int mp_ValidatedGraphConfig__OutputStreamToNode__PKc(mediapipe::ValidatedGraphConfig* config, const char* name) { return config->OutputStreamToNode(name); }

MpReturnCode mp_ValidatedGraphConfig__RegisteredSidePacketTypeName(mediapipe::ValidatedGraphConfig* config, const char* name,
                                                                   absl::Status** status_out, const char** string_out) {
  TRY
    auto status_or_string = config->RegisteredSidePacketTypeName(name);
    copy_absl_StatusOrString(std::move(status_or_string), status_out, string_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ValidatedGraphConfig__RegisteredStreamTypeName(mediapipe::ValidatedGraphConfig* config, const char* name,
                                                               absl::Status** status_out, const char** string_out) {
  TRY
    auto status_or_string = config->RegisteredStreamTypeName(name);
    copy_absl_StatusOrString(std::move(status_or_string), status_out, string_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ValidatedGraphConfig__Package(mediapipe::ValidatedGraphConfig* config, const char** str_out) {
  TRY
    *str_out = strcpy_to_heap(config->Package());
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool mp_ValidatedGraphConfig_IsReservedExecutorName(const char* name) { return mediapipe::ValidatedGraphConfig::IsReservedExecutorName(name); }

bool mp_ValidatedGraphConfig__IsExternalSidePacket__PKc(mediapipe::ValidatedGraphConfig* config, const char* name) {
  return config->IsExternalSidePacket(name);
}

void mp_api_EdgeInfoArray__delete(mp_api::EdgeInfo* edge_info_vector_data, int size) {
  auto edge_info = edge_info_vector_data;
  for (auto i = 0; i < size; ++i) {
    delete (edge_info++)->name;
  }
  delete[] edge_info_vector_data;
}
