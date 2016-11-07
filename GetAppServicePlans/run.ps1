$requestBody = Get-Content $req -Raw | ConvertFrom-Json

# Set Service Principal credentials
# SP_PASSWORD, SP_USERNAME, TENANTID are app settings
$secpasswd = ConvertTo-SecureString $env:SP_PASSWORD -AsPlainText -Force;
$mycreds = New-Object System.Management.Automation.PSCredential ($env:SP_USERNAME, $secpasswd)
Add-AzureRmAccount -ServicePrincipal -Tenant $env:TENANTID -Credential $mycreds;
$context = Get-AzureRmContext;
Set-AzureRmContext -Context $context;

# Get app service plans
Find-AzureRmResource -ResourceType $requestBody.resourcetype -ResourceGroupNameContains $requestBody.resourcegroup -ResourceNameContains $requestBody.resourcename | Out-String;

<#
Sample GET JSON body:

{
   "resourcegroup": "RG1",
   "resourcename" : "Default1",
   "resourcetype": "Microsoft.Web/serverfarms"
}
#>