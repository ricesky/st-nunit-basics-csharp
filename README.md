![.NET](https://img.shields.io/badge/.NET-10.0-blue)
![C%23](https://img.shields.io/badge/C%23-14-purple)
![NUnit](https://img.shields.io/badge/NUnit-4.x-green)

# Pengenalan Unit Testing dengan NUnit

Tugas ini memperkenalkan **unit test** menggunakan **C#**, **.NET 10**, dan **NUnit**. Mahasiswa terlebih dahulu mengikuti satu tutorial, kemudian mengerjakan lima soal dengan tema domain berbeda. Mahasiswa diminta untuk membaca kode dan membuat unit test yang sesuai spesifikasi.

---

## Capaian Pembelajaran

Setelah menyelesaikan tutorial dan latihan, mahasiswa mampu:

1. menjelaskan perbedaan kode produksi dan kode unit test;
2. membuat *test project* NUnit dan menghubungkannya dengan project produksi;
3. menggunakan `[TestFixture]`, `[SetUp]`, `[Test]`, `[TestCase]`, dan `[TestCaseSource]`;
4. menyusun unit test menggunakan pola **Arrange–Act–Assert**;
5. menggunakan `Assert.That(...)` dan constraint dasar NUnit;
6. memeriksa exception menggunakan `Assert.Throws<TException>(...)`;
7. menggunakan `Assert.Multiple(...)` untuk memeriksa beberapa keadaan dari satu hasil; dan
8. menjalankan serta membaca hasil pengujian melalui Visual Studio maupun `dotnet test`.

---

## Lingkungan Pengembangan

- **IDE:** Visual Studio 2026
- **SDK:** .NET 10
- **Bahasa:** C# 14
- **Test framework:** NUnit 4.x
- **Test runner:** Microsoft.NET.Test.Sdk dan NUnit3TestAdapter

Perintah utama:

```bash
dotnet restore
dotnet build
dotnet test
```

Struktur solution:

```text
UnitTestingBasics/
├── UnitTestingBasics.sln
├── src/
│   ├── CampusWallet.Domain/
│   ├── Cafeteria.Domain/
│   ├── CourseRegistration.Domain/
│   ├── LibraryLoan.Domain/
│   ├── Parking.Domain/
│   └── MobileData.Domain/
└── tests/
    ├── CampusWallet.Domain.Tests/
    ├── Cafeteria.Domain.Tests/
    ├── CourseRegistration.Domain.Tests/
    ├── LibraryLoan.Domain.Tests/
    ├── Parking.Domain.Tests/
    └── MobileData.Domain.Tests/
```

Ketentuan umum:

- Folder `src/` berisi kode produksi dan tidak boleh diubah.
- Folder `tests/` digunakan untuk menulis unit test.
- Setiap *test project* harus memiliki *project reference* ke project produksi yang diuji.
- Semua test harus dapat dijalankan menggunakan `dotnet test`.

---

## Tutorial 1 — Menguji `CampusWallet` dengan NUnit

### 1. Deskripsi Studi Kasus

`CampusWallet` adalah dompet digital sederhana milik mahasiswa. Dompet menyimpan nama pemilik dan saldo. Mahasiswa dapat melakukan *top up* dan pembayaran selama data yang diberikan valid.

Aturan bisnis:

1. Nama pemilik wajib diisi.
2. Saldo awal tidak boleh negatif.
3. Nominal *top up* dan pembayaran harus lebih besar dari nol.
4. Pembayaran hanya dapat dilakukan jika saldo mencukupi.
5. *Top up* menambah saldo dan pembayaran mengurangi saldo.
6. Operasi yang ditolak tidak boleh mengubah saldo.

### 2. Membaca Kode Produksi

Buka `src/CampusWallet.Domain/CampusWallet.cs`.

```csharp
namespace CampusWallet.Domain;

public class CampusWallet
{
    public string OwnerName { get; }
    public decimal Balance { get; private set; }

    public CampusWallet(string ownerName, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(ownerName))
            throw new ArgumentException("Owner name must not be empty.", nameof(ownerName));
        if (initialBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(initialBalance));

        OwnerName = ownerName;
        Balance = initialBalance;
    }

    public void TopUp(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        Balance += amount;
    }

    public void Pay(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
    }
}
```

Sebelum membuat test, kenali state objek, operasi yang mengubah state, input setiap operasi, serta kondisi yang menyebabkan exception.

### 3. Membuat Solution dan Project Produksi

1. Buka **Visual Studio 2026**.
2. Pilih **Create a new project → Blank Solution**.
3. Beri nama solution `UnitTestingBasics`.
4. Klik kanan solution, lalu pilih **Add → New Project**.
5. Pilih **Class Library** untuk C#.
6. Beri nama `CampusWallet.Domain` dan letakkan di folder `src`.
7. Pilih **.NET 10.0**.
8. Hapus `Class1.cs`, tambahkan `CampusWallet.cs`, lalu salin kode produksi.

### 4. Membuat NUnit Test Project

1. Klik kanan solution, lalu pilih **Add → New Project**.
2. Pilih **NUnit Test Project** untuk C#.
3. Beri nama `CampusWallet.Domain.Tests` dan letakkan di folder `tests`.
4. Pilih **.NET 10.0**.
5. Hapus file test bawaan dan tambahkan `CampusWalletTests.cs`.

Template NUnit umumnya memasang `Microsoft.NET.Test.Sdk`, `NUnit`, `NUnit3TestAdapter`, dan `NUnit.Analyzers`.

### 5. Menambahkan Project Reference

1. Klik kanan `CampusWallet.Domain.Tests`.
2. Pilih **Add → Project Reference**.
3. Centang `CampusWallet.Domain`, lalu klik **OK**.

### 6. Membuat Test Fixture

```csharp
using CampusWallet.Domain;
using NUnit.Framework;

namespace CampusWallet.Domain.Tests;

[TestFixture]
public class CampusWalletTests
{
}
```

`[TestFixture]` menandai class yang berisi sekumpulan test.

### 7. Menulis Test Pertama dengan `[Test]`

```csharp
[Test]
public void Constructor_WithValidData_CreatesWallet()
{
    // Arrange dan Act
    var wallet = new CampusWallet("Alya", 50_000m);

    // Assert
    Assert.That(wallet.OwnerName, Is.EqualTo("Alya"));
    Assert.That(wallet.Balance, Is.EqualTo(50_000m));
}
```

- **Arrange** menyiapkan objek dan data.
- **Act** menjalankan perilaku yang diuji.
- **Assert** memeriksa hasil aktual.
- `Is.EqualTo(...)` memeriksa kesamaan nilai.
- Huruf `m` menandai nilai literal bertipe `decimal`.

Gunakan pola nama:

```text
MethodUnderTest_Scenario_ExpectedBehavior
```

### 8. Menggunakan `Assert.Multiple`

```csharp
[Test]
public void Constructor_WithValidData_CreatesWallet()
{
    var wallet = new CampusWallet("Alya", 50_000m);

    Assert.Multiple(() =>
    {
        Assert.That(wallet.OwnerName, Is.EqualTo("Alya"));
        Assert.That(wallet.Balance, Is.EqualTo(50_000m));
    });
}
```

`Assert.Multiple` memungkinkan NUnit menjalankan beberapa assertion yang memeriksa satu hasil atau keadaan yang sama.

### 9. Menggunakan `[SetUp]`

```csharp
private CampusWallet _wallet = null!;

[SetUp]
public void SetUp()
{
    _wallet = new CampusWallet("Alya", 100_000m);
}
```

`[SetUp]` dijalankan sebelum **setiap** test. Setiap test memperoleh objek baru sehingga perubahan pada satu test tidak memengaruhi test lain.

### 10. Menguji Perubahan State

```csharp
[Test]
public void TopUp_WithPositiveAmount_IncreasesBalance()
{
    // Act
    _wallet.TopUp(25_000m);

    // Assert
    Assert.That(_wallet.Balance, Is.EqualTo(125_000m));
}

[Test]
public void Pay_WithSufficientBalance_DecreasesBalance()
{
    // Act
    _wallet.Pay(35_000m);

    // Assert
    Assert.That(_wallet.Balance, Is.EqualTo(65_000m));
}
```

### 11. Menguji Exception dengan `Assert.Throws`

```csharp
[Test]
public void Pay_WithInsufficientBalance_ThrowsInvalidOperationException()
{
    TestDelegate action = () => _wallet.Pay(150_000m);

    Assert.Throws<InvalidOperationException>(action);
}
```

Bentuk ringkas:

```csharp
Assert.Throws<InvalidOperationException>(() => _wallet.Pay(150_000m));
```

Periksa juga state setelah operasi ditolak:

```csharp
[Test]
public void Pay_WithInsufficientBalance_KeepsOriginalBalance()
{
    Assert.Throws<InvalidOperationException>(() => _wallet.Pay(150_000m));

    Assert.That(_wallet.Balance, Is.EqualTo(100_000m));
}
```

### 12. Menggunakan `[TestCase]`

```csharp
[TestCase(10_000, 110_000)]
[TestCase(25_000, 125_000)]
[TestCase(100_000, 200_000)]
public void TopUp_WithPositiveAmount_IncreasesBalance(
    decimal amount,
    decimal expectedBalance)
{
    _wallet.TopUp(amount);

    Assert.That(_wallet.Balance, Is.EqualTo(expectedBalance));
}
```

Setiap `[TestCase]` menghasilkan satu test terpisah.

### 13. Menggunakan `[TestCaseSource]`

```csharp
private static IEnumerable<TestCaseData> InvalidPaymentCases()
{
    yield return new TestCaseData(0m)
        .SetName("Pay_ZeroAmount_ThrowsArgumentOutOfRangeException");
    yield return new TestCaseData(-10_000m)
        .SetName("Pay_NegativeAmount_ThrowsArgumentOutOfRangeException");
}

[TestCaseSource(nameof(InvalidPaymentCases))]
public void Pay_WithInvalidAmount_ThrowsArgumentOutOfRangeException(decimal amount)
{
    Assert.Throws<ArgumentOutOfRangeException>(() => _wallet.Pay(amount));
}
```

Gunakan `[TestCaseSource]` ketika data lebih kompleks, lebih panjang, atau perlu diberi nama khusus.

### 14. Menjalankan Test

Melalui Visual Studio, buka **Test → Test Explorer**, lalu pilih **Run All Tests**.

Melalui terminal:

```bash
dotnet test
```

Untuk menjalankan test tertentu:

```bash
dotnet test --filter "Name~Pay_WithInsufficientBalance"
```

Gunakan pola dan fitur NUnit yang telah dipelajari untuk mengerjakan lima soal berikut.

---

## Soal 1 — Pemesanan Menu Kantin

### Deskripsi Domain dan Aturan Bisnis

`CafeteriaOrder` merepresentasikan satu pesanan makanan di kantin kampus.

1. Nama menu wajib diisi.
2. Harga satuan dan jumlah awal harus lebih besar dari nol.
3. `AddQuantity` hanya menerima nilai lebih besar dari nol.
4. `GetTotal` menghasilkan harga satuan dikalikan jumlah.
5. Pesanan hanya dapat dibayar satu kali.
6. Setelah dibayar, jumlah pesanan tidak dapat diubah.

### Kode Produksi

```csharp
namespace Cafeteria.Domain;

public class CafeteriaOrder
{
    public string MenuName { get; }
    public decimal UnitPrice { get; }
    public int Quantity { get; private set; }
    public bool IsPaid { get; private set; }

    public CafeteriaOrder(string menuName, decimal unitPrice, int quantity)
    {
        if (string.IsNullOrWhiteSpace(menuName))
            throw new ArgumentException("Menu name is required.", nameof(menuName));
        if (unitPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        MenuName = menuName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public void AddQuantity(int quantity)
    {
        if (IsPaid)
            throw new InvalidOperationException("Paid order cannot be changed.");
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));
        Quantity += quantity;
    }

    public decimal GetTotal() => UnitPrice * Quantity;

    public void Pay()
    {
        if (IsPaid)
            throw new InvalidOperationException("Order has already been paid.");
        IsPaid = true;
    }
}
```

### Tugas Unit Test

Buat `CafeteriaOrderTests` untuk memeriksa:

1. constructor menghasilkan pesanan dengan data yang benar;
2. `AddQuantity` menambah jumlah pesanan;
3. nilai tambahan nol dan negatif ditolak menggunakan `[TestCase]`;
4. `GetTotal` menghasilkan total yang benar;
5. `Pay` mengubah `IsPaid` menjadi `true`;
6. pembayaran kedua melempar `InvalidOperationException`; dan
7. jumlah tidak berubah ketika perubahan setelah pembayaran ditolak.

Gunakan `[TestFixture]`, `[SetUp]`, `[Test]`, `[TestCase]`, `Assert.That`, `Assert.Multiple`, dan `Assert.Throws`.

---

## Soal 2 — Pendaftaran Mata Kuliah

### Deskripsi Domain dan Aturan Bisnis

`CourseRegistration` merepresentasikan pendaftaran mahasiswa ke mata kuliah.

1. Kode mata kuliah wajib diisi dan kapasitas harus lebih besar dari nol.
2. NRP wajib diisi dan tidak boleh didaftarkan dua kali.
3. Jumlah mahasiswa tidak boleh melebihi kapasitas.
4. NRP yang belum terdaftar tidak dapat dibatalkan.

### Kode Produksi

```csharp
namespace CourseRegistration.Domain;

public class CourseRegistration
{
    private readonly List<string> _studentIds = [];

    public string CourseCode { get; }
    public int Capacity { get; }
    public int RegisteredCount => _studentIds.Count;
    public IReadOnlyList<string> StudentIds => _studentIds.AsReadOnly();

    public CourseRegistration(string courseCode, int capacity)
    {
        if (string.IsNullOrWhiteSpace(courseCode))
            throw new ArgumentException("Course code is required.", nameof(courseCode));
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));
        CourseCode = courseCode;
        Capacity = capacity;
    }

    public void Register(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (_studentIds.Contains(studentId))
            throw new InvalidOperationException("Student is already registered.");
        if (RegisteredCount >= Capacity)
            throw new InvalidOperationException("Course is full.");
        _studentIds.Add(studentId);
    }

    public void Cancel(string studentId)
    {
        if (!_studentIds.Remove(studentId))
            throw new InvalidOperationException("Student is not registered.");
    }

    public bool IsRegistered(string studentId) => _studentIds.Contains(studentId);
}
```

### Tugas Unit Test

Buat `CourseRegistrationTests` untuk memeriksa:

1. mata kuliah baru memiliki jumlah nol dan daftar mahasiswa kosong;
2. `Register` menambahkan mahasiswa;
3. `IsRegistered` menghasilkan nilai yang benar;
4. NRP kosong dan hanya spasi ditolak menggunakan `[TestCase]`;
5. NRP yang sama tidak dapat didaftarkan dua kali;
6. pendaftaran ditolak ketika kapasitas penuh;
7. `Cancel` menghapus mahasiswa; dan
8. pembatalan mahasiswa yang tidak terdaftar melempar exception.

Gunakan constraint koleksi, misalnya:

```csharp
Assert.That(registration.StudentIds, Is.Empty);
Assert.That(registration.StudentIds, Does.Contain("5025261001"));
Assert.That(registration.StudentIds, Does.Not.Contain("5025261002"));
```

---

## Soal 3 — Peminjaman Buku Perpustakaan

### Deskripsi Domain dan Aturan Bisnis

`BookLoan` menghitung tanggal pengembalian dan denda keterlambatan.

1. ID buku dan NRP wajib diisi.
2. Lama peminjaman antara 1 sampai 14 hari.
3. Jatuh tempo adalah tanggal peminjaman ditambah lama peminjaman.
4. Pengembalian tepat waktu atau lebih awal tidak dikenai denda.
5. Denda keterlambatan Rp2.000 per hari.
6. Buku hanya dapat dikembalikan sekali.
7. Pengembalian tidak boleh lebih awal daripada tanggal peminjaman.

### Kode Produksi

```csharp
namespace LibraryLoan.Domain;

public class BookLoan
{
    public string BookId { get; }
    public string StudentId { get; }
    public DateOnly LoanDate { get; }
    public DateOnly DueDate { get; }
    public DateOnly? ReturnDate { get; private set; }
    public bool IsReturned => ReturnDate.HasValue;

    public BookLoan(string bookId, string studentId, DateOnly loanDate, int loanDays)
    {
        if (string.IsNullOrWhiteSpace(bookId))
            throw new ArgumentException("Book ID is required.", nameof(bookId));
        if (string.IsNullOrWhiteSpace(studentId))
            throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (loanDays is < 1 or > 14)
            throw new ArgumentOutOfRangeException(nameof(loanDays));

        BookId = bookId;
        StudentId = studentId;
        LoanDate = loanDate;
        DueDate = loanDate.AddDays(loanDays);
    }

    public decimal Return(DateOnly returnDate)
    {
        if (IsReturned)
            throw new InvalidOperationException("Book has already been returned.");
        if (returnDate < LoanDate)
            throw new ArgumentOutOfRangeException(nameof(returnDate));

        ReturnDate = returnDate;
        int lateDays = Math.Max(0, returnDate.DayNumber - DueDate.DayNumber);
        return lateDays * 2_000m;
    }
}
```

### Tugas Unit Test

Buat `BookLoanTests` untuk memeriksa:

1. constructor menyimpan data dan menghitung `DueDate`;
2. lama peminjaman tidak valid ditolak menggunakan `[TestCase]`;
3. pengembalian sebelum dan tepat pada jatuh tempo menghasilkan denda nol;
4. beberapa kasus keterlambatan menggunakan `[TestCaseSource]`;
5. `Return` mengubah `IsReturned` dan menyimpan `ReturnDate`;
6. buku tidak dapat dikembalikan dua kali; dan
7. tanggal pengembalian sebelum tanggal peminjaman ditolak.

Gunakan sumber data berikut atau buat versi setara. Tanggal pinjam adalah 1 September 2026 dan durasi pinjam 14 hari.

```csharp
private static IEnumerable<TestCaseData> LateReturnCases()
{
    yield return new TestCaseData(new DateOnly(2026, 9, 16), 2_000m)
        .SetName("Return_OneDayLate_ChargesTwoThousand");
    yield return new TestCaseData(new DateOnly(2026, 9, 18), 6_000m)
        .SetName("Return_ThreeDaysLate_ChargesSixThousand");
    yield return new TestCaseData(new DateOnly(2026, 9, 22), 14_000m)
        .SetName("Return_SevenDaysLate_ChargesFourteenThousand");
}
```

---

## Soal 4 — Pembayaran Parkir Kampus

### Deskripsi Domain dan Aturan Bisnis

`ParkingTicket` menghitung biaya parkir berdasarkan jenis kendaraan dan durasi.

1. Nomor kendaraan wajib diisi.
2. Durasi harus lebih besar dari nol.
3. Tarif motor: Rp2.000 jam pertama dan Rp1.000 setiap jam berikutnya.
4. Tarif mobil: Rp5.000 jam pertama dan Rp3.000 setiap jam berikutnya.
5. Sebagian jam dibulatkan ke atas.
6. Tiket hanya dapat dibayar satu kali.

### Kode Produksi

```csharp
namespace Parking.Domain;

public enum VehicleType { Motorcycle, Car }

public class ParkingTicket
{
    public string VehicleNumber { get; }
    public VehicleType VehicleType { get; }
    public bool IsPaid { get; private set; }

    public ParkingTicket(string vehicleNumber, VehicleType vehicleType)
    {
        if (string.IsNullOrWhiteSpace(vehicleNumber))
            throw new ArgumentException("Vehicle number is required.", nameof(vehicleNumber));
        VehicleNumber = vehicleNumber;
        VehicleType = vehicleType;
    }

    public decimal CalculateFee(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(duration));

        int hours = (int)Math.Ceiling(duration.TotalHours);
        decimal firstRate = VehicleType == VehicleType.Motorcycle ? 2_000m : 5_000m;
        decimal nextRate = VehicleType == VehicleType.Motorcycle ? 1_000m : 3_000m;
        return firstRate + ((hours - 1) * nextRate);
    }

    public void Pay()
    {
        if (IsPaid)
            throw new InvalidOperationException("Ticket has already been paid.");
        IsPaid = true;
    }
}
```

### Tugas Unit Test

Buat `ParkingTicketTests` untuk memeriksa:

1. constructor menyimpan nomor dan jenis kendaraan;
2. nomor kosong dan hanya spasi ditolak;
3. durasi nol dan negatif ditolak;
4. tarif motor untuk beberapa durasi menggunakan `[TestCaseSource]`;
5. tarif mobil untuk beberapa durasi menggunakan `[TestCaseSource]`;
6. sebagian jam dibulatkan ke atas;
7. `Pay` mengubah `IsPaid`; dan
8. pembayaran kedua melempar exception.

Contoh sumber data:

```csharp
private static IEnumerable<TestCaseData> MotorcycleFeeCases()
{
    yield return new TestCaseData(TimeSpan.FromMinutes(30), 2_000m);
    yield return new TestCaseData(TimeSpan.FromHours(1), 2_000m);
    yield return new TestCaseData(TimeSpan.FromMinutes(90), 3_000m);
    yield return new TestCaseData(TimeSpan.FromHours(3), 4_000m);
}
```

Tambahkan sendiri sumber data untuk mobil.

---

## Soal 5 — Paket Data Internet Mahasiswa

### Deskripsi Domain dan Aturan Bisnis

`MobileDataPackage` menyimpan kuota dan masa aktif paket data.

1. Nama paket wajib diisi.
2. Kuota awal dan masa aktif harus lebih besar dari nol.
3. Penggunaan data harus lebih besar dari nol dan tidak melebihi sisa kuota.
4. Penggunaan data mengurangi sisa kuota.
5. Perpanjangan harus lebih besar dari nol hari dan menambah tanggal kedaluwarsa.
6. Paket habis ketika sisa kuota sama dengan nol.

### Kode Produksi

```csharp
namespace MobileData.Domain;

public class MobileDataPackage
{
    public string PackageName { get; }
    public decimal RemainingQuotaGb { get; private set; }
    public DateOnly ExpiryDate { get; private set; }
    public bool IsDepleted => RemainingQuotaGb == 0;

    public MobileDataPackage(
        string packageName,
        decimal initialQuotaGb,
        DateOnly activationDate,
        int activeDays)
    {
        if (string.IsNullOrWhiteSpace(packageName))
            throw new ArgumentException("Package name is required.", nameof(packageName));
        if (initialQuotaGb <= 0)
            throw new ArgumentOutOfRangeException(nameof(initialQuotaGb));
        if (activeDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(activeDays));

        PackageName = packageName;
        RemainingQuotaGb = initialQuotaGb;
        ExpiryDate = activationDate.AddDays(activeDays);
    }

    public void UseData(decimal amountGb)
    {
        if (amountGb <= 0)
            throw new ArgumentOutOfRangeException(nameof(amountGb));
        if (amountGb > RemainingQuotaGb)
            throw new InvalidOperationException("Insufficient data quota.");
        RemainingQuotaGb -= amountGb;
    }

    public void Extend(int additionalDays)
    {
        if (additionalDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(additionalDays));
        ExpiryDate = ExpiryDate.AddDays(additionalDays);
    }
}
```

### Tugas Unit Test

Buat `MobileDataPackageTests` untuk memeriksa:

1. constructor menyimpan nama, kuota, dan tanggal kedaluwarsa;
2. nama paket, kuota awal, dan masa aktif yang tidak valid ditolak;
3. `UseData` mengurangi kuota;
4. beberapa jumlah penggunaan menghasilkan sisa kuota yang benar menggunakan `[TestCase]`;
5. penggunaan yang melebihi kuota melempar exception dan tidak mengubah kuota;
6. penggunaan seluruh kuota mengubah `IsDepleted` menjadi `true`;
7. `Extend` menambah tanggal kedaluwarsa; dan
8. perpanjangan nol atau negatif melempar exception.

Gunakan seluruh fitur utama yang telah dipelajari: `[TestFixture]`, `[SetUp]`, `[Test]`, `[TestCase]`, `[TestCaseSource]` jika diperlukan, `Assert.That`, `Assert.Multiple`, dan `Assert.Throws`.

Sebelum mengumpulkan tugas, jalankan:

```bash
dotnet test
```

Pastikan seluruh test terdeteksi, dapat dikompilasi, dan berstatus **Passed**.
