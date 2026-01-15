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

