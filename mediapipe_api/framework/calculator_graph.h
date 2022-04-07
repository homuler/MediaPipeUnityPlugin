// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_
#define MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_

#include <map>
#include <memory>
#include <string>

#include "mediapipe/framework/calculator_graph.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/absl/status.h"
#include "mediapipe_api/external/absl/statusor.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

#ifndef MEDIAPIPE_DISABLE_GPU
#include "mediapipe/gpu/gl_calculator_helper.h"
#include "mediapipe/gpu/gpu_shared_data_internal.h"
#endif  // !defined(MEDIAPIPE_DISABLE_GPU)

extern "C" {

typedef std::map<std::string, mediapipe::Packet> SidePackets;
typedef absl::Status* NativePacketCallback(mediapipe::CalculatorGraph* graph, int stream_id, const mediapipe::Packet&);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__(mediapipe::CalculatorGraph** graph_out);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__PKc(const char* text_format_config, mediapipe::CalculatorGraph** graph_out);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__PKc_i(const char* serialized_config, int size, mediapipe::CalculatorGraph** graph_out);
MP_CAPI(void) mp_CalculatorGraph__delete(mediapipe::CalculatorGraph* graph);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__Initialize__PKc_i(mediapipe::CalculatorGraph* graph, const char* serialized_config, int size,
                                                            absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__Initialize__PKc_i_Rsp(mediapipe::CalculatorGraph* graph, const char* serialized_config, int size,
                                                                SidePackets* side_packets, absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__Config(mediapipe::CalculatorGraph* graph, mp_api::SerializedProto* config_out);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__ObserveOutputStream__PKc_PF_b(mediapipe::CalculatorGraph* graph, const char* stream_name, int stream_id,
                                                                        NativePacketCallback* packet_callback, bool observe_timestamp_bounds,
                                                                        absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__AddOutputStreamPoller__PKc_b(mediapipe::CalculatorGraph* graph, const char* stream_name,
                                                                       bool observe_timestamp_bounds, mediapipe::StatusOrPoller** status_or_poller_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__Run__Rsp(mediapipe::CalculatorGraph* graph, SidePackets* side_packets, absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__StartRun__Rsp(mediapipe::CalculatorGraph* graph, SidePackets* side_packets, absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__WaitUntilIdle(mediapipe::CalculatorGraph* graph, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__WaitUntilDone(mediapipe::CalculatorGraph* graph, absl::Status** status_out);
MP_CAPI(bool) mp_CalculatorGraph__HasError(mediapipe::CalculatorGraph* graph);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(mediapipe::CalculatorGraph* graph, const char* stream_name,
                                                                              mediapipe::Packet* packet, absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__SetInputStreamMaxQueueSize__PKc_i(mediapipe::CalculatorGraph* graph, const char* stream_name, int max_queue_size,
                                                                            absl::Status** status_out);

MP_CAPI(bool) mp_CalculatorGraph__HasInputStream__PKc(mediapipe::CalculatorGraph* graph, const char* name);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__CloseInputStream__PKc(mediapipe::CalculatorGraph* graph, const char* stream_name, absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__CloseAllPacketSources(mediapipe::CalculatorGraph* graph, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__Cancel(mediapipe::CalculatorGraph* graph);
MP_CAPI(bool) mp_CalculatorGraph__GraphInputStreamsClosed(mediapipe::CalculatorGraph* graph);
MP_CAPI(bool) mp_CalculatorGraph__IsNodeThrottled__i(mediapipe::CalculatorGraph* graph, int node_id);
MP_CAPI(bool) mp_CalculatorGraph__UnthrottleSources(mediapipe::CalculatorGraph* graph);

#ifndef MEDIAPIPE_DISABLE_GPU
MP_CAPI(MpReturnCode) mp_CalculatorGraph__GetGpuResources(mediapipe::CalculatorGraph* graph, std::shared_ptr<mediapipe::GpuResources>** gpu_resources_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__SetGpuResources__SPgpu(mediapipe::CalculatorGraph* graph, std::shared_ptr<mediapipe::GpuResources>* gpu_resources,
                                                                 absl::Status** status_out);
#endif  // !defined(MEDIAPIPE_DISABLE_GPU)

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_
