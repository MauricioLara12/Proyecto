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
            using (var scope = _serviceProvider.CreateScope())
            {
                var reservacionesController = scope.ServiceProvider.GetRequiredService<ReservacionesController>();
                reservacionesController.VerificarReservacion();
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Ejecuta cada hora
        }
    }
}
