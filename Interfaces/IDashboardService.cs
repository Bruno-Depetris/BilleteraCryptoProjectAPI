public interface IDashboardService
{
    Task<DashboardSummaryDTO?> GetByClienteIdAsync(int clienteId);
}


