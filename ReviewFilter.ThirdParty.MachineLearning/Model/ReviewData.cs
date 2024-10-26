using Microsoft.ML.Data;

namespace ReviewFilter.ThirdParty.MachineLearning.Model
{
    public class ReviewData
    {
        [LoadColumn(1)]
        public string ReviewText { get; set; }

        [LoadColumn(2)] // Assuming IsFake is in the third column (index 2)
        public string Label { get; set; }
    }

    public class ReviewTransformed
    {
        public string ReviewText { get; set; }
        public bool Label { get; set; }
    }

    public class ReviewPrediction
    {
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
