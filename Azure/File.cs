using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neudesic.Azure
{
    class File
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageAccount"].ConnectionString;
            string localFolder = ConfigurationManager.AppSettings["Source"];
            string Destination = ConfigurationManager.AppSettings["Destination"];
            Console.WriteLine(@"Connecting to storage account");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Console.WriteLine(@"Getting reference to container");
            CloudBlobContainer container = blobClient.GetContainerReference(Destination);
            string[] files = Directory.GetFiles(localFolder);
            foreach(string filePath in files)
            {
                string key = Path.GetFileName(filePath);
                UploadBlob(container, key, filePath, true);
                Console.WriteLine(@"upload process is complete.Press any key to exit");
                Console.ReadKey();
            }
        }
        static void UploadBlob(CloudBlobContainer container, string key, string fileName, bool deleteAfter)
        {
            Console.WriteLine(@"Uploading file to container : key=" + key + "source file" + fileName);
            CloudBlockBlob cloudBlob = container.GetBlockBlobReference(key);
            using (var file = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                cloudBlob.UploadFromStream(file);
            }
        }

    }
}
