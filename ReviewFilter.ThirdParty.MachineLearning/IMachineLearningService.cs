using Microsoft.ML;
using ReviewFilter.ThirdParty.MachineLearning.Model;

namespace ReviewFilter.ThirdParty.MachineLearning
{
    public interface IMachineLearningService
    {
        string Analize(string reviewText);
    }
}
