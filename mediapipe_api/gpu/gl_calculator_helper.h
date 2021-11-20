// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_
#define MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_

#include <memory>

#include "mediapipe/gpu/gl_calculator_helper.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/absl/status.h"

extern "C" {

typedef absl::Status* NativeGlStatusFunction();

/** GlCalculatorHelper API */
MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__(mediapipe::GlCalculatorHelper** gl_calculator_helper_out);
MP_CAPI(void) mp_GlCalculatorHelper__delete(mediapipe::GlCalculatorHelper* gl_calculator_helper);
MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__InitializeForTest__Pgr(mediapipe::GlCalculatorHelper* gl_calculator_helper,
                                                                    mediapipe::GpuResources* gpu_resources);
MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__RunInGlContext__PF(mediapipe::GlCalculatorHelper* gl_calculator_helper, NativeGlStatusFunction* gl_func,
                                                                absl::Status** status_out);

MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__CreateSourceTexture__Rif(mediapipe::GlCalculatorHelper* gl_calculator_helper, mediapipe::ImageFrame* image_frame,
                                                                      mediapipe::GlTexture** gl_texture_out);
MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__CreateSourceTexture__Rgb(mediapipe::GlCalculatorHelper* gl_calculator_helper, mediapipe::GpuBuffer* gpu_buffer,
                                                                      mediapipe::GlTexture** gl_texture_out);

#ifdef __APPLE__
MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__CreateSourceTexture__Rgb_i(mediapipe::GlCalculatorHelper* gl_calculator_helper, mediapipe::GpuBuffer* gpu_buffer,
                                                                        int plane, mediapipe::GlTexture** gl_texture_out);
#endif  // __APPLE__

MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__CreateDestinationTexture__i_i_ui(mediapipe::GlCalculatorHelper* gl_calculator_helper, int output_width,
                                                                              int output_height, mediapipe::GpuBufferFormat format,
                                                                              mediapipe::GlTexture** gl_texture_out);
MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__CreateDestinationTexture__Rgb(mediapipe::GlCalculatorHelper* gl_calculator_helper,
                                                                           mediapipe::GpuBuffer* gpu_buffer, mediapipe::GlTexture** gl_texture_out);
MP_CAPI(GLuint) mp_GlCalculatorHelper__framebuffer(mediapipe::GlCalculatorHelper* gl_calculator_helper);
MP_CAPI(MpReturnCode) mp_GlCalculatorHelper__BindFrameBuffer__Rtexture(mediapipe::GlCalculatorHelper* gl_calculator_helper, mediapipe::GlTexture* gl_texture);
MP_CAPI(mediapipe::GlContext&) mp_GlCalculatorHelper__GetGlContext(mediapipe::GlCalculatorHelper* gl_calculator_helper);
MP_CAPI(bool) mp_GlCalculatorHelper__Initialized(mediapipe::GlCalculatorHelper* gl_calculator_helper);

/** GlTexture API */
MP_CAPI(MpReturnCode) mp_GlTexture__(mediapipe::GlTexture** gl_texture_out);
MP_CAPI(void) mp_GlTexture__delete(mediapipe::GlTexture* gl_texture);
MP_CAPI(int) mp_GlTexture__width(mediapipe::GlTexture* gl_texture);
MP_CAPI(int) mp_GlTexture__height(mediapipe::GlTexture* gl_texture);
MP_CAPI(GLenum) mp_GlTexture__target(mediapipe::GlTexture* gl_texture);
MP_CAPI(GLuint) mp_GlTexture__name(mediapipe::GlTexture* gl_texture);
MP_CAPI(MpReturnCode) mp_GlTexture__Release(mediapipe::GlTexture* gl_texture);
MP_CAPI(MpReturnCode) mp_GlTexture__GetGpuBufferFrame(mediapipe::GlTexture* gl_texture, mediapipe::GpuBuffer** gpu_buffer_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_
