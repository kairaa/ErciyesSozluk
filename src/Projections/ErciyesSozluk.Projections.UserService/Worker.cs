using ErciyesSozluk.Common.BlazorSozluk.Common;
using ErciyesSozluk.Common.Events.User;
using ErciyesSozluk.Common.Infrastructure;
using ErciyesSozluk.Projections.UserService.Services;

namespace ErciyesSozluk.Projections.UserService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Services.UserService userService;
        private readonly EmailService emailService;

        public Worker(ILogger<Worker> logger, UserService.Services.UserService userService, EmailService emailService)
        {
            _logger = logger;
            this.userService = userService;
            this.emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.UserExchangeName)
                .EnsureQueue(SozlukConstants.UserEmailChangedQueueName, SozlukConstants.UserExchangeName)
                .Receive<UserEmailChangedEvent>(user =>
                {
                    // DB Insert 

                    var confirmationId = userService.CreateEmailConfirmation(user).GetAwaiter().GetResult();

                    // Generate Link

                    var link = emailService.GenerateConfirmationLink(confirmationId);

                    // Send Email

                    emailService.SendEmail(user.NewEmailAddress, link).GetAwaiter().GetResult();
                })
                .StartConsuming(SozlukConstants.UserEmailChangedQueueName);
        }
    }
}