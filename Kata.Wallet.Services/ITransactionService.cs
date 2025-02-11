using Kata.Wallet.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Services
{
    public interface ITransactionService
    {
        TransactionDto CreateTransaction(TransactionDto transactionDto);
        IEnumerable<TransactionDto> GetTransactions(int walletId);
    }

}
