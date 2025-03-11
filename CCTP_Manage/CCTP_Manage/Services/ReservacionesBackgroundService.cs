using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ReservacionesBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ReservacionesBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var reservacionesController = scope.ServiceProvider.GetRequiredService<ReservacionesController>();
                    reservacionesController.VerificarReservacion(); // Llama al método
                }
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Ejecuta cada hora
            }
            catch (Exception ex)
            {
                // Manejar errores (logearlos, etc.)
                Console.WriteLine($"Error en ReservacionesBackgroundService: {ex.Message}");
            }
        }
    }
}
