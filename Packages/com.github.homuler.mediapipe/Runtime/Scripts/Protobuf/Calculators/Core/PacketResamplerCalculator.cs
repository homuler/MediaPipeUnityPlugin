// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: mediapipe/calculators/core/packet_resampler_calculator.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Mediapipe {

  /// <summary>Holder for reflection information generated from mediapipe/calculators/core/packet_resampler_calculator.proto</summary>
  public static partial class PacketResamplerCalculatorReflection {

    #region Descriptor
    /// <summary>File descriptor for mediapipe/calculators/core/packet_resampler_calculator.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PacketResamplerCalculatorReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjxtZWRpYXBpcGUvY2FsY3VsYXRvcnMvY29yZS9wYWNrZXRfcmVzYW1wbGVy",
            "X2NhbGN1bGF0b3IucHJvdG8SCW1lZGlhcGlwZRokbWVkaWFwaXBlL2ZyYW1l",
            "d29yay9jYWxjdWxhdG9yLnByb3RvIsoECiBQYWNrZXRSZXNhbXBsZXJDYWxj",
            "dWxhdG9yT3B0aW9ucxIWCgpmcmFtZV9yYXRlGAEgASgBOgItMRJVCg1vdXRw",
            "dXRfaGVhZGVyGAIgASgOMjgubWVkaWFwaXBlLlBhY2tldFJlc2FtcGxlckNh",
            "bGN1bGF0b3JPcHRpb25zLk91dHB1dEhlYWRlcjoETk9ORRIfChFmbHVzaF9s",
            "YXN0X3BhY2tldBgDIAEoCDoEdHJ1ZRIOCgZqaXR0ZXIYBCABKAESJQoWaml0",
            "dGVyX3dpdGhfcmVmbGVjdGlvbhgJIAEoCDoFZmFsc2USJAoVcmVwcm9kdWNp",
            "YmxlX3NhbXBsaW5nGAogASgIOgVmYWxzZRIWCg5iYXNlX3RpbWVzdGFtcBgF",
            "IAEoAxISCgpzdGFydF90aW1lGAYgASgDEhAKCGVuZF90aW1lGAcgASgDEhsK",
            "DHJvdW5kX2xpbWl0cxgIIAEoCDoFZmFsc2USIwoUdXNlX2lucHV0X2ZyYW1l",
            "X3JhdGUYCyABKAg6BWZhbHNlEhoKDm1heF9mcmFtZV9yYXRlGAwgASgBOgIt",
            "MSJCCgxPdXRwdXRIZWFkZXISCAoETk9ORRAAEg8KC1BBU1NfSEVBREVSEAES",
            "FwoTVVBEQVRFX1ZJREVPX0hFQURFUhACMlkKA2V4dBIcLm1lZGlhcGlwZS5D",
            "YWxjdWxhdG9yT3B0aW9ucxjk3tMtIAEoCzIrLm1lZGlhcGlwZS5QYWNrZXRS",
            "ZXNhbXBsZXJDYWxjdWxhdG9yT3B0aW9ucw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Mediapipe.CalculatorReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Mediapipe.PacketResamplerCalculatorOptions), global::Mediapipe.PacketResamplerCalculatorOptions.Parser, new[]{ "FrameRate", "OutputHeader", "FlushLastPacket", "Jitter", "JitterWithReflection", "ReproducibleSampling", "BaseTimestamp", "StartTime", "EndTime", "RoundLimits", "UseInputFrameRate", "MaxFrameRate" }, null, new[]{ typeof(global::Mediapipe.PacketResamplerCalculatorOptions.Types.OutputHeader) }, new pb::Extension[] { global::Mediapipe.PacketResamplerCalculatorOptions.Extensions.Ext }, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class PacketResamplerCalculatorOptions : pb::IMessage<PacketResamplerCalculatorOptions>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<PacketResamplerCalculatorOptions> _parser = new pb::MessageParser<PacketResamplerCalculatorOptions>(() => new PacketResamplerCalculatorOptions());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<PacketResamplerCalculatorOptions> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Mediapipe.PacketResamplerCalculatorReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PacketResamplerCalculatorOptions() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PacketResamplerCalculatorOptions(PacketResamplerCalculatorOptions other) : this() {
      _hasBits0 = other._hasBits0;
      frameRate_ = other.frameRate_;
      outputHeader_ = other.outputHeader_;
      flushLastPacket_ = other.flushLastPacket_;
      jitter_ = other.jitter_;
      jitterWithReflection_ = other.jitterWithReflection_;
      reproducibleSampling_ = other.reproducibleSampling_;
      baseTimestamp_ = other.baseTimestamp_;
      startTime_ = other.startTime_;
      endTime_ = other.endTime_;
      roundLimits_ = other.roundLimits_;
      useInputFrameRate_ = other.useInputFrameRate_;
      maxFrameRate_ = other.maxFrameRate_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PacketResamplerCalculatorOptions Clone() {
      return new PacketResamplerCalculatorOptions(this);
    }

    /// <summary>Field number for the "frame_rate" field.</summary>
    public const int FrameRateFieldNumber = 1;
    private readonly static double FrameRateDefaultValue = -1D;

    private double frameRate_;
    /// <summary>
    /// The output frame rate measured in frames per second.
    ///
    /// The closest packet in time in each period will be chosen. If there
    /// is no packet in the period then the most recent packet will be chosen
    /// (not the closest in time).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double FrameRate {
      get { if ((_hasBits0 & 1) != 0) { return frameRate_; } else { return FrameRateDefaultValue; } }
      set {
        _hasBits0 |= 1;
        frameRate_ = value;
      }
    }
    /// <summary>Gets whether the "frame_rate" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasFrameRate {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "frame_rate" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearFrameRate() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "output_header" field.</summary>
    public const int OutputHeaderFieldNumber = 2;
    private readonly static global::Mediapipe.PacketResamplerCalculatorOptions.Types.OutputHeader OutputHeaderDefaultValue = global::Mediapipe.PacketResamplerCalculatorOptions.Types.OutputHeader.None;

    private global::Mediapipe.PacketResamplerCalculatorOptions.Types.OutputHeader outputHeader_;
    /// <summary>
    /// Whether and what kind of header to place on the output stream.
    /// Note, this is about the actual header, not the VIDEO_HEADER stream.
    /// If this option is set to UPDATE_VIDEO_HEADER then the header will
    /// also be parsed (updated) and passed along to the VIDEO_HEADER stream.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Mediapipe.PacketResamplerCalculatorOptions.Types.OutputHeader OutputHeader {
      get { if ((_hasBits0 & 2) != 0) { return outputHeader_; } else { return OutputHeaderDefaultValue; } }
      set {
        _hasBits0 |= 2;
        outputHeader_ = value;
      }
    }
    /// <summary>Gets whether the "output_header" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasOutputHeader {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "output_header" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearOutputHeader() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "flush_last_packet" field.</summary>
    public const int FlushLastPacketFieldNumber = 3;
    private readonly static bool FlushLastPacketDefaultValue = true;

    private bool flushLastPacket_;
    /// <summary>
    /// Flush last packet even if its timestamp is greater than the final stream
    /// timestamp.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool FlushLastPacket {
      get { if ((_hasBits0 & 4) != 0) { return flushLastPacket_; } else { return FlushLastPacketDefaultValue; } }
      set {
        _hasBits0 |= 4;
        flushLastPacket_ = value;
      }
    }
    /// <summary>Gets whether the "flush_last_packet" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasFlushLastPacket {
      get { return (_hasBits0 & 4) != 0; }
    }
    /// <summary>Clears the value of the "flush_last_packet" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearFlushLastPacket() {
      _hasBits0 &= ~4;
    }

    /// <summary>Field number for the "jitter" field.</summary>
    public const int JitterFieldNumber = 4;
    private readonly static double JitterDefaultValue = 0D;

    private double jitter_;
    /// <summary>
    /// Adds jitter to resampling if set, so that Google's sampling is not
    /// externally deterministic.
    ///
    /// When set, the randomizer will be initialized with a seed.  Then, the first
    /// sample is chosen randomly (uniform distribution) among frames that
    /// correspond to timestamps [0, 1/frame_rate).  Let the chosen frame
    /// correspond to timestamp t.  The next frame is chosen randomly (uniform
    /// distribution) among frames that correspond to [t+(1-jitter)/frame_rate,
    /// t+(1+jitter)/frame_rate].  t is updated and the process is repeated.
    ///
    /// Valid values are in the range of [0.0, 1.0] with the default being 0.0 (no
    /// jitter).  A typical value would be a value in the range of 0.1-0.25.
    ///
    /// Note that this does NOT guarantee the desired frame rate, but if the
    /// pseudo-random number generator does its job and the number of frames is
    /// sufficiently large, the average frame rate will be close to this value.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double Jitter {
      get { if ((_hasBits0 & 8) != 0) { return jitter_; } else { return JitterDefaultValue; } }
      set {
        _hasBits0 |= 8;
        jitter_ = value;
      }
    }
    /// <summary>Gets whether the "jitter" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasJitter {
      get { return (_hasBits0 & 8) != 0; }
    }
    /// <summary>Clears the value of the "jitter" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearJitter() {
      _hasBits0 &= ~8;
    }

    /// <summary>Field number for the "jitter_with_reflection" field.</summary>
    public const int JitterWithReflectionFieldNumber = 9;
    private readonly static bool JitterWithReflectionDefaultValue = false;

    private bool jitterWithReflection_;
    /// <summary>
    /// Enables reflection when applying jitter.
    ///
    /// This option is ignored when reproducible_sampling is true, in which case
    /// reflection will be used.
    ///
    /// New use cases should use reproducible_sampling = true, as
    /// jitter_with_reflection is deprecated and will be removed at some point.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool JitterWithReflection {
      get { if ((_hasBits0 & 256) != 0) { return jitterWithReflection_; } else { return JitterWithReflectionDefaultValue; } }
      set {
        _hasBits0 |= 256;
        jitterWithReflection_ = value;
      }
    }
    /// <summary>Gets whether the "jitter_with_reflection" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasJitterWithReflection {
      get { return (_hasBits0 & 256) != 0; }
    }
    /// <summary>Clears the value of the "jitter_with_reflection" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearJitterWithReflection() {
      _hasBits0 &= ~256;
    }

    /// <summary>Field number for the "reproducible_sampling" field.</summary>
    public const int ReproducibleSamplingFieldNumber = 10;
    private readonly static bool ReproducibleSamplingDefaultValue = false;

    private bool reproducibleSampling_;
    /// <summary>
    /// If set, enabled reproducible sampling, allowing frames to be sampled
    /// without regards to where the stream starts.  See
    /// packet_resampler_calculator.h for details.
    ///
    /// This enables reflection (ignoring jitter_with_reflection setting).
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool ReproducibleSampling {
      get { if ((_hasBits0 & 512) != 0) { return reproducibleSampling_; } else { return ReproducibleSamplingDefaultValue; } }
      set {
        _hasBits0 |= 512;
        reproducibleSampling_ = value;
      }
    }
    /// <summary>Gets whether the "reproducible_sampling" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasReproducibleSampling {
      get { return (_hasBits0 & 512) != 0; }
    }
    /// <summary>Clears the value of the "reproducible_sampling" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearReproducibleSampling() {
      _hasBits0 &= ~512;
    }

    /// <summary>Field number for the "base_timestamp" field.</summary>
    public const int BaseTimestampFieldNumber = 5;
    private readonly static long BaseTimestampDefaultValue = 0L;

    private long baseTimestamp_;
    /// <summary>
    /// If specified, output timestamps are aligned with base_timestamp.
    /// Otherwise, they are aligned with the first input timestamp.
    ///
    /// In order to ensure that the outptut timestamps are reproducible,
    /// with round_limits = false, the bounds for input timestamps must include:
    ///   [start_time - period / 2, end_time + period / 2],
    /// with round_limits = true, the bounds for input timestamps must include:
    ///   [start_time - period, end_time + period],
    /// where period = 1 / frame_rate.
    ///
    /// For example, in PacketResamplerCalculatorOptions specify
    /// "start_time: 3000000", and in MediaDecoderOptions specify
    /// "start_time: 2999950".
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long BaseTimestamp {
      get { if ((_hasBits0 & 16) != 0) { return baseTimestamp_; } else { return BaseTimestampDefaultValue; } }
      set {
        _hasBits0 |= 16;
        baseTimestamp_ = value;
      }
    }
    /// <summary>Gets whether the "base_timestamp" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasBaseTimestamp {
      get { return (_hasBits0 & 16) != 0; }
    }
    /// <summary>Clears the value of the "base_timestamp" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearBaseTimestamp() {
      _hasBits0 &= ~16;
    }

    /// <summary>Field number for the "start_time" field.</summary>
    public const int StartTimeFieldNumber = 6;
    private readonly static long StartTimeDefaultValue = 0L;

    private long startTime_;
    /// <summary>
    /// If specified, only outputs at/after start_time are included.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long StartTime {
      get { if ((_hasBits0 & 32) != 0) { return startTime_; } else { return StartTimeDefaultValue; } }
      set {
        _hasBits0 |= 32;
        startTime_ = value;
      }
    }
    /// <summary>Gets whether the "start_time" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasStartTime {
      get { return (_hasBits0 & 32) != 0; }
    }
    /// <summary>Clears the value of the "start_time" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearStartTime() {
      _hasBits0 &= ~32;
    }

    /// <summary>Field number for the "end_time" field.</summary>
    public const int EndTimeFieldNumber = 7;
    private readonly static long EndTimeDefaultValue = 0L;

    private long endTime_;
    /// <summary>
    /// If specified, only outputs before end_time are included.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long EndTime {
      get { if ((_hasBits0 & 64) != 0) { return endTime_; } else { return EndTimeDefaultValue; } }
      set {
        _hasBits0 |= 64;
        endTime_ = value;
      }
    }
    /// <summary>Gets whether the "end_time" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasEndTime {
      get { return (_hasBits0 & 64) != 0; }
    }
    /// <summary>Clears the value of the "end_time" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearEndTime() {
      _hasBits0 &= ~64;
    }

    /// <summary>Field number for the "round_limits" field.</summary>
    public const int RoundLimitsFieldNumber = 8;
    private readonly static bool RoundLimitsDefaultValue = false;

    private bool roundLimits_;
    /// <summary>
    /// If set, the output timestamps nearest to start_time and end_time
    /// are included in the output, even if the nearest timestamp is not
    /// between start_time and end_time.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool RoundLimits {
      get { if ((_hasBits0 & 128) != 0) { return roundLimits_; } else { return RoundLimitsDefaultValue; } }
      set {
        _hasBits0 |= 128;
        roundLimits_ = value;
      }
    }
    /// <summary>Gets whether the "round_limits" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasRoundLimits {
      get { return (_hasBits0 & 128) != 0; }
    }
    /// <summary>Clears the value of the "round_limits" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearRoundLimits() {
      _hasBits0 &= ~128;
    }

    /// <summary>Field number for the "use_input_frame_rate" field.</summary>
    public const int UseInputFrameRateFieldNumber = 11;
    private readonly static bool UseInputFrameRateDefaultValue = false;

    private bool useInputFrameRate_;
    /// <summary>
    /// If set, the output frame rate is the same as the input frame rate.
    /// You need to provide the frame rate of the input images in the header in the
    /// input_side_packet.
    /// This option only makes sense in combination with max_frame_rate. It will
    /// hold on to the original frame rate unless it's higher than the
    /// max_frame_rate.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool UseInputFrameRate {
      get { if ((_hasBits0 & 1024) != 0) { return useInputFrameRate_; } else { return UseInputFrameRateDefaultValue; } }
      set {
        _hasBits0 |= 1024;
        useInputFrameRate_ = value;
      }
    }
    /// <summary>Gets whether the "use_input_frame_rate" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasUseInputFrameRate {
      get { return (_hasBits0 & 1024) != 0; }
    }
    /// <summary>Clears the value of the "use_input_frame_rate" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearUseInputFrameRate() {
      _hasBits0 &= ~1024;
    }

    /// <summary>Field number for the "max_frame_rate" field.</summary>
    public const int MaxFrameRateFieldNumber = 12;
    private readonly static double MaxFrameRateDefaultValue = -1D;

    private double maxFrameRate_;
    /// <summary>
    /// If set, the output frame rate is limited to this value.
    /// You need to provide the frame rate of the input images in the header in the
    /// input_side_packet.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public double MaxFrameRate {
      get { if ((_hasBits0 & 2048) != 0) { return maxFrameRate_; } else { return MaxFrameRateDefaultValue; } }
      set {
        _hasBits0 |= 2048;
        maxFrameRate_ = value;
      }
    }
    /// <summary>Gets whether the "max_frame_rate" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasMaxFrameRate {
      get { return (_hasBits0 & 2048) != 0; }
    }
    /// <summary>Clears the value of the "max_frame_rate" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearMaxFrameRate() {
      _hasBits0 &= ~2048;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as PacketResamplerCalculatorOptions);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(PacketResamplerCalculatorOptions other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(FrameRate, other.FrameRate)) return false;
      if (OutputHeader != other.OutputHeader) return false;
      if (FlushLastPacket != other.FlushLastPacket) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Jitter, other.Jitter)) return false;
      if (JitterWithReflection != other.JitterWithReflection) return false;
      if (ReproducibleSampling != other.ReproducibleSampling) return false;
      if (BaseTimestamp != other.BaseTimestamp) return false;
      if (StartTime != other.StartTime) return false;
      if (EndTime != other.EndTime) return false;
      if (RoundLimits != other.RoundLimits) return false;
      if (UseInputFrameRate != other.UseInputFrameRate) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(MaxFrameRate, other.MaxFrameRate)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasFrameRate) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(FrameRate);
      if (HasOutputHeader) hash ^= OutputHeader.GetHashCode();
      if (HasFlushLastPacket) hash ^= FlushLastPacket.GetHashCode();
      if (HasJitter) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Jitter);
      if (HasJitterWithReflection) hash ^= JitterWithReflection.GetHashCode();
      if (HasReproducibleSampling) hash ^= ReproducibleSampling.GetHashCode();
      if (HasBaseTimestamp) hash ^= BaseTimestamp.GetHashCode();
      if (HasStartTime) hash ^= StartTime.GetHashCode();
      if (HasEndTime) hash ^= EndTime.GetHashCode();
      if (HasRoundLimits) hash ^= RoundLimits.GetHashCode();
      if (HasUseInputFrameRate) hash ^= UseInputFrameRate.GetHashCode();
      if (HasMaxFrameRate) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(MaxFrameRate);
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
      if (HasFrameRate) {
        output.WriteRawTag(9);
        output.WriteDouble(FrameRate);
      }
      if (HasOutputHeader) {
        output.WriteRawTag(16);
        output.WriteEnum((int) OutputHeader);
      }
      if (HasFlushLastPacket) {
        output.WriteRawTag(24);
        output.WriteBool(FlushLastPacket);
      }
      if (HasJitter) {
        output.WriteRawTag(33);
        output.WriteDouble(Jitter);
      }
      if (HasBaseTimestamp) {
        output.WriteRawTag(40);
        output.WriteInt64(BaseTimestamp);
      }
      if (HasStartTime) {
        output.WriteRawTag(48);
        output.WriteInt64(StartTime);
      }
      if (HasEndTime) {
        output.WriteRawTag(56);
        output.WriteInt64(EndTime);
      }
      if (HasRoundLimits) {
        output.WriteRawTag(64);
        output.WriteBool(RoundLimits);
      }
      if (HasJitterWithReflection) {
        output.WriteRawTag(72);
        output.WriteBool(JitterWithReflection);
      }
      if (HasReproducibleSampling) {
        output.WriteRawTag(80);
        output.WriteBool(ReproducibleSampling);
      }
      if (HasUseInputFrameRate) {
        output.WriteRawTag(88);
        output.WriteBool(UseInputFrameRate);
      }
      if (HasMaxFrameRate) {
        output.WriteRawTag(97);
        output.WriteDouble(MaxFrameRate);
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
      if (HasFrameRate) {
        output.WriteRawTag(9);
        output.WriteDouble(FrameRate);
      }
      if (HasOutputHeader) {
        output.WriteRawTag(16);
        output.WriteEnum((int) OutputHeader);
      }
      if (HasFlushLastPacket) {
        output.WriteRawTag(24);
        output.WriteBool(FlushLastPacket);
      }
      if (HasJitter) {
        output.WriteRawTag(33);
        output.WriteDouble(Jitter);
      }
      if (HasBaseTimestamp) {
        output.WriteRawTag(40);
        output.WriteInt64(BaseTimestamp);
      }
      if (HasStartTime) {
        output.WriteRawTag(48);
        output.WriteInt64(StartTime);
      }
      if (HasEndTime) {
        output.WriteRawTag(56);
        output.WriteInt64(EndTime);
      }
      if (HasRoundLimits) {
        output.WriteRawTag(64);
        output.WriteBool(RoundLimits);
      }
      if (HasJitterWithReflection) {
        output.WriteRawTag(72);
        output.WriteBool(JitterWithReflection);
      }
      if (HasReproducibleSampling) {
        output.WriteRawTag(80);
        output.WriteBool(ReproducibleSampling);
      }
      if (HasUseInputFrameRate) {
        output.WriteRawTag(88);
        output.WriteBool(UseInputFrameRate);
      }
      if (HasMaxFrameRate) {
        output.WriteRawTag(97);
        output.WriteDouble(MaxFrameRate);
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
      if (HasFrameRate) {
        size += 1 + 8;
      }
      if (HasOutputHeader) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) OutputHeader);
      }
      if (HasFlushLastPacket) {
        size += 1 + 1;
      }
      if (HasJitter) {
        size += 1 + 8;
      }
      if (HasJitterWithReflection) {
        size += 1 + 1;
      }
      if (HasReproducibleSampling) {
        size += 1 + 1;
      }
      if (HasBaseTimestamp) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(BaseTimestamp);
      }
      if (HasStartTime) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(StartTime);
      }
      if (HasEndTime) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(EndTime);
      }
      if (HasRoundLimits) {
        size += 1 + 1;
      }
      if (HasUseInputFrameRate) {
        size += 1 + 1;
      }
      if (HasMaxFrameRate) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(PacketResamplerCalculatorOptions other) {
      if (other == null) {
        return;
      }
      if (other.HasFrameRate) {
        FrameRate = other.FrameRate;
      }
      if (other.HasOutputHeader) {
        OutputHeader = other.OutputHeader;
      }
      if (other.HasFlushLastPacket) {
        FlushLastPacket = other.FlushLastPacket;
      }
      if (other.HasJitter) {
        Jitter = other.Jitter;
      }
      if (other.HasJitterWithReflection) {
        JitterWithReflection = other.JitterWithReflection;
      }
      if (other.HasReproducibleSampling) {
        ReproducibleSampling = other.ReproducibleSampling;
      }
      if (other.HasBaseTimestamp) {
        BaseTimestamp = other.BaseTimestamp;
      }
      if (other.HasStartTime) {
        StartTime = other.StartTime;
      }
      if (other.HasEndTime) {
        EndTime = other.EndTime;
      }
      if (other.HasRoundLimits) {
        RoundLimits = other.RoundLimits;
      }
      if (other.HasUseInputFrameRate) {
        UseInputFrameRate = other.UseInputFrameRate;
      }
      if (other.HasMaxFrameRate) {
        MaxFrameRate = other.MaxFrameRate;
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
          case 9: {
            FrameRate = input.ReadDouble();
            break;
          }
          case 16: {
            OutputHeader = (global::Mediapipe.PacketResamplerCalculatorOptions.Types.OutputHeader) input.ReadEnum();
            break;
          }
          case 24: {
            FlushLastPacket = input.ReadBool();
            break;
          }
          case 33: {
            Jitter = input.ReadDouble();
            break;
          }
          case 40: {
            BaseTimestamp = input.ReadInt64();
            break;
          }
          case 48: {
            StartTime = input.ReadInt64();
            break;
          }
          case 56: {
            EndTime = input.ReadInt64();
            break;
          }
          case 64: {
            RoundLimits = input.ReadBool();
            break;
          }
          case 72: {
            JitterWithReflection = input.ReadBool();
            break;
          }
          case 80: {
            ReproducibleSampling = input.ReadBool();
            break;
          }
          case 88: {
            UseInputFrameRate = input.ReadBool();
            break;
          }
          case 97: {
            MaxFrameRate = input.ReadDouble();
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
          case 9: {
            FrameRate = input.ReadDouble();
            break;
          }
          case 16: {
            OutputHeader = (global::Mediapipe.PacketResamplerCalculatorOptions.Types.OutputHeader) input.ReadEnum();
            break;
          }
          case 24: {
            FlushLastPacket = input.ReadBool();
            break;
          }
          case 33: {
            Jitter = input.ReadDouble();
            break;
          }
          case 40: {
            BaseTimestamp = input.ReadInt64();
            break;
          }
          case 48: {
            StartTime = input.ReadInt64();
            break;
          }
          case 56: {
            EndTime = input.ReadInt64();
            break;
          }
          case 64: {
            RoundLimits = input.ReadBool();
            break;
          }
          case 72: {
            JitterWithReflection = input.ReadBool();
            break;
          }
          case 80: {
            ReproducibleSampling = input.ReadBool();
            break;
          }
          case 88: {
            UseInputFrameRate = input.ReadBool();
            break;
          }
          case 97: {
            MaxFrameRate = input.ReadDouble();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the PacketResamplerCalculatorOptions message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Types {
      public enum OutputHeader {
        /// <summary>
        /// Do not output a header, even if the input contained one.
        /// </summary>
        [pbr::OriginalName("NONE")] None = 0,
        /// <summary>
        /// Pass the header, if the input contained one.
        /// </summary>
        [pbr::OriginalName("PASS_HEADER")] PassHeader = 1,
        /// <summary>
        /// Update the frame rate in the header, which must be of type VideoHeader.
        /// </summary>
        [pbr::OriginalName("UPDATE_VIDEO_HEADER")] UpdateVideoHeader = 2,
      }

    }
    #endregion

    #region Extensions
    /// <summary>Container for extensions for other messages declared in the PacketResamplerCalculatorOptions message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Extensions {
      public static readonly pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.PacketResamplerCalculatorOptions> Ext =
        new pb::Extension<global::Mediapipe.CalculatorOptions, global::Mediapipe.PacketResamplerCalculatorOptions>(95743844, pb::FieldCodec.ForMessage(765950754, global::Mediapipe.PacketResamplerCalculatorOptions.Parser));
    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
