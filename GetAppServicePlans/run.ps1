$requestBody = Get-Content $req -Raw | ConvertFrom-Json

# Set Service Principal credentials
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
   "resourcegroup": "Default-Web-WestUS",
   "resourcename" : "Default1",
   "resourcetype": "Microsoft.Web/serverfarms"
}
#>