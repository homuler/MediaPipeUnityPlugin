#include "mediapipe_api/framework/formats/classification.h"

void MpClassificationListDestroy(MpClassificationList* classification_list) {
  delete classification_list;
}

MpClassification* MpClassificationListClassifications(MpClassificationList* classification_list) {
  return classification_list->classifications;
}

int MpClassificationListSize(MpClassificationList* classification_list) {
  return classification_list->size;
}

MpClassificationList* MpPacketGetClassificationList(MpPacket* packet) {
  auto& classification_list = packet->impl->Get<mediapipe::ClassificationList>();
  int size = classification_list.classification_size();

  MpClassification* classifications = new MpClassification[size];

  for (int i = 0; i < size; ++i) {
    const auto& classification = classification_list.classification(i);
    const auto& label_src = classification.label();

    char* label = new char[label_src.size() + 1];
    snprintf(label, label_src.size() + 1, label_src.c_str());

    classifications[i] = MpClassification { classification.index(), classification.score(), label };
  }

  return new MpClassificationList { classifications, size };
}
