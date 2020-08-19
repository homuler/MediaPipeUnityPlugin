#include "mediapipe_api/framework/formats/image_frame.h"

mediapipe::ImageFrame* MpImageFrameCreate(int format_code, int width, int height, uint32 alignment_boundary) {
  auto format = static_cast<mediapipe::ImageFormat::Format>(format_code);

  return new mediapipe::ImageFrame { format, width, height, alignment_boundary };
}

mediapipe::ImageFrame* MpImageFrameCreateDefault() {
  return new mediapipe::ImageFrame();
}

mediapipe::ImageFrame* MpImageFrameCreateWithPixelData(
    int format_code, int width, int height, int width_step, uint8* pixel_data, Deleter deleter) {
  auto format = static_cast<mediapipe::ImageFormat::Format>(format_code);

  return new mediapipe::ImageFrame { format, width, height, width_step, pixel_data, deleter };
}

void MpImageFrameDestroy(mediapipe::ImageFrame* image_frame) {
  return delete image_frame;
}

bool MpImageFrameIsEmpty(mediapipe::ImageFrame* image_frame) {
  return image_frame->IsEmpty();
}

int MpImageFrameFormat(mediapipe::ImageFrame* image_frame) {
  return static_cast<int>(image_frame->Format());
}

int MpImageFrameWidth(mediapipe::ImageFrame* image_frame) {
  return image_frame->Width();
}

int MpImageFrameHeight(mediapipe::ImageFrame* image_frame) {
  return image_frame->Height();
}

int MpImageFrameChannelSize(mediapipe::ImageFrame* image_frame) {
  return image_frame->ChannelSize();
}

int MpImageFrameNumberOfChannels(mediapipe::ImageFrame* image_frame) {
  return image_frame->NumberOfChannels();
}

int MpImageFrameByteDepth(mediapipe::ImageFrame* image_frame) {
  return image_frame->ByteDepth();
}

int MpImageFrameWidthStep(mediapipe::ImageFrame* image_frame) {
  return image_frame->WidthStep();
}

uint8* MpImageFramePixelData(mediapipe::ImageFrame* image_frame) {
  return image_frame->MutablePixelData();
}

void MpStatusOrImageFrameDestroy(MpStatusOrImageFrame* status_or_image_frame) {
  delete status_or_image_frame;
}

MpStatus* MpStatusOrImageFrameStatus(MpStatusOrImageFrame* status_or_image_frame) {
  mediapipe::Status status { *status_or_image_frame->status };

  return new MpStatus { std::move(status) };
}

mediapipe::ImageFrame* MpStatusOrImageFrameConsumeValue(MpStatusOrImageFrame* status_or_image_frame) {
  return status_or_image_frame->value.release();
}

MpPacket* MpMakeImageFramePacketAt(mediapipe::ImageFrame* image_frame, int timestamp) {
  auto packet = mediapipe::Adopt(image_frame).At(mediapipe::Timestamp(timestamp));

  return new MpPacket { std::move(packet) };
}

mediapipe::ImageFrame* MpPacketGetImageFrame(MpPacket* packet) {
  auto holder = static_cast<const UnsafePacketHolder<mediapipe::ImageFrame>*>(mediapipe::packet_internal::GetHolder(*packet->impl));

  return holder->Get();
}

MpStatusOrImageFrame* MpPacketConsumeImageFrame(MpPacket* packet) {
  auto status_or_image_frame = packet->impl->Consume<mediapipe::ImageFrame>();

  return new MpStatusOrImageFrame { std::move(status_or_image_frame) };
}
