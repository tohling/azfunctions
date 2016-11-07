#r "AddressLookup.dll"

using System;
using AddressLookup;
using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // Reading environment variable from App Settings, replace with hardcoded value if not using App settings
    string apiKey = System.Environment.GetEnvironmentVariable("GoogleMapsAPIKey", EnvironmentVariableTarget.Process);

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();
    string address = data?.address;
    string name = data?.name;

    HttpResponseMessage message = null;
    if (string.IsNullOrEmpty(name))
    {
        message = req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a 'name' in the request body");
    }
    else if (string.IsNullOrEmpty(address))
    {
        message = req.CreateResponse(HttpStatusCode.BadRequest, "Please pass an 'address' in the request body");
    }
    else
    {
        GeoCoder geoCoder = new GeoCoder(apiKey);
        GeoLocation loc = geoCoder.GetGeoLocation(address);
        string formattedAddress = geoCoder.GetAddress(loc);

        var msg = $"Hello {name}. Lon: '{loc.Longitude}', Lat: '{loc.Latitude}', Formatted address: '{formattedAddress}'"; 
        message = req.CreateResponse(HttpStatusCode.OK, msg);
    }
    
    return message;
}