$requestBody = Get-Content $req -Raw | ConvertFrom-Json
$name = $requestBody.name

if ($req_query_name) 
{
    $name = $req_query_name 
}

Out-File -Encoding Ascii -FilePath $res -inputObject "Hello $name"

$secpasswd = ConvertTo-SecureString $env:SP_PASSWORD -AsPlainText -Force;
$mycreds = New-Object System.Management.Automation.PSCredential ($env:SP_USERNAME, $secpasswd)
Add-AzureRmAccount -ServicePrincipal -Tenant $env:TENANTID -Credential $mycreds;

$context = Get-AzureRmContext;
$resourceGroupName = "Default-Storage-WestUS";
$storageAccountName = "hutohant10store";
Set-AzureRmContext -Context $context;
$storage = Get-AzureRmStorageAccount -StorageAccountName $storageAccountName -ResourceGroupName $resourceGroupName;
if ($storage -ne $null){
    echo "Storage";
    $storage | Out-String;
}
else {
    echo "Empty";
}

