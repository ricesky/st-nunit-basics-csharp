namespace StudentWallet.Domain;

public class StudentWallet
{
    public string OwnerName { get; }
    public decimal Balance { get; private set; }

    public StudentWallet(string ownerName, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(ownerName))
        {
            throw new ArgumentException(
                "Owner name must not be empty.",
                nameof(ownerName));
        }

        if (initialBalance < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(initialBalance),
                "Initial balance must not be negative.");
        }

        OwnerName = ownerName;
        Balance = initialBalance;
    }

    public void TopUp(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Top-up amount must be greater than zero.");
        }

        Balance += amount;
    }

    public void Pay(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Payment amount must be greater than zero.");
        }

        if (amount > Balance)
        {
            throw new InvalidOperationException("Insufficient balance.");
        }

        Balance -= amount;
    }
}