// TODO:
// THIS class needs the following binary to work: "adder_model_single_input_2x3.bytes"
// You can download it via this Google Drive link:
// https://drive.google.com/file/d/1vVDn80eZClBDJ8dJUk3szb9rLaE9p60q/view?usp=sharing
// You need to copy the file to 
// Packages\com.github.homuler.mediapipe\Runtime\Resources

// Issues:
// - Make sure TfliteConverterCalculator is compiled along with the plugin


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity
{
  /// <summary>
  /// Toy example on how to use tflite model for matrix classification. 
  /// In contrast to other mediapipe graphs, this graph does not expect an image as input data, 
  /// but deals with matrix input data. Internally the input (MatrixData) will be converted
  /// into an Eigen::MatrixXf which is then fed to the first calculator node of the graph 
  /// (TfLiteConverterCalculator). From there the input is converted to a tflite tensor, forwarded 
  /// throught the neural network (tflite model). The output of the neural network is converted into
  /// a std::vector<float> and and finally passed back into Unity as a List<float>
  /// 
  /// Example: This code could be used for a pose based action classifier.
  /// Input: Matrix containing values of an input pose 
  /// Output: Vector of action probabilities
  /// 
  /// Since matrix can only store 2D input data, but tflite models generally expect 4D input (NHWC), 
  /// the trick is to collapse the exceeding dimensions such that tflite can parse the 4D data from a 
  /// 2D matrix. Since the input matrix (Eigen::MatrixXf) is stored in column major order, the input 2D
  /// matrix has to be transposed befored feeding it into the graph.
  /// 
  /// Example: matrix of dimension [num_frames = 10, num_landmarks = 33, num_coordinates = 3]
  /// - collapse into matrix of shape [10 x 99]
  /// - now transpose to make "coordinate" dimension "fastest" -> output shape is now [99x10]
  /// - feed the data into the graph
  /// 
  /// TODO: 
  /// In theory mediapipe can also handle matrices stored in row-major order 
  /// -> write code for row-major matrix input to mediapipe graph
  /// </summary>
  public class MatrixClassification : MonoBehaviour
  {

    private readonly string TAG = "MatrixClassificationToyExample";

    private void OnEnable()
    {
      var _ = StartCoroutine(Init());
    }

    private void Start()
    {
      Debug.Log("Setup Protobuf Logging");
      Protobuf.SetLogHandler(Protobuf.DefaultLogHandler);

      Debug.Log("Start");
      var configText = @"
        input_stream: ""MATRIX:in""
        output_stream: ""FLOATS:out""

        node 
        {
          calculator: ""TfLiteConverterCalculator""
          input_stream: ""MATRIX:in""
          output_stream: ""TENSORS:image_tensor""
          options: 
          {
              [mediapipe.TfLiteConverterCalculatorOptions.ext]
              {
              zero_center: false
              }
          }
        }

        node 
        {
          calculator: ""TfLiteInferenceCalculator""
          input_stream: ""TENSORS:image_tensor""
          output_stream: ""TENSORS:tensor_features""
          options: 
          {
            [mediapipe.TfLiteInferenceCalculatorOptions.ext] 
            {
              model_path: ""mediapipe/models/adder_model_single_input_2x3.tflite""
            }
          }
        }

        node 
        {
          calculator: ""TfLiteTensorsToFloatsCalculator""
          input_stream: ""TENSORS:tensor_features""
          output_stream: ""FLOATS:out""
        }
      ";
      var graph = new CalculatorGraph(configText);

      // Specify expected output of tflite model
      var poller = graph.AddOutputStreamPoller<List<float>>("out").Value();

      Debug.Log("StartRun");
      graph.StartRun().AssertOk();
      for (var i = 0; i < 10; i++)
      {
        var matrix = CreateInputData();

        // feed data into graph
        var input = new MatrixPacket(matrix, new Timestamp(i));
        graph.AddPacketToInputStream("in", input).AssertOk();
      }
      graph.CloseInputStream("in").AssertOk();

      Debug.Log("Poll output");
      // Create output container with suitable size
      //  -> size should correspond to tflite model output size
      var outputFloatVector = new List<float>(new float[6] { 10, 11, 12, 13, 14, 15 });
      var output = new FloatVectorPacket(outputFloatVector);

      while (poller.Next(output))
      {
        var result = output.Get();
        foreach (var item in result)
        {
          Debug.Log("result array: " + item);
        }
      }


      graph.WaitUntilDone().AssertOk();
      graph.Dispose();

      Debug.Log("Done");
    }

    private static MatrixData CreateInputData()
    {
      var matrix = new MatrixData();
      matrix.PackedData.Add(0);
      matrix.PackedData.Add(1);
      matrix.PackedData.Add(2);
      matrix.PackedData.Add(3);
      matrix.PackedData.Add(4);
      matrix.PackedData.Add(5);

      matrix.Rows = 2;
      matrix.Cols = 3;
      return matrix;
    }

    private void OnApplicationQuit()
    {
      Protobuf.ResetLogHandler();
    }

    // Load tflite model from assets
    private IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
          WaitForAsset("adder_model_single_input_2x3.bytes"),
        };
    }

    private WaitForResult WaitForAsset(string assetName, bool overwrite = false)
    {
      return WaitForAsset(assetName, assetName, overwrite);
    }

    private WaitForResult WaitForAsset(string assetName, string uniqueKey, bool overwrite = false)
    {
      return new WaitForResult(this, AssetLoader.PrepareAssetAsync(assetName, uniqueKey, overwrite));
    }

    private IEnumerator Init()
    {
      Logger.LogInfo(TAG, "Loading dependent assets...");
      var assetRequests = RequestDependentAssets();
      yield return new WaitWhile(() => assetRequests.Any((request) => request.keepWaiting));

      var errors = assetRequests.Where((request) => request.isError).Select((request) => request.error).ToList();
      if (errors.Count > 0)
      {
        foreach (var error in errors)
        {
          Logger.LogError(TAG, error);
        }
        throw new InternalException("Failed to prepare dependent assets");
      }
    }

  }
}



