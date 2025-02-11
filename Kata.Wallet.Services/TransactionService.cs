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
        }

        public TransactionDto CreateTransaction(TransactionDto transactionDto)
        {
            var transaction = new Transaction
            {
                Id = _transactions.Count + 1,
                Amount = transactionDto.Amount,
                Date = DateTime.UtcNow,
                Description = transactionDto.Description
            };
            _transactions.Add(transaction);
            return transactionDto;
        }

        public IEnumerable<TransactionDto> GetTransactions(int walletId)
        {
            return _transactions
                .Where(t => t.Id == walletId)  // Filtra por el walletId
                .Select(t => new TransactionDto  // Convierte a DTO
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description
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
                Description = transaction.Description
            } : null;
        }

    }
}
