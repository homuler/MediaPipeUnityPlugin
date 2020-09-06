using System;
using System.Runtime.InteropServices;

using MpAssociatedDetection = System.IntPtr;
using MpDetection = System.IntPtr;
using MpDetectionArray = System.IntPtr;
using MpLocationData = System.IntPtr;

namespace Mediapipe {
  public class Detection {
    public string[] label;
    public int[] labelId;
    public float[] score;
    public LocationData locationData;
    public string featureTag;
    public string trackId;
    public Int64 detectionId;
    public AssociatedDetection[] associatedDetections;
    public string[] displayName;
    public Int64 timestampUsec;

    public unsafe Detection(MpDetection detectionPtr) {
      var inner = Marshal.PtrToStructure<DetectionInner>(detectionPtr);
      var scoreSize = inner.scoreSize;

      score = new float[scoreSize];

      bool isLabelPresent = (IntPtr)inner.label != IntPtr.Zero;
      bool isLabelIdPresent = (IntPtr)inner.labelId != IntPtr.Zero;
      bool isDisplayNamePresent = (IntPtr)inner.displayName != IntPtr.Zero;

      label = isLabelPresent ? new string[scoreSize] : null;
      labelId = isLabelIdPresent ? new int[scoreSize] : null;
      displayName = isDisplayNamePresent ? new string[scoreSize] : null;

      var labelPtr = inner.label;
      var labelIdPtr = inner.labelId;
      var scorePtr = inner.score;
      var displayNamePtr = inner.displayName;

      for (var i = 0; i < scoreSize; i++) {
        if (isLabelPresent) { label[i] = Marshal.PtrToStringAnsi((IntPtr)(*labelPtr++)); }
        if (isLabelIdPresent) { labelId[i] = *labelIdPtr++; }
        score[i] = *scorePtr++;
        if (isDisplayNamePresent) { displayName[i] = Marshal.PtrToStringAnsi((IntPtr)(*displayNamePtr++)); }
      }

      locationData = new LocationData(inner.locationData);
      featureTag = Marshal.PtrToStringAnsi((IntPtr)inner.featureTag);
      trackId = Marshal.PtrToStringAnsi((IntPtr)inner.trackId);
      detectionId = inner.detectionId;
      associatedDetections = AssociatedDetection.PtrToArray((IntPtr)inner.associatedDetections, inner.associatedDetectionsSize);
      timestampUsec = inner.timestampUsec;
    }

    public static Detection[] PtrToArray(MpDetectionArray ptr, int size) {
      var detections = new Detection[size];

      if (size == 0) { return detections; }

      unsafe {
        DetectionInner** arr = (DetectionInner**)ptr;

        for (var i = 0; i < size; i++) {
          detections[i] = new Detection((IntPtr)(*arr++));
        }
      }

      return detections;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct DetectionInner {
      public char** label;
      public int* labelId;
      public float* score;
      public MpLocationData locationData;
      public char* featureTag;
      public char* trackId;
      public Int64 detectionId;
      public MpAssociatedDetection* associatedDetections;
      public char** displayName;
      public Int64 timestampUsec;
      public int scoreSize;
      public int associatedDetectionsSize;
    }
  }
}
