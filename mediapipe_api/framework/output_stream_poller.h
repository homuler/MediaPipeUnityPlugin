// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_OUTPUT_STREAM_POLLER_H_
#define MEDIAPIPE_API_FRAMEWORK_OUTPUT_STREAM_POLLER_H_

#include "mediapipe/framework/calculator_graph.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(void) mp_OutputStreamPoller__delete(mediapipe::OutputStreamPoller* poller);
MP_CAPI(MpReturnCode) mp_OutputStreamPoller__Reset(mediapipe::OutputStreamPoller* poller);
MP_CAPI(MpReturnCode) mp_OutputStreamPoller__Next_Ppacket(mediapipe::OutputStreamPoller* poller, mediapipe::Packet* packet, bool* result_out);
MP_CAPI(MpReturnCode) mp_OutputStreamPoller__SetMaxQueueSize(mediapipe::OutputStreamPoller* poller, int queue_size);
MP_CAPI(MpReturnCode) mp_OutputStreamPoller__QueueSize(mediapipe::OutputStreamPoller* poller, int* queue_size_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_OUTPUT_STREAM_POLLER_H_
