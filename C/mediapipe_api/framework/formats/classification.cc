#include "mediapipe_api/framework/formats/classification.h"

MpSerializedProto* MpPacketGetClassificationList(MpPacket* packet) {
  return MpPacketGetProto<mediapipe::ClassificationList>(packet);
}
