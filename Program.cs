using Microsoft.Extensions.Hosting;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage;

const string AccountName = "--Storage Account Name Here--";
const string AccountKey = "--Storage Account Key Here--";
const string ContainerName = "--Container Name Here--";
const string BlobName = "--Blob Resource Name Here--";
const string ConnectionString = "--Connection String Here--";

BlobContainerClient blobContainerClient = new BlobContainerClient(ConnectionString,
    ContainerName);

BlobClient blobClient = blobContainerClient.GetBlobClient(BlobName);

Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
{
    BlobContainerName = ContainerName,
    BlobName = "filename.xlsx",
    ExpiresOn = DateTime.UtcNow.AddDays(1),//Let SAS token expire after 1 Day;
};
blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);//User will only be able to read the blob and its properties
var sasToken = blobSasBuilder.ToSasQueryParameters(new
StorageSharedKeyCredential(AccountName, AccountKey)).ToString();
var sasURL = $"{blobClient.Uri.AbsoluteUri}?{sasToken}"; //Generate URL

Console.WriteLine(sasURL);

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
