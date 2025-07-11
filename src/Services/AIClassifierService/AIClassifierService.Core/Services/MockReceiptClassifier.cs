using AIClassifierService.Core.Interfaces;

namespace AIClassifierService.Core.Services;

public class MockReceiptClassifier : IReceiptClassifier
{
    public string PredictCategory(string merchantName, decimal totalAmount)
    {
        if (merchantName.ToLower().Contains("market"))
            return "Market";
        if (merchantName.ToLower().Contains("pharmacy"))
            return "Sağlık";
        if (merchantName.ToLower().Contains("burger") || merchantName.ToLower().Contains("cafe"))
            return "Yeme-İçme";

        return totalAmount > 1000 ? "Elektronik" : "Genel";
    }
}
