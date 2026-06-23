using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public partial record EnrollmentRecord;

public class EnrollmentWorker
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EnrollmentWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void ProcessBatch()
    {
        using var scope = _scopeFactory.CreateScope();

        var enrollmentService = scope.ServiceProvider.GetRequiredService<IEnrollmentService>();

        var records = enrollmentService.GetAllAsync().GetAwaiter().GetResult();
        Console.WriteLine($"[Worker] Processed batch containing {records.Count} enrollment records.");
    }
}