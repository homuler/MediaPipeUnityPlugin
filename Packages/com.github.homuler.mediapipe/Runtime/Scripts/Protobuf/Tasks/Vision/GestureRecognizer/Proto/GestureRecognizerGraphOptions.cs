// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: mediapipe/tasks/cc/vision/gesture_recognizer/proto/gesture_recognizer_graph_options.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Mediapipe.Tasks.Vision.GestureRecognizer.Proto {

  /// <summary>Holder for reflection information generated from mediapipe/tasks/cc/vision/gesture_recognizer/proto/gesture_recognizer_graph_options.proto</summary>
  public static partial class GestureRecognizerGraphOptionsReflection {

    #region Descriptor
    /// <summary>File descriptor for mediapipe/tasks/cc/vision/gesture_recognizer/proto/gesture_recognizer_graph_options.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static GestureRecognizerGraphOptionsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ClltZWRpYXBpcGUvdGFza3MvY2MvdmlzaW9uL2dlc3R1cmVfcmVjb2duaXpl",
            "ci9wcm90by9nZXN0dXJlX3JlY29nbml6ZXJfZ3JhcGhfb3B0aW9ucy5wcm90",
            "bxIvbWVkaWFwaXBlLnRhc2tzLnZpc2lvbi5nZXN0dXJlX3JlY29nbml6ZXIu",
            "cHJvdG8aJG1lZGlhcGlwZS9mcmFtZXdvcmsvY2FsY3VsYXRvci5wcm90bxos",
            "bWVkaWFwaXBlL2ZyYW1ld29yay9jYWxjdWxhdG9yX29wdGlvbnMucHJvdG8a",
            "MG1lZGlhcGlwZS90YXNrcy9jYy9jb3JlL3Byb3RvL2Jhc2Vfb3B0aW9ucy5w",
            "cm90bxpebWVkaWFwaXBlL3Rhc2tzL2NjL3Zpc2lvbi9nZXN0dXJlX3JlY29n",
            "bml6ZXIvcHJvdG8vaGFuZF9nZXN0dXJlX3JlY29nbml6ZXJfZ3JhcGhfb3B0",
            "aW9ucy5wcm90bxpTbWVkaWFwaXBlL3Rhc2tzL2NjL3Zpc2lvbi9oYW5kX2xh",
            "bmRtYXJrZXIvcHJvdG8vaGFuZF9sYW5kbWFya2VyX2dyYXBoX29wdGlvbnMu",
            "cHJvdG8i0gMKHUdlc3R1cmVSZWNvZ25pemVyR3JhcGhPcHRpb25zEj0KDGJh",
            "c2Vfb3B0aW9ucxgBIAEoCzInLm1lZGlhcGlwZS50YXNrcy5jb3JlLnByb3Rv",
            "LkJhc2VPcHRpb25zEm8KHWhhbmRfbGFuZG1hcmtlcl9ncmFwaF9vcHRpb25z",
            "GAIgASgLMkgubWVkaWFwaXBlLnRhc2tzLnZpc2lvbi5oYW5kX2xhbmRtYXJr",
            "ZXIucHJvdG8uSGFuZExhbmRtYXJrZXJHcmFwaE9wdGlvbnMSgQEKJWhhbmRf",
            "Z2VzdHVyZV9yZWNvZ25pemVyX2dyYXBoX29wdGlvbnMYAyABKAsyUi5tZWRp",
            "YXBpcGUudGFza3MudmlzaW9uLmdlc3R1cmVfcmVjb2duaXplci5wcm90by5I",
            "YW5kR2VzdHVyZVJlY29nbml6ZXJHcmFwaE9wdGlvbnMyfQoDZXh0EhwubWVk",
            "aWFwaXBlLkNhbGN1bGF0b3JPcHRpb25zGN7hueQBIAEoCzJOLm1lZGlhcGlw",
            "ZS50YXNrcy52aXNpb24uZ2VzdHVyZV9yZWNvZ25pemVyLnByb3RvLkdlc3R1",
            "cmVSZWNvZ25pemVyR3JhcGhPcHRpb25zQl8KOWNvbS5nb29nbGUubWVkaWFw",
            "aXBlLnRhc2tzLnZpc2lvbi5nZXN0dXJlcmVjb2duaXplci5wcm90b0IiR2Vz",
            "dHVyZVJlY29nbml6ZXJHcmFwaE9wdGlvbnNQcm90bw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Mediapipe.CalculatorReflection.Descriptor, global::Mediapipe.CalculatorOptionsReflection.Descriptor, global::Mediapipe.Tasks.Core.Proto.BaseOptionsReflection.Descriptor, global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.HandGestureRecognizerGraphOptionsReflection.Descriptor, global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptionsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.GestureRecognizerGraphOptions), global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.GestureRecognizerGraphOptions.Parser, new[]{ "BaseOptions", "HandLandmarkerGraphOptions", "HandGestureRecognizerGraphOptions" }, null, null, new pb::Extension[] { global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.GestureRecognizerGraphOptions.Extensions.Ext }, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class GestureRecognizerGraphOptions : pb::IMessage<GestureRecognizerGraphOptions>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<GestureRecognizerGraphOptions> _parser = new pb::MessageParser<GestureRecognizerGraphOptions>(() => new GestureRecognizerGraphOptions());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<GestureRecognizerGraphOptions> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.GestureRecognizerGraphOptionsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GestureRecognizerGraphOptions() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GestureRecognizerGraphOptions(GestureRecognizerGraphOptions other) : this() {
      baseOptions_ = other.baseOptions_ != null ? other.baseOptions_.Clone() : null;
      handLandmarkerGraphOptions_ = other.handLandmarkerGraphOptions_ != null ? other.handLandmarkerGraphOptions_.Clone() : null;
      handGestureRecognizerGraphOptions_ = other.handGestureRecognizerGraphOptions_ != null ? other.handGestureRecognizerGraphOptions_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public GestureRecognizerGraphOptions Clone() {
      return new GestureRecognizerGraphOptions(this);
    }

    /// <summary>Field number for the "base_options" field.</summary>
    public const int BaseOptionsFieldNumber = 1;
    private global::Mediapipe.Tasks.Core.Proto.BaseOptions baseOptions_;
    /// <summary>
    /// Base options for configuring gesture recognizer graph, such as specifying
    /// the TfLite model file with metadata, accelerator options, etc.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.Tasks.Core.Proto.BaseOptions BaseOptions {
      get { return baseOptions_; }
      set {
        baseOptions_ = value;
      }
    }

    /// <summary>Field number for the "hand_landmarker_graph_options" field.</summary>
    public const int HandLandmarkerGraphOptionsFieldNumber = 2;
    private global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions handLandmarkerGraphOptions_;
    /// <summary>
    /// Options for configuring hand landmarker graph.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions HandLandmarkerGraphOptions {
      get { return handLandmarkerGraphOptions_; }
      set {
        handLandmarkerGraphOptions_ = value;
      }
    }

    /// <summary>Field number for the "hand_gesture_recognizer_graph_options" field.</summary>
    public const int HandGestureRecognizerGraphOptionsFieldNumber = 3;
    private global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.HandGestureRecognizerGraphOptions handGestureRecognizerGraphOptions_;
    /// <summary>
    /// Options for configuring hand gesture recognizer graph.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.HandGestureRecognizerGraphOptions HandGestureRecognizerGraphOptions {
      get { return handGestureRecognizerGraphOptions_; }
      set {
        handGestureRecognizerGraphOptions_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as GestureRecognizerGraphOptions);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(GestureRecognizerGraphOptions other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(BaseOptions, other.BaseOptions)) return false;
      if (!object.Equals(HandLandmarkerGraphOptions, other.HandLandmarkerGraphOptions)) return false;
      if (!object.Equals(HandGestureRecognizerGraphOptions, other.HandGestureRecognizerGraphOptions)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (baseOptions_ != null) hash ^= BaseOptions.GetHashCode();
      if (handLandmarkerGraphOptions_ != null) hash ^= HandLandmarkerGraphOptions.GetHashCode();
      if (handGestureRecognizerGraphOptions_ != null) hash ^= HandGestureRecognizerGraphOptions.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (baseOptions_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(BaseOptions);
      }
      if (handLandmarkerGraphOptions_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(HandLandmarkerGraphOptions);
      }
      if (handGestureRecognizerGraphOptions_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(HandGestureRecognizerGraphOptions);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (baseOptions_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(BaseOptions);
      }
      if (handLandmarkerGraphOptions_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(HandLandmarkerGraphOptions);
      }
      if (handGestureRecognizerGraphOptions_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(HandGestureRecognizerGraphOptions);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (baseOptions_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(BaseOptions);
      }
      if (handLandmarkerGraphOptions_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(HandLandmarkerGraphOptions);
      }
      if (handGestureRecognizerGraphOptions_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(HandGestureRecognizerGraphOptions);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(GestureRecognizerGraphOptions other) {
      if (other == null) {
        return;
      }
      if (other.baseOptions_ != null) {
        if (baseOptions_ == null) {
          BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
        }
        BaseOptions.MergeFrom(other.BaseOptions);
      }
      if (other.handLandmarkerGraphOptions_ != null) {
        if (handLandmarkerGraphOptions_ == null) {
          HandLandmarkerGraphOptions = new global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions();
        }
        HandLandmarkerGraphOptions.MergeFrom(other.HandLandmarkerGraphOptions);
      }
      if (other.handGestureRecognizerGraphOptions_ != null) {
        if (handGestureRecognizerGraphOptions_ == null) {
          HandGestureRecognizerGraphOptions = new global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.HandGestureRecognizerGraphOptions();
        }
        HandGestureRecognizerGraphOptions.MergeFrom(other.HandGestureRecognizerGraphOptions);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (baseOptions_ == null) {
              BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
            }
            input.ReadMessage(BaseOptions);
            break;
          }
          case 18: {
            if (handLandmarkerGraphOptions_ == null) {
              HandLandmarkerGraphOptions = new global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions();
            }
            input.ReadMessage(HandLandmarkerGraphOptions);
            break;
          }
          case 26: {
            if (handGestureRecognizerGraphOptions_ == null) {
              HandGestureRecognizerGraphOptions = new global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.HandGestureRecognizerGraphOptions();
            }
            input.ReadMessage(HandGestureRecognizerGraphOptions);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (baseOptions_ == null) {
              BaseOptions = new global::Mediapipe.Tasks.Core.Proto.BaseOptions();
            }
            input.ReadMessage(BaseOptions);
            break;
          }
          case 18: {
            if (handLandmarkerGraphOptions_ == null) {
              HandLandmarkerGraphOptions = new global::Mediapipe.Tasks.Vision.HandLandmarker.Proto.HandLandmarkerGraphOptions();
            }
            input.ReadMessage(HandLandmarkerGraphOptions);
            break;
          }
          case 26: {
            if (handGestureRecognizerGraphOptions_ == null) {
              HandGestureRecognizerGraphOptions = new global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.HandGestureRecognizerGraphOptions();
            }
            input.ReadMessage(HandGestureRecognizerGraphOptions);
            break;
          }
        }
      }
    }
    #endif

    #region Extensions
    /// <summary>Container for extensions for other messages declared in the GestureRecognizerGraphOptions message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Extensions {
      public static readonly pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.GestureRecognizerGraphOptions> Ext =
        new pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.GestureRecognizerGraphOptions>(479097054, pb::FieldCodec.ForMessage(3832776434, global::Mediapipe.Tasks.Vision.GestureRecognizer.Proto.GestureRecognizerGraphOptions.Parser));
    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
