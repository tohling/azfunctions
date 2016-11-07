$requestBody = Get-Content $req -Raw | ConvertFrom-Json

# Set Service Principal credentials
# SP_PASSWORD, SP_USERNAME, TENANTID are app settings
$secpasswd = ConvertTo-SecureString $env:SP_PASSWORD -AsPlainText -Force;
$mycreds = New-Object System.Management.Automation.PSCredential ($env:SP_USERNAME, $secpasswd)
Add-AzureRmAccount -ServicePrincipal -Tenant $env:TENANTID -Credential $mycreds;
$context = Get-AzureRmContext;
Set-AzureRmContext -Context $context;

# Create web app
New-AzureRmWebApp -ResourceGroupName $requestBody.resourcegroup -Name $requestBody.sitename -Location $requestBody.location -AppServicePlan $requestBody.appserviceplan | Out-String;

<#
Sample POST JSON body:

{
   "sitename": "hutoh-site",
   "resourcegroup": "Default-Web-WestUS",
   "location": "West US",
   "appserviceplan" : "Default1"
}
#>