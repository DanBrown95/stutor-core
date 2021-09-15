using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using stutor_core.Configurations;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3.Model;

namespace stutor_core.Repositories
{
    public class AWSS3Repository
    {
        private static string _bucketName { get; set; }

        private static AmazonS3Client _s3Client { get; set; }

        public AWSS3Repository(AWSS3Settings config)
        {
            _bucketName = config.BucketName;

            var awsRegion = RegionEndpoint.GetBySystemName(config.Region);
            _s3Client = new AmazonS3Client(awsRegion);
        }

        public async Task<bool> UploadMultipleToBucketAsync(IFormCollection files)
        {
            try
            {
                foreach (var file in files.Files)
                {
                    var guid = Guid.NewGuid();
                    var filename = file.FileName + "-" + guid;
                 
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        PutObjectRequest request = new PutObjectRequest()
                        {
                            InputStream = memoryStream,
                            BucketName = _bucketName + "/expert-applications",
                            Key = filename
                        };
                        request.Headers["ContentDisposition"] = "attachment; filename = "+filename;
                        //request.Metadata.Add("Content-Disposition", "attachment; filename="+filename);

                        // Upload the file if less than 2 MB
                        if (memoryStream.Length < 2097152)
                        {
                            PutObjectResponse response2 = await _s3Client.PutObjectAsync(request);
                            //var fileTransferUtility = new TransferUtility(_s3Client);
                            //await fileTransferUtility.UploadAsync(memoryStream, _bucketName+"/expert-applications", filename);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Could not add files to aws bucket: {bucket}. Error: {error}", _bucketName+"/expert-applications", ex);
                throw ex;
            }
        }


    }
}
