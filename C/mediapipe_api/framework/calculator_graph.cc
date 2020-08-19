#include <string>
#include <utility>
#include "mediapipe/framework/calculator.pb.h"
#include "mediapipe/framework/port/parse_text_proto.h"
#include "mediapipe_api/framework/calculator_graph.h"

MpCalculatorGraphConfig* ParseMpCalculatorGraphConfig(const char* input) {
  auto config = new mediapipe::CalculatorGraphConfig {};
  auto result = google::protobuf::TextFormat::ParseFromString(input, config);

  if (!result) return nullptr;

  return new MpCalculatorGraphConfig { std::unique_ptr<mediapipe::CalculatorGraphConfig> { std::move(config) } };
}

void MpCalculatorGraphConfigDestroy(MpCalculatorGraphConfig* config) {
  delete config;
}

MpCalculatorGraph* MpCalculatorGraphCreate() {
  return new MpCalculatorGraph();
}

void MpCalculatorGraphDestroy(MpCalculatorGraph* graph) {
  delete graph;
}

MpStatus* MpCalculatorGraphInitialize(MpCalculatorGraph* graph, MpCalculatorGraphConfig* config) {
  auto status = graph->impl->Initialize(*config->impl);

  return new MpStatus { std::move(status) };
}

MpStatus* MpCalculatorGraphStartRun(MpCalculatorGraph* graph, MpSidePacket* mp_side_packet) {
  auto side_packet = mp_side_packet->impl;
  auto status = graph->impl->StartRun(*side_packet);

  return new MpStatus { std::move(status) };
}

MpStatus* MpCalculatorGraphWaitUntilDone(MpCalculatorGraph* graph) {
  auto status = graph->impl->WaitUntilDone();

  return new MpStatus { std::move(status) };
}

#ifndef MEDIAPIPE_DISABLE_GPU
MpGpuResources* MpCalculatorGraphGetGpuResources(MpCalculatorGraph* graph) {
  return new MpGpuResources { graph->impl->GetGpuResources() };
}

MpStatus* MpCalculatorGraphSetGpuResources(MpCalculatorGraph* graph, MpGpuResources* gpu_resources) {
  return new MpStatus { graph->impl->SetGpuResources(gpu_resources->impl) };
}
#endif  // !defined(MEDIAPIPE_DISABLE_GPU)

MpStatusOrPoller* MpCalculatorGraphAddOutputStreamPoller(MpCalculatorGraph* graph, const char* name) {
  auto status_or_value = graph->impl->AddOutputStreamPoller(std::string(name));

  return new MpStatusOrPoller {
    status_or_value.status(),
    std::make_unique<mediapipe::OutputStreamPoller>(status_or_value.ConsumeValueOrDie())
  };
}

bool MpOutputStreamPollerNext(mediapipe::OutputStreamPoller* poller, MpPacket* packet) {
  return poller->Next(packet->impl.get());
}

MpStatus* MpCalculatorGraphAddPacketToInputStream(MpCalculatorGraph* graph, const char* name, MpPacket* packet) {
  auto status = graph->impl->AddPacketToInputStream(std::string(name), *packet->impl);

  return new MpStatus { std::move(status) };
}

MpStatus* MpCalculatorGraphCloseInputStream(MpCalculatorGraph* graph, const char* name) {
  auto status = graph->impl->CloseInputStream(std::string(name));

  return new MpStatus { std::move(status) };
}

MpStatus* MpStatusOrPollerStatus(MpStatusOrPoller* status_or_poller) {
  mediapipe::Status status { *status_or_poller->status };

  return new MpStatus { std::move(status) };
}

mediapipe::OutputStreamPoller* MpStatusOrPollerConsumeValue(MpStatusOrPoller* status_or_poller) {
  return status_or_poller->value.release();
}

void MpStatusOrPollerDestroy(MpStatusOrPoller* status_or_poller) {
  delete status_or_poller;
}

void MpOutputStreamPollerDestroy(mediapipe::OutputStreamPoller* poller) {
  delete poller;
}
