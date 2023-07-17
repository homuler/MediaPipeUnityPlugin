// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/calculator_graph.h"

#include <utility>

MpReturnCode mp_CalculatorGraph__(mediapipe::CalculatorGraph** graph_out) {
  TRY
    *graph_out = new mediapipe::CalculatorGraph();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_CalculatorGraph__delete(mediapipe::CalculatorGraph* graph) { delete graph; }

MpReturnCode mp_CalculatorGraph__PKc_i(const char* serialized_config, int size, mediapipe::CalculatorGraph** graph_out) {
  TRY_ALL
    auto config = ParseFromStringAsProto<mediapipe::CalculatorGraphConfig>(serialized_config, size);
    *graph_out = new mediapipe::CalculatorGraph(config);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__Initialize__PKc_i(mediapipe::CalculatorGraph* graph, const char* serialized_config, int size, absl::Status** status_out) {
  TRY_ALL
    auto config = ParseFromStringAsProto<mediapipe::CalculatorGraphConfig>(serialized_config, size);
    *status_out = new absl::Status{graph->Initialize(config)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__Initialize__PKc_i_Rsp(mediapipe::CalculatorGraph* graph, const char* serialized_config, int size, SidePackets* side_packets,
                                                       absl::Status** status_out) {
  TRY_ALL
    auto config = ParseFromStringAsProto<mediapipe::CalculatorGraphConfig>(serialized_config, size);
    *status_out = new absl::Status{graph->Initialize(config, *side_packets)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__Config(mediapipe::CalculatorGraph* graph, mp_api::SerializedProto* config_out) {
  TRY_ALL
    SerializeProto(graph->Config(), config_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__ObserveOutputStream__PKc_PF_b(mediapipe::CalculatorGraph* graph, const char* stream_name, int stream_id,
                                                               NativePacketCallback* packet_callback, bool observe_timestamp_bounds,
                                                               absl::Status** status_out) {
  TRY_ALL
    auto status = graph->ObserveOutputStream(
        stream_name,
        [graph, stream_id, packet_callback](const mediapipe::Packet& packet) -> ::absl::Status {
          auto status_args = packet_callback(graph, stream_id, packet);
          auto callback_status = absl::Status{status_args.code, absl::NullSafeStringView((const char*)status_args.message)};
          if (status_args.message != nullptr) {
            mp_api::freeHGlobal(status_args.message);
          }
          return std::move(callback_status);
        },
        observe_timestamp_bounds);
    *status_out = new absl::Status{std::move(status)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__AddOutputStreamPoller__PKc_b(mediapipe::CalculatorGraph* graph, const char* stream_name,
                                                              bool observe_timestamp_bounds,
                                                              absl::Status** status_out, mediapipe::OutputStreamPoller** poller_out) {
  TRY
    auto status_or_poller = graph->AddOutputStreamPoller(stream_name, observe_timestamp_bounds);
    *status_out = new absl::Status{status_or_poller.status()};
    if (status_or_poller.ok()) {
      *poller_out = new mediapipe::OutputStreamPoller{std::move(status_or_poller).value()};
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__Run__Rsp(mediapipe::CalculatorGraph* graph, SidePackets* side_packets, absl::Status** status_out) {
  TRY
    auto status = graph->Run(*side_packets);
    *status_out = new absl::Status{std::move(status)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__StartRun__Rsp(mediapipe::CalculatorGraph* graph, SidePackets* side_packets, absl::Status** status_out) {
  TRY
    auto status = graph->StartRun(*side_packets);
    *status_out = new absl::Status{std::move(status)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__WaitUntilIdle(mediapipe::CalculatorGraph* graph, absl::Status** status_out) {
  TRY
    auto status = graph->WaitUntilIdle();
    *status_out = new absl::Status{std::move(status)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__WaitUntilDone(mediapipe::CalculatorGraph* graph, absl::Status** status_out) {
  TRY
    auto status = graph->WaitUntilDone();
    *status_out = new absl::Status{std::move(status)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool mp_CalculatorGraph__HasError(mediapipe::CalculatorGraph* graph) { return graph->HasError(); }

MpReturnCode mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(mediapipe::CalculatorGraph* graph, const char* stream_name, mediapipe::Packet* packet,
                                                                     absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{graph->AddPacketToInputStream(stream_name, std::move(*packet))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__SetInputStreamMaxQueueSize__PKc_i(mediapipe::CalculatorGraph* graph, const char* stream_name, int max_queue_size,
                                                                   absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{graph->SetInputStreamMaxQueueSize(stream_name, max_queue_size)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool mp_CalculatorGraph__HasInputStream__PKc(mediapipe::CalculatorGraph* graph, const char* name) { return graph->HasInputStream(name); }

MpReturnCode mp_CalculatorGraph__CloseInputStream__PKc(mediapipe::CalculatorGraph* graph, const char* stream_name, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{graph->CloseInputStream(stream_name)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__CloseAllPacketSources(mediapipe::CalculatorGraph* graph, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{graph->CloseAllPacketSources()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__Cancel(mediapipe::CalculatorGraph* graph) {
  TRY
    graph->Cancel();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool mp_CalculatorGraph__GraphInputStreamsClosed(mediapipe::CalculatorGraph* graph) { return graph->GraphInputStreamsClosed(); }

bool mp_CalculatorGraph__IsNodeThrottled__i(mediapipe::CalculatorGraph* graph, int node_id) { return graph->IsNodeThrottled(node_id); }

bool mp_CalculatorGraph__UnthrottleSources(mediapipe::CalculatorGraph* graph) { return graph->UnthrottleSources(); }

#ifndef MEDIAPIPE_DISABLE_GPU
MpReturnCode mp_CalculatorGraph__GetGpuResources(mediapipe::CalculatorGraph* graph, std::shared_ptr<mediapipe::GpuResources>** gpu_resources_out) {
  TRY
    auto gpu_resources = graph->GetGpuResources();
    *gpu_resources_out = new std::shared_ptr<mediapipe::GpuResources>{gpu_resources};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__SetGpuResources__SPgpu(mediapipe::CalculatorGraph* graph, std::shared_ptr<mediapipe::GpuResources>* gpu_resources,
                                                        absl::Status** status_out) {
  TRY_ALL
    *status_out = new absl::Status{graph->SetGpuResources(*gpu_resources)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}
#endif  // !defined(MEDIAPIPE_DISABLE_GPU)
