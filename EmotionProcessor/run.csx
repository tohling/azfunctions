#r "Microsoft.WindowsAzure.Storage"
#r "System.Web"
#r "System.Runtime"
#r "System.Threading.Tasks"
#r "System.IO"
#r "Newtonsoft.Json"

using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web.Configuration;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;

public static async Task Run(ICloudBlob myBlob, IAsyncCollector<object> outputDocument, TraceWriter log)
{
    log.Info($"C# Blob trigger function processed: {myBlob}");
    
    var apiKey = WebConfigurationManager.AppSettings["EMOTION_API_KEY"];

    EmotionServiceClient emotionServiceClient = new EmotionServiceClient(apiKey);
    Emotion[] emotions = await emotionServiceClient.RecognizeAsync(myBlob.Uri.ToString());

    var photo = new PhotoResult
    {
        Uri = myBlob.Uri.ToString(),
        Name = myBlob.Name,
        NoMatches = emotions.Length,
        ProcessTime = DateTime.UtcNow,
        Results = emotions
    };

    await outputDocument.AddAsync(photo);
} 

public class PhotoResult
{
    public string Uri;
    public string Name;
    public int NoMatches;
    public DateTime ProcessTime;
    public Emotion[] Results;
}