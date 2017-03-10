using System;
using System.Net;
using System.Collections.Generic;
using System.Reflection;
using RestSharp;
using Plivo.API;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Get auth
    string authId = Environment.GetEnvironmentVariable("PLIVO_AUTH_ID"); // or hard-code AUTHID
    string authToken = Environment.GetEnvironmentVariable("PLIVO_AUTH_TOKEN"); // or hard-code AUTHTOKEN

    // Get request body. TODO: Add error handling for invalid request body payload
    string src = data?.src;
    string dst = data?.dst;
    string text = data?.text;
    string url = data?.url;

    RestAPI plivo = new RestAPI(authId, authToken);  

    var resp = plivo.send_message(new Dictionary<string, string>() 
    {
        { "src", src }, // Sender's phone number with country code
        { "dst", dst }, // Receiver's phone number with country code
        { "text", text }, // Your SMS text message
        { "url", url}, // The URL to which with the status of the message is sent
        { "method", "POST"} // Method to invoke the url
    });

    return req.CreateResponse(HttpStatusCode.OK, resp.Content);
}

/*
To test, use the following,

HTTP Method: POST

Sample request body:

{
    "src": "+1222222222",
    "dst": "+1333333333",
    "text": "Hi, this is a text from Azure Functions",
    "url": "https://myfunction.azurewebsites.net/api/PlivoSendSms"
}
*/