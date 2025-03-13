using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CCTP_Manage.Models.Entities;
using CCTP_Manage.Models.Configuration;

public class EmailReminderService : BackgroundService
{
    private readonly IServiceProvider service;
    private readonly EmailSettings email;

    public EmailReminderService(IServiceProvider serviceProvider, IOptions<EmailSettings> emailSettings)
    {
        service = serviceProvider;
        email = emailSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = service.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<SaloneventosdbContext>();

                    var today = DateOnly.FromDateTime(DateTime.Now);

                    // Obtén las reservaciones para el siguiente día
                    var upcomingReservations = await dbContext.Reservaciones
                        .Include(r => r.Usuario)
                        .Where(r => r.FechaReservacion == today.AddDays(1))
                        .ToListAsync();

                    foreach (var reservation in upcomingReservations)
                    {
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress("Tu Salón de Eventos", email.Username));
                        message.To.Add(new MailboxAddress(reservation.Usuario.Nombre, reservation.Usuario.Correo));
                        message.Subject = "Recordatorio de reservación";
                        message.Body = new TextPart("plain")
                        {
                            Text = $"Hola {reservation.Usuario.Nombre},\n\n" +
                                   $"Te recordamos que tienes una reservación el {reservation.FechaReservacion} a las {reservation.HoraInicio}.\n\n" +
                                   "¡Gracias por confiar en nosotros!\n\nSaludos,\nEquipo de Salón de Eventos"
                        };

                        using (var client = new SmtpClient())
                        {
                            await client.ConnectAsync(email.SmtpServer, email.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls, stoppingToken);
                            await client.AuthenticateAsync(email.Username, email.Password, stoppingToken);
                            await client.SendAsync(message, stoppingToken);
                            await client.DisconnectAsync(true, stoppingToken);
                        }
                    }
                }
            }
            catch
            {
                // Error silenciado intencionalmente (no registrar ni mostrar)
            }

            // Espera 24 horas antes de volver a ejecutar el servicio
            await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
        }
    }
}
