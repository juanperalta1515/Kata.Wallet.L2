using System;
using System.Linq;
using Kata.Wallet.Dtos;
using Kata.Wallet.Services;
using Kata.Wallet.Domain;
using Xunit;

namespace Kata.Wallet.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly TransactionService _transactionService;
        private readonly WalletService _walletService;

        public TransactionServiceTests()
        {
            _walletService = new WalletService(); // Usa la implementación en memoria
            _transactionService = new TransactionService(_walletService);
        }

        [Fact]
        public void CreateTransaction_ShouldSucceed_WhenValid()
        {
            // Arrange
            var transactionDto = new TransactionDto
            {
                Amount = 100,
                Description = "Pago de servicio",
                SourceWalletId = 1,
                DestinationWalletId = 2
            };

            // Act
            var transaction = _transactionService.CreateTransaction(transactionDto);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(100, transaction.Amount);
        }

        [Fact]
        public void GetTransactions_ShouldReturnTransactionsForWallet()
        {
            // Act
            var transactions = _transactionService.GetTransactions(1).ToList();

            // Assert
            Assert.NotEmpty(transactions);
            Assert.Contains(transactions, t => t.SourceWalletId == 1 || t.DestinationWalletId == 1);
        }

        [Fact]
        public void GetTransactionById_ShouldReturnCorrectTransaction()
        {
            // Arrange
            var transactionDto = new TransactionDto
            {
                Amount = 150,
                Description = "Transferencia",
                SourceWalletId = 1,
                DestinationWalletId = 2
            };
            _transactionService.CreateTransaction(transactionDto);

            // Act
            var transaction = _transactionService.GetTransactionById(4);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(150, transaction.Amount);
        }
    }
}
