using Kata.Wallet.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Services
{
    public interface IWalletService
    {
        WalletDto CreateWallet(WalletDto walletDto);
        WalletDto? GetWalletById(int id);
        WalletDto? GetWalletByUserDocument(string userDocument);
        IEnumerable<WalletDto> GetWallets(string? currency, string? userDocument);
    }
}
