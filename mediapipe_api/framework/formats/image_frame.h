// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_FORMATS_IMAGE_FRAME_H_
#define MEDIAPIPE_API_FRAMEWORK_FORMATS_IMAGE_FRAME_H_

#include <memory>
#include <utility>

#include "mediapipe/framework/formats/image_frame.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/absl/status.h"
#include "mediapipe_api/external/absl/statusor.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

typedef absl::StatusOr<mediapipe::ImageFrame> StatusOrImageFrame;
typedef void(Deleter)(uint8*);

MP_CAPI(MpReturnCode) mp_ImageFrame__(mediapipe::ImageFrame** image_frame_out);
MP_CAPI(MpReturnCode) mp_ImageFrame__ui_i_i_ui(mediapipe::ImageFormat::Format format, int width, int height, uint32 alignment_boundary,
                                               mediapipe::ImageFrame** image_frame_out);
MP_CAPI(MpReturnCode) mp_ImageFrame__ui_i_i_i_Pui8_PF(mediapipe::ImageFormat::Format format, int width, int height, int width_step, uint8* pixel_data,
                                                      Deleter* deleter, mediapipe::ImageFrame** image_frame_out);
MP_CAPI(void) mp_ImageFrame__delete(mediapipe::ImageFrame* image_frame);
MP_CAPI(bool) mp_ImageFrame__IsEmpty(mediapipe::ImageFrame* image_frame);
MP_CAPI(MpReturnCode) mp_ImageFrame__SetToZero(mediapipe::ImageFrame* image_frame);
MP_CAPI(MpReturnCode) mp_ImageFrame__SetAlignmentPaddingAreas(mediapipe::ImageFrame* image_frame);
MP_CAPI(bool) mp_ImageFrame__IsContiguous(mediapipe::ImageFrame* image_frame);
MP_CAPI(MpReturnCode) mp_ImageFrame__IsAligned__ui(mediapipe::ImageFrame* image_frame, uint32 alignment_boundary, bool* value_out);
MP_CAPI(mediapipe::ImageFormat::Format) mp_ImageFrame__Format(mediapipe::ImageFrame* image_frame);
MP_CAPI(int) mp_ImageFrame__Width(mediapipe::ImageFrame* image_frame);
MP_CAPI(int) mp_ImageFrame__Height(mediapipe::ImageFrame* image_frame);
MP_CAPI(MpReturnCode) mp_ImageFrame__ChannelSize(mediapipe::ImageFrame* image_frame, int* value_out);
MP_CAPI(MpReturnCode) mp_ImageFrame__NumberOfChannels(mediapipe::ImageFrame* image_frame, int* value_out);
MP_CAPI(MpReturnCode) mp_ImageFrame__ByteDepth(mediapipe::ImageFrame* image_frame, int* value_out);
MP_CAPI(int) mp_ImageFrame__WidthStep(mediapipe::ImageFrame* image_frame);
MP_CAPI(uint8*) mp_ImageFrame__MutablePixelData(mediapipe::ImageFrame* image_frame);
MP_CAPI(int) mp_ImageFrame__PixelDataSize(mediapipe::ImageFrame* image_frame);
MP_CAPI(MpReturnCode) mp_ImageFrame__PixelDataSizeStoredContiguously(mediapipe::ImageFrame* image_frame, int* value_out);
MP_CAPI(MpReturnCode) mp_ImageFrame__CopyToBuffer__Pui8_i(mediapipe::ImageFrame* image_frame, uint8* buffer, int buffer_size);
MP_CAPI(MpReturnCode) mp_ImageFrame__CopyToBuffer__Pui16_i(mediapipe::ImageFrame* image_frame, uint16* buffer, int buffer_size);
MP_CAPI(MpReturnCode) mp_ImageFrame__CopyToBuffer__Pf_i(mediapipe::ImageFrame* image_frame, float* buffer, int buffer_size);

// StatusOr API
MP_CAPI(void) mp_StatusOrImageFrame__delete(StatusOrImageFrame* status_or_image_frame);
MP_CAPI(bool) mp_StatusOrImageFrame__ok(StatusOrImageFrame* status_or_image_frame);
MP_CAPI(MpReturnCode) mp_StatusOrImageFrame__status(StatusOrImageFrame* status_or_image_frame, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_StatusOrImageFrame__value(StatusOrImageFrame* status_or_image_frame, mediapipe::ImageFrame** value_out);

// Packet API
MP_CAPI(MpReturnCode) mp__MakeImageFramePacket__Pif(mediapipe::ImageFrame* image_frame, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeImageFramePacket_At__Pif_Rt(mediapipe::ImageFrame* image_frame, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__ConsumeImageFrame(mediapipe::Packet* packet, StatusOrImageFrame** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetImageFrame(mediapipe::Packet* packet, const mediapipe::ImageFrame** value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsImageFrame(mediapipe::Packet* packet, absl::Status** status_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_FORMATS_IMAGE_FRAME_H_
