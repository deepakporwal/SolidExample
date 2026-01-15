using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingOCP
{
    // ❌ BAD: Violates OCP - must modify calculator for each new account type
    class BadBankingExample
    {
        class InterestCalculator
        {
            public decimal CalculateInterest(string accountType, decimal balance)
            {
                // ❌ Closed for extension - must modify this method for new account types
                if (accountType == "Savings")
                    return balance * 0.04m;  // 4% interest
                else if (accountType == "MoneyMarket")
                    return balance * 0.05m;  // 5% interest
                else if (accountType == "FixedDeposit")
                    return balance * 0.06m;  // 6% interest
                // Every new account type requires modification here!
                return 0;
            }
        }
    }

    // ✅ GOOD: Follows OCP - open for extension, closed for modification
    namespace GoodBankingExample
    {
        // Abstraction: Contract for all account types
        public interface IBankAccount
        {
            string AccountHolder { get; }
            decimal Balance { get; }
            string AccountType { get; }
            
            // Behavior that can vary by account type
            decimal CalculateAnnualInterest();
            bool CanWithdraw(decimal amount);
            decimal GetWithdrawalLimit();
        }

        // Concrete implementations - can be extended without changing calculator
        public sealed class SavingsAccount : IBankAccount
        {
            public string AccountHolder { get; }
            public decimal Balance { get; private set; }
            public string AccountType => "Savings";

            public SavingsAccount(string holder, decimal balance)
            {
                AccountHolder = holder;
                Balance = balance;
            }

            public decimal CalculateAnnualInterest() => Balance * 0.04m;  // 4%
            public bool CanWithdraw(decimal amount) => amount <= Balance;
            public decimal GetWithdrawalLimit() => Balance;

            public override string ToString() => $"{AccountType} | {AccountHolder} | Balance: ${Balance:F2}";
        }

        public sealed class MoneyMarketAccount : IBankAccount
        {
            public string AccountHolder { get; }
            public decimal Balance { get; private set; }
            public string AccountType => "Money Market";

            public MoneyMarketAccount(string holder, decimal balance)
            {
                AccountHolder = holder;
                Balance = balance;
            }

            public decimal CalculateAnnualInterest() => Balance * 0.05m;  // 5%
            public bool CanWithdraw(decimal amount) => amount <= (Balance * 0.95m);  // Must keep 5%
            public decimal GetWithdrawalLimit() => Balance * 0.95m;

            public override string ToString() => $"{AccountType} | {AccountHolder} | Balance: ${Balance:F2}";
        }

        public sealed class FixedDepositAccount : IBankAccount
        {
            public string AccountHolder { get; }
            public decimal Balance { get; private set; }
            public string AccountType => "Fixed Deposit";
            public int LockInMonths { get; }

            public FixedDepositAccount(string holder, decimal balance, int months)
            {
                AccountHolder = holder;
                Balance = balance;
                LockInMonths = months;
            }

            public decimal CalculateAnnualInterest() => Balance * 0.06m;  // 6%
            public bool CanWithdraw(decimal amount) => false;  // Cannot withdraw early
            public decimal GetWithdrawalLimit() => 0;

            public override string ToString() => 
                $"{AccountType} | {AccountHolder} | Balance: ${Balance:F2} | Locked for {LockInMonths} months";
        }

        // ✅ NEW ACCOUNT TYPE - NO CHANGES TO CALCULATOR NEEDED!
        public sealed class HighYieldSavingsAccount : IBankAccount
        {
            public string AccountHolder { get; }
            public decimal Balance { get; private set; }
            public string AccountType => "High Yield Savings";

            public HighYieldSavingsAccount(string holder, decimal balance)
            {
                AccountHolder = holder;
                Balance = balance;
            }

            public decimal CalculateAnnualInterest() => Balance * 0.07m;  // 7%
            public bool CanWithdraw(decimal amount) => amount <= Balance;
            public decimal GetWithdrawalLimit() => Balance;

            public override string ToString() => $"{AccountType} | {AccountHolder} | Balance: ${Balance:F2}";
        }

        // Calculator is CLOSED for modification - works with any IBankAccount
        public class InterestCalculator
        {
            // ✅ This method never needs modification - works with any account type
            public decimal CalculateTotalAnnualInterest(IEnumerable<IBankAccount> accounts)
            {
                return accounts.Sum(account => account.CalculateAnnualInterest());
            }

            public void PrintInterestReport(IEnumerable<IBankAccount> accounts)
            {
                Console.WriteLine("╔════════════════════════════════════════════════╗");
                Console.WriteLine("║     ANNUAL INTEREST CALCULATION REPORT         ║");
                Console.WriteLine("╚════════════════════════════════════════════════╝\n");

                decimal totalInterest = 0;

                foreach (var account in accounts)
                {
                    decimal interest = account.CalculateAnnualInterest();
                    totalInterest += interest;
                    Console.WriteLine($"  {account}");
                    Console.WriteLine($"  → Annual Interest: ${interest:F2}\n");
                }

                Console.WriteLine("╔════════════════════════════════════════════════╗");
                Console.WriteLine($"║  TOTAL ANNUAL INTEREST: ${totalInterest:F2,-37}║");
                Console.WriteLine("╚════════════════════════════════════════════════╝\n");
            }
        }

        // Account operations service - also closed for modification
        public class AccountOperationService
        {
            public bool AttemptWithdrawal(IBankAccount account, decimal amount)
            {
                if (!account.CanWithdraw(amount))
                {
                    Console.WriteLine($"  ❌ Cannot withdraw ${amount:F2} from {account.AccountType}");
                    Console.WriteLine($"     Withdrawal Limit: ${account.GetWithdrawalLimit():F2}\n");
                    return false;
                }

                Console.WriteLine($"  ✅ Successfully withdrew ${amount:F2}\n");
                return true;
            }
        }

        public static void Demo()
        {
            Console.WriteLine("═══════════════════════════════════════════════════════\n");
            Console.WriteLine("    OPEN/CLOSED PRINCIPLE IN BANKING\n");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            // Create various account types
            var accounts = new List<IBankAccount>
            {
                new SavingsAccount("Alice Johnson", 5000),
                new MoneyMarketAccount("Bob Smith", 10000),
                new FixedDepositAccount("Carol White", 20000, 12),
                new HighYieldSavingsAccount("David Brown", 15000)  // New type - no changes needed!
            };

            var calculator = new InterestCalculator();
            calculator.PrintInterestReport(accounts);

            // Test withdrawal rules - also works with new account type
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("                 WITHDRAWAL TESTS\n");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");

            var operationService = new AccountOperationService();

            operationService.AttemptWithdrawal(accounts[0], 2000);  // Savings - allowed
            operationService.AttemptWithdrawal(accounts[1], 9600);  // MoneyMarket - needs 5% buffer
            operationService.AttemptWithdrawal(accounts[2], 100);   // FixedDeposit - locked
            operationService.AttemptWithdrawal(accounts[3], 5000);  // HighYield - allowed
        }
    }
}