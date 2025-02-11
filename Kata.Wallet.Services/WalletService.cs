using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;
using Kata.Wallet.Services;

public class WalletService : IWalletService
{
    private readonly List<Wallet> _wallets = new();
    public WalletService()
    {
        // Simulamos algunas cuentas predefinidas
        _wallets = new List<Wallet>
        {
            new Wallet { Id = 1, Balance = 1000, UserDocument = "123456789", UserName = "Juan Peralta", Currency = Currency.USD },
            new Wallet { Id = 2, Balance = 500, UserDocument = "987654321", UserName = "Maria Lopez", Currency = Currency.USD },
            new Wallet { Id = 3, Balance = 1500, UserDocument = "123123123", UserName = "Carlos Perez", Currency = Currency.EUR }
        };  // Base de datos en memoria
    }
    public WalletDto CreateWallet(WalletDto walletDto)
    {
        var wallet = new Wallet
        {
            // No asignamos el Id, lo generamos en el servicio
            Balance = walletDto.Balance,
            UserDocument = walletDto.UserDocument,
            UserName = walletDto.UserName,
            Currency = walletDto.Currency
        };
        wallet.Id = _wallets.Count + 1; // Lógica para generar el Id de forma automática
        _wallets.Add(wallet);
        return walletDto; // Retornamos el DTO sin el Id desde el cliente
    }

    public IEnumerable<WalletDto> GetWallets(string? currency, string? userDocument)
    {
        var query = _wallets.AsQueryable();

        if (!string.IsNullOrEmpty(currency))
        {
            query = query.Where(w => w.Currency.ToString().Equals(currency, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(userDocument))
        {
            query = query.Where(w => w.UserDocument == userDocument);
        }

        return query.Select(w => new WalletDto
        {
            
            Balance = w.Balance,
            UserDocument = w.UserDocument,
            UserName = w.UserName,
            Currency = w.Currency
        }).ToList();
    }


    public WalletDto? GetWalletById(int id)
    {
        var wallet = _wallets.FirstOrDefault(w => w.Id == id);
        return wallet != null ? new WalletDto
        {
            Id = wallet.Id,
            Balance = wallet.Balance,
            UserDocument = wallet.UserDocument,
            UserName = wallet.UserName,
            Currency = wallet.Currency
        } : null;
    }
    public WalletDto? GetWalletByUserDocument(string userDocument)
    {
        var wallet = _wallets.FirstOrDefault(w => w.UserDocument == userDocument);
        return wallet != null ? new WalletDto
        {
            
            Balance = wallet.Balance,
            UserDocument = wallet.UserDocument,
            UserName = wallet.UserName,
            Currency = wallet.Currency
        } : null;
    }

}
