using System;
using System.Collections.Generic;

// ❌ BAD: High-level modules depend on low-level modules (violates DIP)
class BadBankingExample
{
    class EmailService
    {
        public void SendEmail(string recipient, string message)
        {
            Console.WriteLine($"Sending email to {recipient}: {message}");
        }
    }

    class SMSService
    {
        public void SendSMS(string phone, string message)
        {
            Console.WriteLine($"Sending SMS to {phone}: {message}");
        }
    }

    class TransactionProcessor
    {
        private EmailService emailService = new EmailService();
        private SMSService smsService = new SMSService();

        public void ProcessTransaction(decimal amount)
        {
            Console.WriteLine($"Processing transaction: ${amount}");
            emailService.SendEmail("user@bank.com", $"Transaction: ${amount}");
            smsService.SendSMS("555-1234", $"Amount debited: ${amount}");
        }
    }
}

// ✅ GOOD: Both depend on abstractions (follows DIP)
namespace GoodBankingExample
{
    // Abstraction: High-level module depends on this
    interface INotificationService
    {
        void SendNotification(string recipient, string message);
    }

    // Low-level modules implement the abstraction
    class EmailNotification : INotificationService
    {
        public void SendNotification(string recipient, string message)
        {
            Console.WriteLine($"📧 Email to {recipient}: {message}");
        }
    }

    class SMSNotification : INotificationService
    {
        public void SendNotification(string recipient, string message)
        {
            Console.WriteLine($"📱 SMS to {recipient}: {message}");
        }
    }

    class PushNotification : INotificationService
    {
        public void SendNotification(string recipient, string message)
        {
            Console.WriteLine($"🔔 Push to {recipient}: {message}");
        }
    }

    // High-level module depends on abstraction, not concrete implementations
    class TransactionProcessor
    {
        private readonly List<INotificationService> notificationServices;

        public TransactionProcessor(List<INotificationService> services)
        {
            notificationServices = services;
        }

        public void ProcessTransaction(string accountHolder, decimal amount)
        {
            Console.WriteLine($"\n=== Processing transaction for {accountHolder} ===");
            Console.WriteLine($"Amount: ${amount}");

            // Notify through all configured channels
            foreach (var service in notificationServices)
            {
                service.SendNotification(
                    accountHolder,
                    $"Transaction processed: ${amount} debited from your account"
                );
            }
        }
    }

    // Usage example
    class Program
    {
        static void Main()
        {
            var notificationServices = new List<INotificationService>
            {
                new EmailNotification(),
                new SMSNotification(),
                new PushNotification()
            };

            var processor = new TransactionProcessor(notificationServices);
            processor.ProcessTransaction("John Doe", 500);

            // Key benefit: Adding new notification type requires NO changes to TransactionProcessor
            Console.WriteLine("\n--- Adding new notification service ---");
            var newServices = new List<INotificationService>
            {
                new EmailNotification(),
                new WhatsAppNotification() // New service!
            };

            var processor2 = new TransactionProcessor(newServices);
            processor2.ProcessTransaction("Jane Smith", 1000);
        }
    }

    // New notification type - zero changes to existing code
    class WhatsAppNotification : INotificationService
    {
        public void SendNotification(string recipient, string message)
        {
            Console.WriteLine($"💬 WhatsApp to {recipient}: {message}");
        }
    }
}