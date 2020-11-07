#ifndef C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_
#define C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_

#include <map>
#include <memory>
#include <string>
#include <utility>
#include "mediapipe/framework/calculator_graph.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"
#include "mediapipe_api/framework/port/status.h"
#include "mediapipe_api/framework/port/statusor.h"

#ifndef MEDIAPIPE_DISABLE_GPU
#include "mediapipe_api/gpu/gl_calculator_helper.h"
#include "mediapipe_api/gpu/gpu_shared_data_internal.h"
#endif  // !defined(MEDIAPIPE_DISABLE_GPU)

extern "C" {

typedef struct MpCalculatorGraph {
  std::unique_ptr<mediapipe::CalculatorGraph> impl;

  MpCalculatorGraph() : impl { std::make_unique<mediapipe::CalculatorGraph>() } {}
} MpCalculatorGraph;

typedef std::map<std::string, mediapipe::Packet> SidePackets;

MP_CAPI(MpReturnCode) mp_CalculatorGraph__(mediapipe::CalculatorGraph** graph_out);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__Rconfig(mediapipe::CalculatorGraphConfig* config, mediapipe::CalculatorGraph** graph_out);
MP_CAPI(void) mp_CalculatorGraph__delete(mediapipe::CalculatorGraph* graph);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__Initialize__Rconfig(mediapipe::CalculatorGraph* graph,
                                                              mediapipe::CalculatorGraphConfig* config,
                                                              mediapipe::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__Initialize__Rconfig_Rsp(
    mediapipe::CalculatorGraph* graph,
    mediapipe::CalculatorGraphConfig* config,
    SidePackets* side_packets,
    mediapipe::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__Config(mediapipe::CalculatorGraph* graph, mediapipe::CalculatorGraphConfig** config_out);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__StartRun__Rsp(mediapipe::CalculatorGraph* graph,
                                                        SidePackets* side_packets,
                                                        mediapipe::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__WaitUntilDone(mediapipe::CalculatorGraph* graph, mediapipe::Status** status_out);

MP_CAPI(MpReturnCode) mp_CalculatorGraph__AddOutputStreamPoller__PKc(mediapipe::CalculatorGraph* graph,
                                                                     const char* name,
                                                                     mediapipe::StatusOrPoller** status_or_poller_out);

#ifndef MEDIAPIPE_DISABLE_GPU
MP_CAPI_EXPORT extern MpGpuResources* MpCalculatorGraphGetGpuResources(MpCalculatorGraph* graph);
MP_CAPI_EXPORT extern MpStatus* MpCalculatorGraphSetGpuResources(MpCalculatorGraph* graph, MpGpuResources* gpu_resources);
#endif  // !defined(MEDIAPIPE_DISABLE_GPU)

MP_CAPI(MpReturnCode) mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(mediapipe::CalculatorGraph* graph,
                                                                              const char* name,
                                                                              mediapipe::Packet* packet,
                                                                              mediapipe::Status** status_out);
MP_CAPI(MpReturnCode) mp_CalculatorGraph__CloseInputStream__PKc(mediapipe::CalculatorGraph* graph,
                                                                const char* name,
                                                                mediapipe::Status** status_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_
