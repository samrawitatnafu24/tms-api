using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TmsApi.Controlles
{
    // 1. Model Class
    public class EnrollmentRecord
    {
        public int Id { get; set; }
        // Add other database columns/properties here if you have any
    }

    // 2. Interface Definition
    public interface IEnrollmentService
    {
        Task<List<EnrollmentRecord>> GetAllAsync();
        Task<EnrollmentRecord> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<EnrollmentRecord> EnrollAsync(EnrollmentRecord record);
    }

    // 3. Service Implementation
    public class EnrollmentService : IEnrollmentService
    {
        public EnrollmentService()
        {
            // Inject your DbContext here when ready
        }

        public async Task<List<EnrollmentRecord>> GetAllAsync()
        {
            return await Task.FromResult(new List<EnrollmentRecord>());
        }

        public async Task<EnrollmentRecord> GetByIdAsync(int id)
        {
            var record = new EnrollmentRecord { Id = id };
            return await Task.FromResult(record);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult(true);
        }

        public async Task<EnrollmentRecord> EnrollAsync(EnrollmentRecord record)
        {
            return await Task.FromResult(record);
        }
    }
}
