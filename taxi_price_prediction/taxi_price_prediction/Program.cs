using System;
using System.Text;
using Microsoft.ML;

namespace taxi_price_prediction
{
    class Program
    {
        static void Main(string[] args)
        {   //ml ortamı
            var mlContext = new MLContext(); 
            
            //öğrenme ve test verileri çekme
            var trainDataWiew = mlContext.Data.LoadFromTextFile<model.taxiFare>("data/taxi-fare-train.csv", hasHeader: true, separatorChar: ',');
            var testDataWiew = mlContext.Data.LoadFromTextFile<model.taxiFare>("data/taxi-fare-test.csv", hasHeader: true, separatorChar: ',');

            //pipeline oluşturulması
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount")
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "VendorId", outputColumnName: "VendorIdEncoded"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "RateCode", outputColumnName: "RateCodeEncoded"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "PaymentType", outputColumnName: "PaymentTypeEncoded"))
                    .Append(mlContext.Transforms.Concatenate("Features", new string[] { "VendorIdEncoded", "RateCodeEncoded", "PaymentTypeEncoded", "PassengerCount", "TripTime", "TripDistance" }))
                    .Append(mlContext.Regression.Trainers.LbfgsPoissonRegression());//kullanılan ml algoritması(LbfgsPoissonRegression)

            //verinin pipeline a fit edilmesi
            var model = pipeline.Fit(trainDataWiew);

            //test için örnek 
            PredictionEngine<model.taxiFare, model.taxiFareOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<model.taxiFare, model.taxiFareOutput>(model, trainDataWiew.Schema);
            var predictionResult = predictionEngine.Predict(new model.taxiFare() {
                PassengerCount = 3,
                TripDistance = 100,
                TripTime=600,
                PaymentType="CRD",
                RateCode="1",
                VendorId="VIS",
            });
        Console.WriteLine("amount:"+predictionResult.FareAmount);
            Console.ReadLine();
        }
    }
}
