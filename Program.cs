using Microsoft.Extensions.Hosting;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage;

const string AccountName = "ssastoken";
const string AccountKey = "AllkL3Rdak9SS5wcnBRWpb/3oU/X6yKQlQCxEn0YKd4Xa4+TClonODjW2cCQbV6Gq8+1dctShffF+AStalF/Sw==";
const string ContainerName = "images";
const string BlobName = "705536322_ResumeAcquisitionForm1.png";
const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ssastoken;AccountKey=AllkL3Rdak9SS5wcnBRWpb/3oU/X6yKQlQCxEn0YKd4Xa4+TClonODjW2cCQbV6Gq8+1dctShffF+AStalF/Sw==;EndpointSuffix=core.windows.net";

BlobContainerClient blobContainerClient = new BlobContainerClient(ConnectionString,
    ContainerName);

BlobClient blobClient = blobContainerClient.GetBlobClient(BlobName);

Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
{
    BlobContainerName = ContainerName,
    BlobName = "filename.xlsx",
    ExpiresOn = DateTime.UtcNow.AddDays(1),//Let SAS token expire after 1 Day;
};
blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);//User will only be able to read the blob and it's properties
var sasToken = blobSasBuilder.ToSasQueryParameters(new
StorageSharedKeyCredential(AccountName, AccountKey)).ToString();
var sasURL = $"{blobClient.Uri.AbsoluteUri}?{sasToken}";

Console.WriteLine(sasURL);

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
