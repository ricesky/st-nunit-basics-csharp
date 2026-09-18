namespace StudentWallet.Tests;

[TestFixture]
public class StudentWalletTests
{
    /*
    private StudentWallet.Domain.StudentWallet _wallet = null!;

    [SetUp]
    public void SetUp()
    {
        _wallet = new StudentWallet.Domain.StudentWallet("Alya", 100_000m);
    }
    */

    [Test]
    public void Constructor_WithValidData_CreatesWallet()
    {
        // Arrange dan Act
        var wallet = new StudentWallet.Domain.StudentWallet("Alya", 50_000m);

        // Assert
        Assert.That(wallet.OwnerName, Is.EqualTo("Alya"));
        Assert.That(wallet.Balance, Is.EqualTo(50_000m));
    }

    [Test]
    public void TopUp_WithPositiveAmount_IncreasesBalance()
    {
        // Arrange
        var wallet = new StudentWallet.Domain.StudentWallet("Alya", 50_000m);

        // Act
        wallet.TopUp(25_000m);

        // Assert
        Assert.That(wallet.Balance, Is.EqualTo(75_000m));
    }

    [Test]
    public void Pay_WithSufficientBalance_DecreasesBalance()
    {
        // Arrange
        var wallet = new StudentWallet.Domain.StudentWallet("Alya", 100_000m);

        // Act
        wallet.Pay(35_000m);

        // Assert
        Assert.That(wallet.Balance, Is.EqualTo(65_000m));
    }

    /*
    [Test]
    public void TopUp_WithZeroAmount_ThrowsArgumentOutOfRangeException()
    {
        // Act
        TestDelegate action = () => _wallet.TopUp(0m);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Test]
    public void Pay_WithInsufficientBalance_KeepsOriginalBalance()
    {
        // Act
        Assert.Throws<InvalidOperationException>(
            () => _wallet.Pay(150_000m));

        // Assert
        Assert.That(_wallet.Balance, Is.EqualTo(100_000m));
    }
    */
}