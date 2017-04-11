using System.Net;
using System.Diagnostics;
using System.Text;

private static ProcessModuleInfo GetProcessModuleInfo(ProcessModule module, bool details = false)
{
    var moduleInfo = new ProcessModuleInfo
    {
        BaseAddress = module.BaseAddress.ToInt64().ToString("x"),
        FileName = Path.GetFileName(module.FileName),
        FileVersion = module.FileVersionInfo.FileVersion,
    };

    if(details)
    {
        moduleInfo.FilePath = module.FileName;
        moduleInfo.ModuleMemorySize = module.ModuleMemorySize;
        moduleInfo.FileDescription = module.FileVersionInfo.FileDescription;
        moduleInfo.Product = module.FileVersionInfo.ProductName;
        moduleInfo.ProductVersion = module.FileVersionInfo.ProductVersion;
        moduleInfo.IsDebug = module.FileVersionInfo.IsDebug;
        moduleInfo.Language = module.FileVersionInfo.Language;
    }

    return moduleInfo;
}

public class ProcessModuleInfo
{
    string Name { get { return BaseAddress; } }

    public string BaseAddress { get; set; }

    public string FileName { get; set; }

    public string FilePath { get; set; }

    public int ModuleMemorySize { get; set; }

    public string FileVersion { get; set; }

    public string FileDescription { get; set; }

    public string Product { get; set; }

    public string ProductVersion { get; set; }

    public bool? IsDebug { get; set; }

    public string Language { get; set; }
}

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");
    bool runtimeNativeDllIsLoaded = false;
    const string runtimeNativeDll = "Microsoft.Azure.WebJobs.Script.WebHost.ni.dll";
    StringBuilder builder = new StringBuilder();

    try
    {

        var modules = new List<ProcessModuleInfo>();
        foreach (var module in Process.GetCurrentProcess().Modules.Cast<ProcessModule>().OrderBy(m => Path.GetFileName(m.FileName)))
        {
            modules.Add(GetProcessModuleInfo(module, details: true));
        }

        foreach (var module in modules)
        {
            // builder.AppendLine(module.FileName);
            
            if(string.Equals(module.FileName, runtimeNativeDll, StringComparison.OrdinalIgnoreCase))
            {
                runtimeNativeDllIsLoaded = true;
                break;                
            }
        }
    }
    catch (Exception ex)
    {
        log.Info(ex.ToString());
    }

    // return req.CreateResponse(HttpStatusCode.OK, builder.ToString());
    return req.CreateResponse(HttpStatusCode.OK, runtimeNativeDllIsLoaded);
}