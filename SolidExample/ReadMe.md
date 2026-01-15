# SOLID Principles - Complete Guide

## Overview of SOLID Principles

| Principle | Acronym | Focus Area | Purpose |
|-----------|---------|-----------|---------|
| Single Responsibility | **S** | Class design | Each class should have one reason to change |
| Open/Closed | **O** | Extension | Open for extension, closed for modification |
| Liskov Substitution | **L** | Inheritance | Subclasses must be substitutable for parent |
| Interface Segregation | **I** | Interfaces | Use specific interfaces, not generic ones |
| Dependency Inversion | **D** | Dependencies | Depend on abstractions, not concrete classes |

---

## Detailed Principle Breakdown

### Single Responsibility Principle (SRP)

| Aspect | Description |
|--------|-------------|
| **Definition** | A class should have only one reason to change |
| **Benefit** | Easier to understand, test, and maintain |
| **Anti-pattern** | God objects with multiple responsibilities |
| **Example** | Separate classes for data access, business logic, and UI |

### Open/Closed Principle (OCP)

| Aspect | Description |
|--------|-------------|
| **Definition** | Open for extension, closed for modification |
| **Benefit** | New features without changing existing code |
| **Anti-pattern** | Using if-else chains for new types |
| **Implementation** | Use inheritance, composition, and polymorphism |

### Liskov Substitution Principle (LSP)

| Aspect | Description |
|--------|-------------|
| **Definition** | Subtypes must be substitutable for parent types |
| **Benefit** | Reliable polymorphic code |
| **Anti-pattern** | Subclass breaks parent class contract |
| **Example** | Bank accounts with different withdrawal rules |

### Interface Segregation Principle (ISP)

| Aspect | Description |
|--------|-------------|
| **Definition** | Use specific interfaces, not one generic interface |
| **Benefit** | Classes implement only what they need |
| **Anti-pattern** | Fat interfaces with many unrelated methods |
| **Example** | Separate `IWithdrawable`, `ITransferable` instead of `IAccount` |

### Dependency Inversion Principle (DIP)

| Aspect | Description |
|--------|-------------|
| **Definition** | High-level modules depend on abstractions |
| **Benefit** | Loose coupling, easy testing and extension |
| **Anti-pattern** | Direct dependencies on concrete classes |
| **Implementation** | Use interfaces, dependency injection, factories |

---

## Key DIP Concepts Explained

| Concept | Bad Approach | Good Approach |
|---------|--------------|---------------|
| **Dependency** | High-level depends on low-level (concrete) | Both depend on abstractions |
| **Flexibility** | Hard to change/extend | Easy to add new implementations |
| **Coupling** | Tightly coupled | Loosely coupled |
| **Testing** | Hard to mock dependencies | Easy to inject test doubles |
| **Maintenance** | Changes ripple through code | Changes isolated to implementations |

---

## Real-World Industry Applications

| Industry | SOLID Application | Example |
|----------|------------------|---------|
| **Banking** | DIP for notification services | Email, SMS, Push notifications for transactions |
| **Banking** | LSP for account types | SavingsAccount, CheckingAccount, FixedDeposit |
| **Automobile** | DIP for engine types | GasolineEngine, DieselEngine, ElectricEngine |
| **Automobile** | OCP for new features | Adding new engine without modifying Car class |
| **E-commerce** | SRP for payment processing | PaymentProcessor, NotificationService, OrderService |
| **Healthcare** | ISP for patient records | Separate interfaces for billing, medical history, scheduling |

---

## File Structure

| File | Purpose | SOLID Principles Covered |
|------|---------|------------------------|
| `SRP.cs` | Single Responsibility | SRP |
| `OCP.cs` | Open/Closed | OCP |
| `LSP.cs` | Liskov Substitution | LSP |
| `ISP.cs` | Interface Segregation | ISP |
| `DIP.cs` | Dependency Inversion | DIP |
| `banking-dip-example.cs` | Banking industry DIP example | DIP, Real-world application |
| `automobile-dip-example.cs` | Automobile industry DIP example | DIP, Real-world application |

---

## Key Takeaways

| Point | Explanation |
|-------|-------------|
| **Cohesion** | SOLID principles increase cohesion within classes |
| **Coupling** | SOLID principles decrease coupling between classes |
| **Testability** | Code following SOLID is easier to unit test |
| **Maintainability** | Changes are localized and don't cascade |
| **Scalability** | New features can be added with minimal changes |
| **Readability** | Well-organized code is easier to understand |

---

## Open/Closed Principle (OCP) - Banking Example

### Overview
The Open/Closed Principle states that software entities should be **open for extension but closed for modification**. This means you should be able to add new functionality without changing existing code.

### File: [`ocp-baking-example.cs`](ocp-baking-example.cs)

#### Problem: Violating OCP
```csharp
class InterestCalculator
{
    public decimal CalculateInterest(string accountType, decimal balance)
    {
        if (accountType == "Savings")
            return balance * 0.04m;
        else if (accountType == "MoneyMarket")
            return balance * 0.05m;
        // ❌ Must modify this method for EVERY new account type!
    }
}
```

**Issues:**
- Must modify `InterestCalculator` for each new account type
- Risk of breaking existing functionality
- Violates the Single Responsibility Principle
- Tight coupling between calculator and account types

#### Solution: Following OCP
The solution uses **polymorphism** through the `IBankAccount` interface:

**Key Components:**

| Component | Purpose |
|-----------|---------|
| [`IBankAccount`](ocp-baking-example.cs) | Contract that all account types must follow |
| [`SavingsAccount`](ocp-baking-example.cs) | 4% interest, full withdrawal capability |
| [`MoneyMarketAccount`](ocp-baking-example.cs) | 5% interest, 95% withdrawal limit |
| [`FixedDepositAccount`](ocp-baking-example.cs) | 6% interest, no early withdrawals |
| [`HighYieldSavingsAccount`](ocp-baking-example.cs) | 7% interest, full withdrawal capability |
| [`InterestCalculator`](ocp-baking-example.cs) | Works with ANY account type - never modified |
| [`AccountOperationService`](ocp-baking-example.cs) | Handles withdrawal rules - closed for modification |

**How It Works:**
```csharp
// ✅ This method NEVER needs modification
public decimal CalculateTotalAnnualInterest(IEnumerable<IBankAccount> accounts)
{
    return accounts.Sum(account => account.CalculateAnnualInterest());
}
```

**Adding New Account Type (Zero Changes Required):**
```csharp
// Just implement IBankAccount - no changes to calculator!
public sealed class StudentSavingsAccount : IBankAccount
{
    public decimal CalculateAnnualInterest() => Balance * 0.08m;  // 8%
    // ... other implementations
}
```

#### Benefits

| Aspect | Impact |
|--------|--------|
| **Extensibility** | Add new account types without modifying existing code |
| **Maintainability** | Each account type is independent |
| **Testability** | Test each account type in isolation |
| **Robustness** | No risk of breaking existing functionality |
| **Code Reuse** | Calculator works with all account types automatically |

#### Running the Example
```csharp
BankingOCP.GoodBankingExample.Demo();
```

---

## Dependency Inversion Principle (DIP)

### Overview
The Dependency Inversion Principle states that:
1. **High-level modules should NOT depend on low-level modules**
2. **Both should depend on abstractions**
3. **Abstractions should NOT depend on details**
4. **Details should depend on abstractions**

### File 1: [`dip-banking-example.cs`](dip-banking-example.cs)

#### Problem: Violating DIP
```csharp
class TransactionProcessor
{
    private EmailService emailService = new EmailService();     // ❌ Direct dependency
    private SMSService smsService = new SMSService();           // ❌ Direct dependency
    
    // ❌ Tightly coupled to concrete implementations
    public void ProcessTransaction(decimal amount)
    {
        emailService.SendEmail("user@bank.com", $"Transaction: ${amount}");
        smsService.SendSMS("555-1234", $"Amount debited: ${amount}");
    }
}
```

**Issues:**
- `TransactionProcessor` is tightly coupled to `EmailService` and `SMSService`
- Cannot easily add new notification methods (WhatsApp, Telegram, etc.)
- Difficult to test (cannot mock dependencies)
- Changes in low-level modules force changes in high-level modules

#### Solution: Following DIP

**Abstraction First:**
```csharp
interface INotificationService
{
    void SendNotification(string recipient, string message);
}
```

**Low-Level Modules Implement Abstraction:**
- [`EmailNotification`](dip-banking-example.cs) - Sends via email
- [`SMSNotification`](dip-banking-example.cs) - Sends via SMS
- [`PushNotification`](dip-banking-example.cs) - Sends push notification
- [`WhatsAppNotification`](dip-banking-example.cs) - Sends via WhatsApp

**High-Level Module Depends on Abstraction:**
```csharp
class TransactionProcessor
{
    private readonly List<INotificationService> notificationServices;
    
    // ✅ Depends on abstraction, not concrete implementations
    public TransactionProcessor(List<INotificationService> services)
    {
        notificationServices = services;
    }
    
    public void ProcessTransaction(string accountHolder, decimal amount)
    {
        foreach (var service in notificationServices)
        {
            service.SendNotification(accountHolder, 
                $"Transaction processed: ${amount}");
        }
    }
}
```

**Usage:**
```csharp
var services = new List<INotificationService>
{
    new EmailNotification(),
    new SMSNotification(),
    new PushNotification()
};

var processor = new TransactionProcessor(services);
processor.ProcessTransaction("John Doe", 500);

// Adding new notification type - ZERO changes to TransactionProcessor!
var newServices = new List<INotificationService>
{
    new EmailNotification(),
    new WhatsAppNotification()  // New service added
};
```

#### Benefits in Banking

| Benefit | Description |
|---------|-------------|
| **Flexibility** | Swap notification methods without changing `TransactionProcessor` |
| **Testability** | Inject mock `INotificationService` for unit testing |
| **Maintainability** | Each notification type is independent |
| **Scalability** | Add new channels (SMS, Email, Push, Telegram, etc.) easily |
| **Decoupling** | High-level logic decoupled from low-level details |

#### Running the Example
```csharp
GoodBankingExample.Program.Main();
```

---

### File 2: [`dip-automobile-example.cs`](dip-automobile-example.cs)

#### Problem: Violating DIP
```csharp
class Car
{
    private GasolineEngine gasolineEngine;   // ❌ Direct dependency
    private DieselEngine dieselEngine;       // ❌ Direct dependency
    
    // ❌ Car depends on specific engine types
    public void Drive()
    {
        if (engineType == "gasoline")
        {
            gasolineEngine.Start();
            // ... drive ...
            gasolineEngine.Stop();
        }
    }
}
```

**Issues:**
- `Car` is tightly coupled to `GasolineEngine` and `DieselEngine`
- Cannot use electric or hybrid engines without modifying `Car`
- Different engines have different methods (`Start()`, `StartDiesel()`)
- Violates Liskov Substitution Principle

#### Solution: Following DIP

**Abstraction for Engines:**
```csharp
interface IEngine
{
    void Start();
    void Stop();
    string GetFuelType();
}
```

**Abstraction for Transmissions:**
```csharp
interface ITransmission
{
    void Shift(int gear);
}
```

**Engine Implementations:**
- [`GasolineEngine`](dip-automobile-example.cs) - Traditional fuel
- [`DieselEngine`](dip-automobile-example.cs) - Diesel fuel
- [`ElectricEngine`](dip-automobile-example.cs) - Battery powered
- [`HybridEngine`](dip-automobile-example.cs) - Dual mode
- [`HydrogenFuelCell`](dip-automobile-example.cs) - Future fuel

**Transmission Implementations:**
- [`AutomaticTransmission`](dip-automobile-example.cs) - Automatic shifting
- [`ManualTransmission`](dip-automobile-example.cs) - Manual control

**High-Level Module (Car) Depends on Abstractions:**
```csharp
class Car
{
    private readonly IEngine engine;           // ✅ Depends on abstraction
    private readonly ITransmission transmission; // ✅ Depends on abstraction
    
    public Car(string model, IEngine engine, ITransmission transmission)
    {
        this.model = model;
        this.engine = engine;
        this.transmission = transmission;
    }
    
    public void Drive()
    {
        engine.Start();
        transmission.Shift(1);
        // ... drive ...
        engine.Stop();
    }
}
```

**Factory Pattern for Easy Configuration:**
```csharp
class CarFactory
{
    public static Car CreateStandardCar()
        => new Car("Toyota Camry", new GasolineEngine(), new AutomaticTransmission());
    
    public static Car CreatePremiumCar()
        => new Car("Tesla Model S", new ElectricEngine(), new AutomaticTransmission());
}
```

**Adding New Engine Type (Zero Changes to Car):**
```csharp
// Create hydrogen-powered car without modifying Car class
var hydrogenCar = new Car(
    "Toyota Mirai",
    new HydrogenFuelCell(),      // New engine - works seamlessly!
    new AutomaticTransmission()
);
hydrogenCar.Drive();
```

#### Benefits in Automobile Industry

| Benefit | Description |
|---------|-------------|
| **Engine Agility** | Switch engine types (Gasoline → Electric → Hydrogen) without code changes |
| **Transmission Flexibility** | Support both automatic and manual transmissions |
| **Future-Proof** | New fuel types and transmission types can be added easily |
| **Modularity** | Each engine/transmission is independently testable |
| **Reusability** | Engine/transmission combinations can be mixed and matched |

#### Real-World Analogy
Just like a car manufacturer designs the **interface** between the engine and car (standard bolt patterns, connections), your code should design **abstractions** that components depend on, not concrete implementations.

#### Running the Example
```csharp
GoodAutomobileExample.Program.Main();
```

---

## Comparison: OCP vs DIP in Banking

| Aspect | OCP (Open/Closed) | DIP (Dependency Inversion) |
|--------|-------------------|---------------------------|
| **Focus** | Behavioral variation across types | Loose coupling between layers |
| **Example** | Different account interest rates | Notification service flexibility |
| **Mechanism** | Polymorphic interface methods | Constructor injection |
| **Problem Solved** | Adding new account types | Adding new notification channels |
| **Extension Point** | New class implementing interface | New service implementing interface |

---

## Key Takeaways

### OCP in Banking
- **Principle:** Open for new account types, closed for calculator modification
- **Result:** Add `StudentAccount`, `BusinessAccount`, etc. without touching `InterestCalculator`
- **Files:** [`ocp-baking-example.cs`](ocp-baking-example.cs)

### DIP in Banking
- **Principle:** Both `TransactionProcessor` and notification services depend on `INotificationService`
- **Result:** Swap email ↔ SMS ↔ Push ↔ WhatsApp without code changes
- **File:** [`dip-banking-example.cs`](dip-banking-example.cs)

### DIP in Automobiles
- **Principle:** `Car` depends on `IEngine` and `ITransmission` abstractions
- **Result:** Use Gasoline → Electric → Hydrogen engines interchangeably
- **File:** [`dip-automobile-example.cs`](dip-automobile-example.cs)

---

## Running All Examples

Execute in `Program.cs`:
```csharp
// OCP - Banking Example
BankingOCP.GoodBankingExample.Demo();

// DIP - Banking Example
GoodBankingExample.Program.Main();

// DIP - Automobile Example
GoodAutomobileExample.Program.Main();
```

---

