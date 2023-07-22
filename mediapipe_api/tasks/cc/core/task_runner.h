// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_TASKS_CC_CORE_TASK_RUNNER_H_
#define MEDIAPIPE_API_TASKS_CC_CORE_TASK_RUNNER_H_

#include <map>

#include "absl/status/statusor.h"
#include "mediapipe/tasks/cc/core/task_runner.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"

using TaskRunner = mediapipe::tasks::core::TaskRunner;

extern "C" {

typedef std::map<std::string, mediapipe::Packet> PacketMap;
typedef void NativePacketsCallback(int, absl::Status*, PacketMap*);

MP_CAPI(MpReturnCode) mp_tasks_core_TaskRunner_Create__PKc_i_PF(const char* serialized_config, int size,
                                                                int callback_id, NativePacketsCallback* packets_callback,
                                                                absl::Status** status_out, TaskRunner** task_runner_out);
MP_CAPI(void) mp_tasks_core_TaskRunner__delete(TaskRunner* task_runner);

MP_CAPI(MpReturnCode) mp_tasks_core_TaskRunner__Process__Ppm(TaskRunner* task_runner, PacketMap* inputs, absl::Status** status_out, PacketMap** value_out);
MP_CAPI(MpReturnCode) mp_tasks_core_TaskRunner__Send__Ppm(TaskRunner* task_runner, PacketMap* inputs, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_tasks_core_TaskRunner__Close(TaskRunner* task_runner, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_tasks_core_TaskRunner__Restart(TaskRunner* task_runner, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_tasks_core_TaskRunner__GetGraphConfig(TaskRunner* task_runner, mp_api::SerializedProto* value_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_TASKS_CC_CORE_TASK_RUNNER_H_
