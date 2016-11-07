$requestBody = Get-Content $req -Raw | ConvertFrom-Json

# Set Service Principal credentials
# SP_PASSWORD, SP_USERNAME, TENANTID are app settings
$secpasswd = ConvertTo-SecureString $env:SP_PASSWORD -AsPlainText -Force;
$mycreds = New-Object System.Management.Automation.PSCredential ($env:SP_USERNAME, $secpasswd)
Add-AzureRmAccount -ServicePrincipal -Tenant $env:TENANTID -Credential $mycreds;
$context = Get-AzureRmContext;
Set-AzureRmContext -Context $context;

$storage = Get-AzureRmStorageAccount -StorageAccountName $requestBody.storageaccount -ResourceGroupName $requestBody.resourcegroup;
if ($storage -ne $null){
    echo "Storage";
    $storage | Out-String;
}
else {
    echo "Empty";
}

<#
Sample GET JSON body:

{
    "storageaccount": "hutohant10store",
    "resourcegroup": "Default-Storage-WestUS"
}
#>
