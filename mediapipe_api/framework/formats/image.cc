#include "mediapipe_api/framework/formats/image.h"

MpReturnCode mp_Image__ui_i_i_i_Pui8_PF(mediapipe::ImageFormat::Format format, int width, int height, int width_step, uint8* pixel_data,
                                        Deleter* deleter, mediapipe::Image** image_out) {
  TRY_ALL
    *image_out = new mediapipe::Image{std::make_shared<mediapipe::ImageFrame>(format, width, height, width_step, pixel_data, deleter)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

#if !MEDIAPIPE_DISABLE_GPU

#if !MEDIAPIPE_GPU_BUFFER_USE_CV_PIXEL_BUFFER
MpReturnCode mp_Image__ui_ui_i_i_ui_PF_PSgc(GLenum target, GLuint name, int width, int height, mediapipe::GpuBufferFormat format,
                                            GlTextureBufferDeletionCallback* deletion_callback,
                                            std::shared_ptr<mediapipe::GlContext>* producer_context,
                                            mediapipe::Image** image_out) {
  TRY_ALL
    auto callback = [name, deletion_callback](mediapipe::GlSyncToken token) -> void {
      deletion_callback(name, new mediapipe::GlSyncToken{token});
    };
    *image_out = new mediapipe::Image{std::make_shared<mediapipe::GlTextureBuffer>(
        GL_TEXTURE_2D,
        name,
        width,
        height,
        format,
        callback,
        *producer_context
    )};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}
#endif

#endif

void mp_Image__delete(mediapipe::Image* image) {
  delete image;
}

int mp_Image__width(mediapipe::Image* image) {
  return image->width();
}

int mp_Image__height(mediapipe::Image* image) {
  return image->height();
}

int mp_Image__channels(mediapipe::Image* image) {
  return image->channels();
}

int mp_Image__step(mediapipe::Image* image) {
  return image->step();
}

bool mp_Image__UsesGpu(mediapipe::Image* image) {
  return image->UsesGpu();
}

mediapipe::ImageFormat::Format mp_Image__image_format(mediapipe::Image* image) {
  return image->image_format();
}

mediapipe::GpuBufferFormat mp_Image__format(mediapipe::Image* image) {
  return image->format();
}

MpReturnCode mp_Image__ConvertToCpu(mediapipe::Image* image, bool* result_out) {
  TRY_ALL
    *result_out = image->ConvertToCpu();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Image__ConvertToGpu(mediapipe::Image* image, bool* result_out) {
  TRY_ALL
    *result_out = image->ConvertToGpu();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_PixelWriteLock__RI(mediapipe::Image* image, mediapipe::PixelWriteLock** pixel_Write_lock_out) {
  TRY
    *pixel_Write_lock_out = new mediapipe::PixelWriteLock{image};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_PixelWriteLock__delete(mediapipe::PixelWriteLock* pixel_Write_lock) {
  delete pixel_Write_lock;
}

uint8* mp_PixelWriteLock__Pixels(mediapipe::PixelWriteLock* pixel_read_lock) {
  return pixel_read_lock->Pixels();
}

// Packet API
MpReturnCode mp__MakeImagePacket__Pif(mediapipe::Image* image, mediapipe::Packet** packet_out) {
  TRY_ALL
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Image>(std::move(*image))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp__MakeImagePacket_At__Pif_Rt(mediapipe::Image* image, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY_ALL
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Image>(std::move(*image)).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ConsumeImage(mediapipe::Packet* packet, absl::Status **status_out, mediapipe::Image** value_out) {
  return mp_Packet__Consume(packet, status_out, value_out);
}

MpReturnCode mp_Packet__GetImage(mediapipe::Packet* packet, const mediapipe::Image** value_out) {
  return mp_Packet__Get(packet, value_out);
}

MpReturnCode mp_Packet__GetImageVector(mediapipe::Packet* packet, mp_api::StructArray<mediapipe::Image*>* value_out) {
  TRY_ALL
    auto vec = packet->Get<std::vector<mediapipe::Image>>();
    auto size = vec.size();
    auto data = new mediapipe::Image*[size];

    for (auto i = 0; i < size; ++i) {
      data[i] = new mediapipe::Image(std::move(vec[i]));
    }
    value_out->data = data;
    value_out->size = static_cast<int>(size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ValidateAsImage(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<mediapipe::Image>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_api_ImageArray__delete(mediapipe::Image** image_array) {
  delete[] image_array;
}
