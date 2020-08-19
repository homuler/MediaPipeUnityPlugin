#ifndef C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_
#define C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_

#include <memory>
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

typedef struct MpCalculatorGraphConfig {
  std::unique_ptr<mediapipe::CalculatorGraphConfig> impl;
} MpCalculatorGraphConfig;

typedef struct MpCalculatorGraph {
  std::unique_ptr<mediapipe::CalculatorGraph> impl;

  MpCalculatorGraph() : impl { std::make_unique<mediapipe::CalculatorGraph>() } {}
} MpCalculatorGraph;

typedef MpStatusOrValue<std::unique_ptr<mediapipe::OutputStreamPoller>> MpStatusOrPoller;

/** mediapipe::CalculatorGraph API */
MP_CAPI_EXPORT extern MpCalculatorGraphConfig* ParseMpCalculatorGraphConfig(const char* input);
MP_CAPI_EXPORT extern void MpCalculatorGraphConfigDestroy(MpCalculatorGraphConfig* config);

MP_CAPI_EXPORT extern MpCalculatorGraph* MpCalculatorGraphCreate();
MP_CAPI_EXPORT extern void MpCalculatorGraphDestroy(MpCalculatorGraph* graph);
MP_CAPI_EXPORT extern MpStatus* MpCalculatorGraphInitialize(MpCalculatorGraph* graph, MpCalculatorGraphConfig* input_config);
MP_CAPI_EXPORT extern MpStatus* MpCalculatorGraphStartRun(MpCalculatorGraph* graph, MpSidePacket* side_packet);
MP_CAPI_EXPORT extern MpStatus* MpCalculatorGraphWaitUntilDone(MpCalculatorGraph* graph);

#ifndef MEDIAPIPE_DISABLE_GPU
MP_CAPI_EXPORT extern MpGpuResources* MpCalculatorGraphGetGpuResources(MpCalculatorGraph* graph);
MP_CAPI_EXPORT extern MpStatus* MpCalculatorGraphSetGpuResources(MpCalculatorGraph* graph, MpGpuResources* gpu_resources);
#endif  // !defined(MEDIAPIPE_DISABLE_GPU)

/** mediapipe::OutputStreamPoller API */
MP_CAPI_EXPORT extern MpStatusOrPoller* MpCalculatorGraphAddOutputStreamPoller(MpCalculatorGraph* graph, const char* name);
MP_CAPI_EXPORT extern bool MpOutputStreamPollerNext(mediapipe::OutputStreamPoller* poller, MpPacket* packet);

/** mediapipe::InputStream API */
MP_CAPI_EXPORT extern MpStatus* MpCalculatorGraphAddPacketToInputStream(MpCalculatorGraph* graph, const char* name, MpPacket* packet);
MP_CAPI_EXPORT extern MpStatus* MpCalculatorGraphCloseInputStream(MpCalculatorGraph* graph, const char* name);

/** mediapipe::StatusOrPoller API */
MP_CAPI_EXPORT extern void MpStatusOrPollerDestroy(MpStatusOrPoller* status_or_poller);
MP_CAPI_EXPORT extern MpStatus* MpStatusOrPollerStatus(MpStatusOrPoller* status_or_poller);
MP_CAPI_EXPORT extern mediapipe::OutputStreamPoller* MpStatusOrPollerConsumeValue(MpStatusOrPoller* status_or_poller);
MP_CAPI_EXPORT extern void MpOutputStreamPollerDestroy(mediapipe::OutputStreamPoller* poller);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_CALCULATOR_GRAPH_H_
