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

    // Set name to query string or body data
    string src = data?.src;
    string dst = data?.dst;
    string text = data?.text;
    string url = data?.url;

    RestAPI plivo = new RestAPI(authId, authToken);  

    var resp = plivo.send_message(new Dictionary<string, string>() 
    {
        { "src", src }, // Sender's phone number with country code
        { "dst", dst }, // Receiver's phone number wiht country code
        { "text", text }, // Your SMS text message
        { "url", url}, // The URL to which with the status of the message is sent
        { "method", "POST"} // Method to invoke the url
    });

    if(src == null || dst == null || text == null || url == null)
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a src, dst, text, and url in the request body");
    }

    return req.CreateResponse(HttpStatusCode.OK, resp.Content);
}