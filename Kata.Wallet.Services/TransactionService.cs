using Kata.Wallet.Dtos;
using Kata.Wallet.Services;
using Kata.Wallet.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly List<Transaction> _transactions = new();
        private readonly IWalletService _walletService;

        public TransactionService(IWalletService walletService)
        {
            _walletService = walletService;
            _transactions = new List<Transaction>();
            {
                new Transaction { Id = 1, Amount = 200, Date = DateTime.UtcNow.AddDays(-2), Description = "Pago de servicio", SourceWalletId = 1, DestinationWalletId = 2 };
                new Transaction { Id = 2, Amount = 500, Date = DateTime.UtcNow.AddDays(-1), Description = "Transferencia personal", SourceWalletId = 2, DestinationWalletId = 3 };
                new Transaction { Id = 3, Amount = 1000, Date = DateTime.UtcNow, Description = "Pago de alquiler", SourceWalletId = 3, DestinationWalletId = 1 };
            };
        }

        public TransactionDto CreateTransaction(TransactionDto transactionDto)
        {
            var transaction = new Transaction
            {
                Id = _transactions.Count + 1,
                Amount = transactionDto.Amount,
                Date = DateTime.UtcNow,
                Description = transactionDto.Description,
                SourceWalletId = transactionDto.SourceWalletId,
                DestinationWalletId = transactionDto.DestinationWalletId
            };
            _transactions.Add(transaction);
            return transactionDto;
        }

        public IEnumerable<TransactionDto> GetTransactions(int walletId)
        {
            return _transactions
                .Where(t => t.SourceWalletId == walletId || t.DestinationWalletId == walletId)  // Filtra por el walletId
                .Select(t => new TransactionDto  // Convierte a DTO
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                     SourceWalletId = t.SourceWalletId,
                    DestinationWalletId = t.DestinationWalletId
                }).ToList(); // Convierte a lista para evitar problemas de iteración
        }
        public TransactionDto? GetTransactionById(int id)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == id);
            return transaction != null ? new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                  SourceWalletId = transaction.SourceWalletId,
                DestinationWalletId = transaction.DestinationWalletId
            } : null;
        }

    }
}
