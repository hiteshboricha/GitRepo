using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using SEG.Azure.Entities;
using SEG.Azure.Business;

namespace SEG.Azure.WebJobStorage
{
    public class Functions
    {


        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("employeequeue")] string message,
            TextWriter log)
        {
            CloudStorageAccount storageAccount =
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient;
            CloudBlobContainer container;

            blobClient = storageAccount.CreateCloudBlobClient();

            container = blobClient.GetContainerReference("hiteshborichacontainer");
            container.CreateIfNotExists();

            BlobContainerPermissions permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;

            container.SetPermissions(permissions);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("blockblob");

            Upload(blockBlob);
            //Dowload(blockBlob);

            log.WriteLine(message);
        }

        public static void Upload(CloudBlockBlob blockBlob)
        {
            List<Employee> employeelist = new List<Employee>();
            EmployeeBAL employeebal = new EmployeeBAL();

            List<Employee> employeegenericlist = employeebal.GetEmployees(0);
            Employee firstemployee = employeegenericlist[1];

            Stream stream = new MemoryStream();
            stream.Position = 0;

            try
            {
                stream.Seek(0, SeekOrigin.Begin);

                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(stream, firstemployee);
                stream.Position = 0;
                blockBlob.UploadFromStream(stream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        public static void Dowload(CloudBlockBlob blockBlob)
        {
            Stream stream = new MemoryStream();
            stream.Position = 0;

            try
            {
                stream.Seek(0, SeekOrigin.Begin);

                blockBlob.DownloadToStream(stream);
                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                BinaryFormatter j = new BinaryFormatter();
                Employee emp = (Employee)j.Deserialize(stream);
                string temp = emp.Address;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }
    }
}
