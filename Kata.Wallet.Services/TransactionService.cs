using Kata.Wallet.Dtos;
using Kata.Wallet.Domain;
using Kata.Wallet.Database;
using Microsoft.EntityFrameworkCore;

namespace Kata.Wallet.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _context;
        private readonly IWalletService _walletService;

        public TransactionService(DataContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        public TransactionDto CreateTransaction(TransactionDto transactionDto)
        {
            var sourceWallet = _context.Wallets.Find(transactionDto.SourceWalletId);
            var destinationWallet = _context.Wallets.Find(transactionDto.DestinationWalletId);

            if (sourceWallet == null || destinationWallet == null)
                throw new Exception("Una o ambas cuentas no existen.");

            if (sourceWallet.Currency != destinationWallet.Currency)
                throw new Exception("Las cuentas deben tener la misma moneda para transferencias.");

            if (sourceWallet.Balance < transactionDto.Amount)
                throw new Exception("Saldo insuficiente en la cuenta de origen.");

            var transaction = new Transaction
            {
                Amount = transactionDto.Amount,
                Date = DateTime.UtcNow,
                Description = transactionDto.Description,
                WalletOutgoing = sourceWallet,
                WalletIncoming = destinationWallet
            };

            // Actualizar saldos
            sourceWallet.Balance -= transaction.Amount;
            destinationWallet.Balance += transaction.Amount;

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                SourceWalletId = transactionDto.SourceWalletId,
                DestinationWalletId = transactionDto.DestinationWalletId
            };
        }

        public IEnumerable<TransactionDto> GetTransactions(int walletId)
        {
            return _context.Transactions
                .Where(t => t.WalletOutgoing.Id == walletId || t.WalletIncoming.Id == walletId)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                    SourceWalletId = t.WalletOutgoing.Id,
                    DestinationWalletId = t.WalletIncoming.Id
                })
                .ToList();
        }
    }
}
