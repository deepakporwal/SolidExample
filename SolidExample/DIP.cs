using System;
using System.Collections.Generic;

namespace SolidExample
{
    // Abstraction that both high-level and low-level modules depend on.
    public interface IMessageSender
    {
        void Send(string message);
    }

    // Low-level implementation: Email sender.
    public class EmailSender : IMessageSender
    {
        private readonly string _fromAddress;

        public EmailSender(string fromAddress)
        {
            _fromAddress = fromAddress ?? throw new ArgumentNullException(nameof(fromAddress));
        }

        public void Send(string message)
        {
            // Simulated email sending logic.
            Console.WriteLine($"[Email] From: {_fromAddress} | Message: {message}");
        }
    }

    // Low-level implementation: SMS sender.
    public class SmsSender : IMessageSender
    {
        private readonly string _phoneNumber;

        public SmsSender(string phoneNumber)
        {
            _phoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        }

        public void Send(string message)
        {
            // Simulated SMS sending logic.
            Console.WriteLine($"[SMS] To: {_phoneNumber} | Message: {message}");
        }
    }

    // Test-friendly/mock implementation to show how DIP improves testability.
    public class MockSender : IMessageSender
    {
        public List<string> SentMessages { get; } = new();

        public void Send(string message)
        {
            // Store messages instead of sending them.
            SentMessages.Add(message);
            Console.WriteLine($"[Mock] Stored message: {message}");
        }
    }

    // High-level module that performs notifications but depends only on the abstraction.
    public class NotificationService
    {
        private readonly IMessageSender _sender;

        // Dependency injected via constructor (inversion of control).
        public NotificationService(IMessageSender sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        public void Notify(string message)
        {
            // Business logic remains independent of how message is sent.
            _sender.Send(message);
        }
    }

    // Composition root and demonstration entry for this example.
    public static class DIP
    {
        // Call `DIP.RunDemo()` from your Main method or tests to see the Principle in action.
        public static void RunDemo()
        {
            Console.WriteLine("DIP Demo: Using EmailSender");
            IMessageSender emailSender = new EmailSender("no-reply@example.com");
            var emailNotification = new NotificationService(emailSender);
            emailNotification.Notify("Welcome! This is an email notification.");

            Console.WriteLine();

            Console.WriteLine("DIP Demo: Swapping to SmsSender at runtime");
            IMessageSender smsSender = new SmsSender("+15551234567");
            var smsNotification = new NotificationService(smsSender);
            smsNotification.Notify("Welcome! This is an SMS notification.");

            Console.WriteLine();

            Console.WriteLine("DIP Demo: Using MockSender for testing");
            var mock = new MockSender();
            var testNotification = new NotificationService(mock);
            testNotification.Notify("Test message 1");
            testNotification.Notify("Test message 2");

            Console.WriteLine($"Mock stored {mock.SentMessages.Count} messages.");
        }
    }
}
