using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace stutor_core.Repositories
{
    public class GoogleCloudRepository
    {
        private static StorageClient _storageClient { get; set; }
        private readonly static string _projectId = "stutor-291218";

        public GoogleCloudRepository()
        {
            var path = Assembly.GetCallingAssembly().Location;
            string sharedkeyFilePath = path.Substring(0, path.LastIndexOf("\\")) + @"\" + "Stutor-google-cloud-services.dev.json";

            GoogleCredential credential = null;
            try
            {
                using (var jsonStream = new FileStream(sharedkeyFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }

                _storageClient = StorageClient.Create(credential);
            }
            catch (Exception ex)
            {
                Log.Error("Could not read google-cloud-service config settings filestream from {path}", sharedkeyFilePath);
                throw ex;
            }
            
        }

        public async Task UploadToBucketAsync(IFormCollection files)
        {
            try
            {
                string bucketName = _projectId + "-test-bucket";
                foreach (var file in files.Files)
                {
                    var stream = file.OpenReadStream();
                    await _storageClient.UploadObjectAsync(bucketName, file.FileName, "text/plain", stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static void UploadToBucket(string bucketName, string localPath, string objectName = null)
        //{
        //    var storage = StorageClient.Create();
        //    using (var f = File.OpenRead(localPath))
        //    {
        //        objectName = objectName ?? Path.GetFileName(localPath);
        //        storage.UploadObject(bucketName, objectName, null, f);
        //        Console.WriteLine($"Uploaded {objectName}.");
        //    }
        //}

        //Not used but shows how
        private void CreateGoogleCloudBucket()
        {
            string bucketName = _projectId + "-test-bucket";
            // The name for the new bucket.
            try
            {
                // Creates the new bucket.
                _storageClient.CreateBucket(_projectId, bucketName);
                Console.WriteLine($"Bucket {bucketName} created.");
            }
            catch (Google.GoogleApiException e)
            when (e.Error.Code == 409)
            {
                // The bucket already exists.  That's fine.
                Console.WriteLine(e.Error.Message);
            }
        }

    }
}
