$requestBody = Get-Content $req -Raw | ConvertFrom-Json;
$siteName = $requestBody.sitename

# Set Service Principal Credentials
# SP_PASSWORD, SP_USERNAME, TENANTID are app settings
$secpasswd = ConvertTo-SecureString $env:SP_PASSWORD -AsPlainText -Force;
$mycreds = New-Object System.Management.Automation.PSCredential ($env:SP_USERNAME, $secpasswd)
Add-AzureRmAccount -ServicePrincipal -Tenant $env:TENANTID -Credential $mycreds;
$context = Get-AzureRmContext;
Set-AzureRmContext -Context $context;

$resourceGroupName = $requestBody.resourcegroup;
$location = $requestBody.location;
$appServicePlan = $requestBody.appserviceplan;

# Get web app
$webapp = Get-AzureRmWebApp -ResourceGroupName $resourceGroupName -Name $siteName;
$appSettings = $webapp.SiteConfig.AppSettings;

#Get the current app settings
$settings = @{};
Write-output "Existing app settings...";
ForEach ($setting in $appSettings) {
  $settings[$setting.Name] = $setting.Value;
  $message = $setting.Name + ", " + $setting.Value;
  Write-Output $message;
}
 
#Add new setting
if($requestBody.appsettings)
{
    $appsettings = $requestBody.appsettings | Get-Member -MemberType NoteProperty;
    $appsettings.Name | ForEach-Object { 
        $key = $_; 
        $item = $requestBody.appsettings.$key; 
        $settings[$key] = $item;
    }
}

# Set the app settings with updated lists
$app = Set-AzureRMWebApp -Name $siteName -ResourceGroupName $resourceGroupName -AppSettings $settings;
Write-Output "Application settings applied to: $($app.Name)";

# Verify new app settings were added
$webapp = Get-AzureRmWebApp -ResourceGroupName $resourceGroupName -Name $siteName;
$appSettings = $webapp.SiteConfig.AppSettings;
Write-output "Updated app settings...";
ForEach ($setting in $appSettings) {
  $message = $setting.Name + ", " + $setting.Value;
  Write-Output $message;
}

Out-File -Encoding Ascii -FilePath $res -inputObject "Update app settings completed for site: $($app.Name)";

<#
Sample POST JSON body:

{
   "appsettings": {
	     "key1" : "value1",
	     "key2" : "value2",
	     "key3" : "value3"
   },
   "sitename": "site1",
   "resourcegroup": "RG1",
   "location": "West US",
   "appserviceplan" : "Default1"
}
#>