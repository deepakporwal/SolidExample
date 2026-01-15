abstract class BankAccount
{
    public virtual decimal Withdraw(decimal amount)
    {
        // Base implementation
        return amount;
    }
}

class SavingsAccount : BankAccount
{
    private decimal balance = 1000;
    
    public override decimal Withdraw(decimal amount)
    {
        if (amount > balance)
            throw new InvalidOperationException("Insufficient funds"); // ❌ Breaks contract
        balance -= amount;
        return amount;
    }
}

class FixedDepositAccount : BankAccount
{
    public override decimal Withdraw(decimal amount)
    {
        throw new NotSupportedException("Cannot withdraw from fixed deposit"); // ❌ Violates LSP
    }
}
```

**The gotcha:** Code expecting a `BankAccount` will crash if it gets a `FixedDepositAccount`, violating LSP.

## The Solution (Following LSP)

````csharp
abstract class BankAccount
{
    public abstract decimal GetBalance();
}

abstract class WithdrawableAccount : BankAccount
{
    public virtual decimal Withdraw(decimal amount)
    {
        if (amount > GetBalance())
            throw new InvalidOperationException("Insufficient funds");
        return amount;
    }
}

class SavingsAccount : WithdrawableAccount
{
    private decimal balance = 1000;
    public override decimal GetBalance() => balance;
    
    public override decimal Withdraw(decimal amount)
    {
        balance -= amount;
        return amount;
    }
}

class FixedDepositAccount : BankAccount
{
    private decimal balance = 5000;
    public override decimal GetBalance() => balance;
    // No Withdraw method - honest about capabilities
}

///
/// Key insight: Subclasses can be safely substituted for their 
/// parent without unexpected behavior. Non-withdrawable accounts
///  simply don't inherit from WithdrawableAccount.
/// 
/// Key insight: Subclasses can be safely substituted 
/// for their parent without unexpected behavior. Non-withdrawable 
/// accounts simply don't inherit from WithdrawableAccount.
/// 