using Microsoft.ML;
using ReviewFilter.ThirdParty.MachineLearning.Model;

namespace ReviewFilter.ThirdParty.MachineLearning
{
    public class MachineLearningService : IMachineLearningService
    {
        PredictionEngine<ReviewData, ReviewPrediction> PredictionEngine { get; set; }

        public MachineLearningService(string filePath)
        {
            MLContext mlContext = new MLContext();
            Action<ReviewData, ReviewTransformed> mapping = (input, output) =>
            {
                output.ReviewText = input.ReviewText;
                output.Label = input.Label == "OR"; // Maps "OR" to true and "CG" to false
            };
          
            IDataView dataView = mlContext.Data.LoadFromTextFile<ReviewData>(
                path: filePath,
                hasHeader: true,
                separatorChar: ',');


            // Split data into train and test sets
            var splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = splitData.TrainSet;
            var testData = splitData.TestSet;

            // Build the pipeline with the custom mapping
            var pipeline = mlContext.Transforms.CustomMapping(mapping, contractName: null)
                .Append(mlContext.Transforms.CopyColumns("Label", nameof(ReviewTransformed.Label)))
                .Append(mlContext.Transforms.Text.FeaturizeText("Features", nameof(ReviewTransformed.ReviewText)))
                .Append(mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "Label", featureColumnName: "Features"));

            var trainedModel = pipeline.Fit(trainData);
            PredictionEngine = mlContext.Model.CreatePredictionEngine<ReviewData, ReviewPrediction>(trainedModel);
         
        }

        public string Analize(string  reviewText)
        {
            var newReview = new ReviewData { ReviewText = reviewText };
            var prediction = PredictionEngine.Predict(newReview);
            return prediction.PredictedLabel ? "OR" : "CG";
        }
    }
}
