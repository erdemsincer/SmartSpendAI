namespace AIClassifierService.Core.Interfaces;

public interface IReceiptClassifier
{
    string PredictCategory(string merchantName, decimal totalAmount);
}
