using System;

public class ATM{
    private long atmCash;
    private bool isServerDown = false;
    private const int MaxPinAttempts = 3;
    private readonly long dailyLimit = 1000;

    public ATM(long cash){
        atmCash = cash;
    }

    public void GetUserDetail(){
        if (isServerDown)
        {
            throw new ServerDownException("Server is currently down: Unable to connect.");
        }
        Console.WriteLine("User details retrieved successfully.");
    }

    public bool AuthenticatePin(UserAccount user, int enteredPin){
        if (user.IsCardBlocked)
            throw new PinLimitExceedException("Card is blocked due to multiple invalid PIN attempts.");

        if (user.Pin == enteredPin){
            user.ResetPinAttempts();
            Console.WriteLine("PIN authenticated successfully.");
            return true;
        }
        else{
            user.IncrementPinAttempts();
            if (user.WrongPinCount >= MaxPinAttempts){
                user.BlockCard();
                throw new PinLimitExceedException("Card is now blocked after 3 invalid attempts.");
            }
            throw new Exception($"Incorrect PIN. Attempts left: {MaxPinAttempts - user.WrongPinCount}");
        }
    }

    public void Withdraw(UserAccount user, long amount){
        if (amount > atmCash)
            throw new InsufficientCashException("ATM does not have enough cash.");

        if (amount > user.GetBalance())
            throw new InsufficientCashException("Insufficient balance in account.");

        if (user.DailyWithdrawn + amount > dailyLimit)
            throw new DailyLimitExceedException("Daily withdrawal limit exceeded.");

        user.Debit(amount);
        atmCash -= amount;
        Console.WriteLine($"Withdrawal successful. Withdrawn: {amount}. Remaining Balance: {user.GetBalance()}");
    }
}

public class UserAccount{
    public long AccountNumber { get; }
    private long balance;
    public int Pin { get; }
    public int WrongPinCount { get; private set; } = 0;
    public bool IsCardBlocked { get; private set; } = false;
    public long DailyWithdrawn { get; private set; } = 0;

    public UserAccount(long accountNumber, long initialBalance, int pin)
    {
        AccountNumber = accountNumber;
        balance = initialBalance;
        Pin = pin;
    }

    public long GetBalance() => balance;

    public void Debit(long amount)
    {
        balance -= amount;
        DailyWithdrawn += amount;
    }

    public void IncrementPinAttempts() => WrongPinCount++;
    public void ResetPinAttempts() => WrongPinCount = 0;
    public void BlockCard() => IsCardBlocked = true;
}

#region Custom Exceptions
    public class InsufficientCashException : Exception
    {
        public InsufficientCashException(string message) : base(message) { }
    }

    public class ServerDownException : Exception
    {
        public ServerDownException(string message) : base(message) { }
    }

    public class PinLimitExceedException : Exception
    {
        public PinLimitExceedException(string message) : base(message) { }
    }

    public class DailyLimitExceedException : Exception
    {
        public DailyLimitExceedException(string message) : base(message) { }
    }
#endregion

public class Program{
    public static void Main(){
        var user = new UserAccount(accountNumber: 6461, initialBalance: 2000, pin: 4321);
        var atm = new ATM(cash: 5000);

        try{
            atm.GetUserDetail();

            Console.Write("Enter PIN: ");
            int enteredPin = int.Parse(Console.ReadLine());
            atm.AuthenticatePin(user, enteredPin);

            Console.Write("Enter amount to withdraw: ");
            long amount = long.Parse(Console.ReadLine());
            atm.Withdraw(user, amount);
        }
        catch (Exception ex){
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }
}