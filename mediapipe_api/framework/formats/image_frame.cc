// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/image_frame.h"

MpReturnCode mp_ImageFrame__(mediapipe::ImageFrame** image_frame_out) {
  TRY
    *image_frame_out = new mediapipe::ImageFrame();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ImageFrame__ui_i_i_ui(mediapipe::ImageFormat::Format format, int width, int height, uint32 alignment_boundary,
                                      mediapipe::ImageFrame** image_frame_out) {
  TRY_ALL
    *image_frame_out = new mediapipe::ImageFrame{format, width, height, alignment_boundary};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_ImageFrame__ui_i_i_i_Pui8_PF(mediapipe::ImageFormat::Format format, int width, int height, int width_step, uint8* pixel_data, Deleter* deleter,
                                             mediapipe::ImageFrame** image_frame_out) {
  TRY_ALL
    *image_frame_out = new mediapipe::ImageFrame{format, width, height, width_step, pixel_data, deleter};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_ImageFrame__delete(mediapipe::ImageFrame* image_frame) { delete image_frame; }

bool mp_ImageFrame__IsEmpty(mediapipe::ImageFrame* image_frame) { return image_frame->IsEmpty(); }

MpReturnCode mp_ImageFrame__SetToZero(mediapipe::ImageFrame* image_frame) {
  TRY
    image_frame->SetToZero();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_ImageFrame__SetAlignmentPaddingAreas(mediapipe::ImageFrame* image_frame) {
  TRY
    image_frame->SetAlignmentPaddingAreas();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool mp_ImageFrame__IsContiguous(mediapipe::ImageFrame* image_frame) { return image_frame->IsContiguous(); }

MpReturnCode mp_ImageFrame__IsAligned__ui(mediapipe::ImageFrame* image_frame, uint32 alignment_boundary, bool* value_out) {
  TRY_ALL
    *value_out = image_frame->IsAligned(alignment_boundary);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

mediapipe::ImageFormat::Format mp_ImageFrame__Format(mediapipe::ImageFrame* image_frame) { return image_frame->Format(); }

int mp_ImageFrame__Width(mediapipe::ImageFrame* image_frame) { return image_frame->Width(); }

int mp_ImageFrame__Height(mediapipe::ImageFrame* image_frame) { return image_frame->Height(); }

int mp_ImageFrame__WidthStep(mediapipe::ImageFrame* image_frame) { return image_frame->WidthStep(); }

uint8* mp_ImageFrame__MutablePixelData(mediapipe::ImageFrame* image_frame) { return image_frame->MutablePixelData(); }

MpReturnCode mp_ImageFrame__CopyToBuffer__Pui8_i(mediapipe::ImageFrame* image_frame, uint8* buffer, int buffer_size) {
  TRY_ALL
    image_frame->CopyToBuffer(buffer, buffer_size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_ImageFrame__CopyToBuffer__Pui16_i(mediapipe::ImageFrame* image_frame, uint16* buffer, int buffer_size) {
  TRY_ALL
    image_frame->CopyToBuffer(buffer, buffer_size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_ImageFrame__CopyToBuffer__Pf_i(mediapipe::ImageFrame* image_frame, float* buffer, int buffer_size) {
  TRY_ALL
    image_frame->CopyToBuffer(buffer, buffer_size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

// Packet API
MpReturnCode mp__MakeImageFramePacket__Pif(mediapipe::ImageFrame* image_frame, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::ImageFrame>(std::move(*image_frame))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeImageFramePacket_At__Pif_Rt(mediapipe::ImageFrame* image_frame, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::ImageFrame>(std::move(*image_frame)).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeImageFramePacket_At__Pif_ll(mediapipe::ImageFrame* image_frame, int64 timestampMicrosec, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::ImageFrame>(std::move(*image_frame)).At(mediapipe::Timestamp(timestampMicrosec))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__ConsumeImageFrame(mediapipe::Packet* packet, absl::Status** status_out, mediapipe::ImageFrame** value_out) {
  return mp_Packet__Consume(packet, status_out, value_out);
}

MpReturnCode mp_Packet__GetImageFrame(mediapipe::Packet* packet, const mediapipe::ImageFrame** value_out) { return mp_Packet__Get(packet, value_out); }

MpReturnCode mp_Packet__ValidateAsImageFrame(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<mediapipe::ImageFrame>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
