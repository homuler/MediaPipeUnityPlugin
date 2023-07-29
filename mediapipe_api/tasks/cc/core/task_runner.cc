#include "mediapipe_api/tasks/cc/core/task_runner.h"

#include "mediapipe/tasks/cc/core/mediapipe_builtin_op_resolver.h"

MpReturnCode mp_tasks_core_TaskRunner_Create__PKc_i_PF(const char* serialized_config, int size,
                                                       int callback_id, NativePacketsCallback* packets_callback,
                                                       absl::Status** status_out, TaskRunner** task_runner_out) {
  TRY
    auto config = ParseFromStringAsProto<mediapipe::CalculatorGraphConfig>(serialized_config, size);
    mediapipe::tasks::core::PacketsCallback callback = nullptr;
    if (packets_callback) {
      callback = [callback_id, packets_callback](absl::StatusOr<PacketMap> status_or_packet_map) -> void {
        auto status = status_or_packet_map.status();
        if (!status.ok()) {
          packets_callback(callback_id, &status, nullptr);
          return;
        }
        auto value = status_or_packet_map.value();
        packets_callback(callback_id, &status, &value);
      };
    }

    auto status_or_task_runner = TaskRunner::Create(
      std::move(config),
      absl::make_unique<mediapipe::tasks::core::MediaPipeBuiltinOpResolver>(),
      std::move(callback));

    *status_out = new absl::Status{status_or_task_runner.status()};
    if (status_or_task_runner.ok()) {
      // NOTE: TaskRunner cannot be moved, so pass the pointer instead.
      *task_runner_out = status_or_task_runner.value().release();
    } else {
      *task_runner_out = nullptr;
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_tasks_core_TaskRunner__delete(TaskRunner* task_runner) {
  delete task_runner;
}

MpReturnCode mp_tasks_core_TaskRunner__Process__Ppm(TaskRunner* task_runner, PacketMap* inputs, absl::Status** status_out, PacketMap** value_out) {
  TRY
    auto status_or_packet_map = task_runner->Process(std::move(*inputs));
    *status_out = new absl::Status{status_or_packet_map.status()};
    if (status_or_packet_map.ok()) {
      *value_out = new PacketMap{status_or_packet_map.value()};
    } else {
      *value_out = nullptr;
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_tasks_core_TaskRunner__Send__Ppm(TaskRunner* task_runner, PacketMap* inputs, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{task_runner->Send(std::move(*inputs))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_tasks_core_TaskRunner__Close(TaskRunner* task_runner, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{task_runner->Close()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_tasks_core_TaskRunner__Restart(TaskRunner* task_runner, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{task_runner->Restart()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_tasks_core_TaskRunner__GetGraphConfig(TaskRunner* task_runner, mp_api::SerializedProto* value_out) {
  TRY
    SerializeProto(task_runner->GetGraphConfig(), value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
