using Microsoft.AspNetCore.Http;
using stutor_core.Configurations;
using stutor_core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Services
{
    public class AWSS3Service
    {
        private readonly AWSS3Repository _repo;

        public AWSS3Service(AWSS3Settings config)
        {
            _repo = new AWSS3Repository(config);
        }

        public async Task<bool> UploadMultipleToBucketAsync(IFormCollection files)
        {
            return await _repo.UploadMultipleToBucketAsync(files);
        }
    }
}
