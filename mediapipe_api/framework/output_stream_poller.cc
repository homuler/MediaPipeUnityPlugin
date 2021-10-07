// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/output_stream_poller.h"

#include "mediapipe_api/external/absl/statusor.h"

void mp_OutputStreamPoller__delete(mediapipe::OutputStreamPoller* poller) { delete poller; }

MpReturnCode mp_OutputStreamPoller__Reset(mediapipe::OutputStreamPoller* poller) {
  TRY_ALL
    poller->Reset();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_OutputStreamPoller__Next_Ppacket(mediapipe::OutputStreamPoller* poller, mediapipe::Packet* packet, bool* result_out) {
  TRY_ALL
    *result_out = poller->Next(packet);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_OutputStreamPoller__SetMaxQueueSize(mediapipe::OutputStreamPoller* poller, int queue_size) {
  TRY_ALL
    poller->SetMaxQueueSize(queue_size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_OutputStreamPoller__QueueSize(mediapipe::OutputStreamPoller* poller, int* queue_size_out) {
  TRY_ALL
    *queue_size_out = poller->QueueSize();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_StatusOrPoller__delete(mediapipe::StatusOrPoller* status_or_poller) { delete status_or_poller; }

bool mp_StatusOrPoller__ok(mediapipe::StatusOrPoller* status_or_poller) { return absl_StatusOr__ok(status_or_poller); }

MpReturnCode mp_StatusOrPoller__status(mediapipe::StatusOrPoller* status_or_poller, absl::Status** status_out) {
  return absl_StatusOr__status(status_or_poller, status_out);
}

MpReturnCode mp_StatusOrPoller__value(mediapipe::StatusOrPoller* poller, mediapipe::OutputStreamPoller** poller_out) {
  return absl_StatusOr__value(poller, poller_out);
}
