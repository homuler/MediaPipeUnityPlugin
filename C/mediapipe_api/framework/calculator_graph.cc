#include "mediapipe_api/framework/calculator_graph.h"

MpReturnCode mp_CalculatorGraph__(mediapipe::CalculatorGraph** graph_out) {
  TRY {
    *graph_out = new mediapipe::CalculatorGraph();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

void mp_CalculatorGraph__delete(mediapipe::CalculatorGraph* graph) {
  delete graph;
}

MpReturnCode mp_CalculatorGraph__Rconfig(mediapipe::CalculatorGraphConfig* config, mediapipe::CalculatorGraph** graph_out) {
  TRY_ALL {
    *graph_out = new mediapipe::CalculatorGraph(*config);
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__Initialize__Rconfig(mediapipe::CalculatorGraph* graph,
                                                     mediapipe::CalculatorGraphConfig* config,
                                                     mediapipe::Status** status_out) {
  TRY_ALL {
    *status_out = new mediapipe::Status { graph->Initialize(*config) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__Initialize__Rconfig_Rsp(
    mediapipe::CalculatorGraph* graph,
    mediapipe::CalculatorGraphConfig* config,
    SidePackets* side_packets,
    mediapipe::Status** status_out) {
  TRY_ALL {
    *status_out = new mediapipe::Status { graph->Initialize(*config, *side_packets) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__Config(mediapipe::CalculatorGraph* graph, mediapipe::CalculatorGraphConfig** config_out) {
  TRY_ALL {
    *config_out = new mediapipe::CalculatorGraphConfig { graph->Config() };  // Crashes if graph has no config
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__StartRun__Rsp(mediapipe::CalculatorGraph* graph,
                                               SidePackets* side_packets,
                                               mediapipe::Status** status_out) {
  TRY {
    auto status = graph->StartRun(*side_packets);
    *status_out = new mediapipe::Status { std::move(status) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__WaitUntilDone(mediapipe::CalculatorGraph* graph, mediapipe::Status** status_out) {
  TRY {
    auto status = graph->WaitUntilDone();
    *status_out = new mediapipe::Status { std::move(status) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

#ifndef MEDIAPIPE_DISABLE_GPU
MpGpuResources* MpCalculatorGraphGetGpuResources(MpCalculatorGraph* graph) {
  return new MpGpuResources { graph->impl->GetGpuResources() };
}

MpStatus* MpCalculatorGraphSetGpuResources(MpCalculatorGraph* graph, MpGpuResources* gpu_resources) {
  return new MpStatus { graph->impl->SetGpuResources(gpu_resources->impl) };
}
#endif  // !defined(MEDIAPIPE_DISABLE_GPU)


MpReturnCode mp_CalculatorGraph__AddOutputStreamPoller__PKc(mediapipe::CalculatorGraph* graph,
                                                            const char* name,
                                                            mediapipe::StatusOrPoller** status_or_poller_out) {
  TRY {
    *status_or_poller_out = new mediapipe::StatusOrPoller { graph->AddOutputStreamPoller(name) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(mediapipe::CalculatorGraph* graph,
                                                                     const char* name,
                                                                     mediapipe::Packet* packet,
                                                                     mediapipe::Status** status_out) {
  TRY_ALL {
    *status_out = new mediapipe::Status { graph->AddPacketToInputStream(name, std::move(*packet)) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

MpReturnCode mp_CalculatorGraph__CloseInputStream__PKc(mediapipe::CalculatorGraph* graph,
                                                       const char* name,
                                                       mediapipe::Status** status_out) {
  TRY_ALL {
    *status_out = new mediapipe::Status { graph->CloseInputStream(name) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}
