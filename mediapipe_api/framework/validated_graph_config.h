// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_VALIDATED_GRAPH_CONFIG_H_
#define MEDIAPIPE_API_FRAMEWORK_VALIDATED_GRAPH_CONFIG_H_

#include <map>
#include <memory>
#include <string>

#include "mediapipe/framework/validated_graph_config.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"

namespace mp_api {

typedef struct EdgeInfo {
  int upstream;
  mediapipe::NodeTypeInfo::NodeRef parent_node;
  const char* name;
  bool back_edge;
};

inline void CopyEdgeInfo(const mediapipe::EdgeInfo& src, mp_api::EdgeInfo* dst) {
  dst->upstream = src.upstream;
  dst->parent_node = src.parent_node;
  dst->name = strcpy_to_heap(src.name);
  dst->back_edge = src.back_edge;
}

inline void CopyEdgeInfoVector(const std::vector<mediapipe::EdgeInfo>& src, mp_api::StructArray<mp_api::EdgeInfo>* dst) {
  auto vec_size = src.size();
  auto data = new mp_api::EdgeInfo[vec_size];

  auto it = src.begin();
  auto p = data;

  for (; it != src.end(); ++it, ++p) {
    CopyEdgeInfo(*it, p);
  }
  dst->data = data;
  dst->size = static_cast<int>(vec_size);
}

}  // namespace mp_api

extern "C" {

typedef std::map<std::string, mediapipe::Packet> SidePackets;

MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__(mediapipe::ValidatedGraphConfig** config_out);
MP_CAPI(void) mp_ValidatedGraphConfig__delete(mediapipe::ValidatedGraphConfig* config);

MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__Initialize__Rcgc(mediapipe::ValidatedGraphConfig* config, const char* serialized_config, int size,
                                                                absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__Initialize__PKc(mediapipe::ValidatedGraphConfig* config, const char* graph_type, absl::Status** status_out);

MP_CAPI(bool) mp_ValidatedGraphConfig__Initialized(mediapipe::ValidatedGraphConfig* config);
MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__ValidateRequiredSidePackets__Rsp(mediapipe::ValidatedGraphConfig* config, SidePackets* side_packets,
                                                                                absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__Config(mediapipe::ValidatedGraphConfig* config, mp_api::SerializedProto* value_out);

MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__InputStreamInfos(mediapipe::ValidatedGraphConfig* config, mp_api::StructArray<mp_api::EdgeInfo>* value_out);
MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__OutputStreamInfos(mediapipe::ValidatedGraphConfig* config, mp_api::StructArray<mp_api::EdgeInfo>* value_out);
MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__InputSidePacketInfos(mediapipe::ValidatedGraphConfig* config, mp_api::StructArray<mp_api::EdgeInfo>* value_out);
MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__OutputSidePacketInfos(mediapipe::ValidatedGraphConfig* config, mp_api::StructArray<mp_api::EdgeInfo>* value_out);

MP_CAPI(int) mp_ValidatedGraphConfig__OutputStreamIndex__PKc(mediapipe::ValidatedGraphConfig* config, const char* name);
MP_CAPI(int) mp_ValidatedGraphConfig__OutputSidePacketIndex__PKc(mediapipe::ValidatedGraphConfig* config, const char* name);
MP_CAPI(int) mp_ValidatedGraphConfig__OutputStreamToNode__PKc(mediapipe::ValidatedGraphConfig* config, const char* name);

MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__RegisteredSidePacketTypeName(mediapipe::ValidatedGraphConfig* config, const char* name,
                                                                            absl::Status** status_out, const char** string_out);
MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__RegisteredStreamTypeName(mediapipe::ValidatedGraphConfig* config, const char* name,
                                                                        absl::Status** status_out, const char** string_out);

MP_CAPI(MpReturnCode) mp_ValidatedGraphConfig__Package(mediapipe::ValidatedGraphConfig* config, const char** str_out);

MP_CAPI(bool) mp_ValidatedGraphConfig_IsReservedExecutorName(const char* name);
MP_CAPI(bool) mp_ValidatedGraphConfig__IsExternalSidePacket__PKc(mediapipe::ValidatedGraphConfig* config, const char* name);

MP_CAPI(void) mp_api_EdgeInfoArray__delete(mp_api::EdgeInfo* edge_info_vector_data, int size);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_VALIDATED_GRAPH_CONFIG_H_
