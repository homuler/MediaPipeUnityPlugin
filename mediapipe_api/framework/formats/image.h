// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_FORMATS_IMAGE_H_
#define MEDIAPIPE_API_FRAMEWORK_FORMATS_IMAGE_H_

#include <memory>
#include <utility>

#include "absl/status/status.h"
#include "mediapipe/framework/formats/image.h"
#include "mediapipe_api/framework/packet.h"
#include "mediapipe_api/common.h"

#if !MEDIAPIPE_DISABLE_GPU

#if !MEDIAPIPE_GPU_BUFFER_USE_CV_PIXEL_BUFFER  // OSX, use GL textures.
#include "mediapipe/gpu/gl_texture_buffer.h"
#endif  // MEDIAPIPE_GPU_BUFFER_USE_CV_PIXEL_BUFFER

#endif  // !MEDIAPIPE_DISABLE_GPU

extern "C" {

typedef void(Deleter)(uint8*);

MP_CAPI(MpReturnCode) mp_Image__ui_i_i_i_Pui8_PF(mediapipe::ImageFormat::Format format, int width, int height, int width_step, uint8* pixel_data,
                                                 Deleter* deleter, mediapipe::Image** image_out);

#if !MEDIAPIPE_DISABLE_GPU

#if !MEDIAPIPE_GPU_BUFFER_USE_CV_PIXEL_BUFFER
typedef void GlTextureBufferDeletionCallback(GLuint name, std::shared_ptr<mediapipe::GlSyncPoint>* sync_token);

MP_CAPI(MpReturnCode) mp_Image__ui_ui_i_i_ui_PF_PSgc(GLenum target, GLuint name, int width, int height, mediapipe::GpuBufferFormat format,
                                                     GlTextureBufferDeletionCallback* deletion_callback,
                                                     std::shared_ptr<mediapipe::GlContext>* producer_context,
                                                     mediapipe::Image** image_out);
#endif

#endif

MP_CAPI(void) mp_Image__delete(mediapipe::Image* image);
MP_CAPI(int) mp_Image__width(mediapipe::Image* image);
MP_CAPI(int) mp_Image__height(mediapipe::Image* image);
MP_CAPI(int) mp_Image__channels(mediapipe::Image* image);
MP_CAPI(int) mp_Image__step(mediapipe::Image* image);
MP_CAPI(bool) mp_Image__UsesGpu(mediapipe::Image* image);
MP_CAPI(mediapipe::ImageFormat::Format) mp_Image__image_format(mediapipe::Image* image);
MP_CAPI(mediapipe::GpuBufferFormat) mp_Image__format(mediapipe::Image* image);
MP_CAPI(MpReturnCode) mp_Image__ConvertToCpu(mediapipe::Image* image, bool* result_out);
MP_CAPI(MpReturnCode) mp_Image__ConvertToGpu(mediapipe::Image* image, bool* result_out);

MP_CAPI(MpReturnCode) mp_PixelWriteLock__RI(mediapipe::Image* image, mediapipe::PixelWriteLock** pixel_Write_lock_out);
MP_CAPI(void) mp_PixelWriteLock__delete(mediapipe::PixelWriteLock* pixel_Write_lock);
MP_CAPI(uint8*) mp_PixelWriteLock__Pixels(mediapipe::PixelWriteLock* pixel_read_lock);

// Packet API
MP_CAPI(MpReturnCode) mp__MakeImagePacket__Pif(mediapipe::Image* image, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeImagePacket_At__Pif_Rt(mediapipe::Image* image, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__ConsumeImage(mediapipe::Packet* packet, absl::Status **status_out, mediapipe::Image** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetImage(mediapipe::Packet* packet, const mediapipe::Image** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetImageVector(mediapipe::Packet* packet, mp_api::StructArray<mediapipe::Image*>* value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsImage(mediapipe::Packet* packet, absl::Status** status_out);

MP_CAPI(void) mp_api_ImageArray__delete(mediapipe::Image** image_array);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_FORMATS_IMAGE_H_
