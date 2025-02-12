
using System.Collections.Generic;
using Kata.Wallet.Dtos;
using Kata.Wallet.Services;
using Kata.Wallet.Domain;
using Xunit;

namespace Kata.Wallet.Tests.Services
{
    public class WalletServiceTests
    {
        private readonly WalletService _walletService;

        public WalletServiceTests()
        {
            _walletService = new WalletService(); // Usa la implementación en memoria
        }

        [Fact]
        public void GetWalletById_ShouldReturnWallet_WhenWalletExists()
        {
            // Act
            var result = _walletService.GetWalletById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1000, result.Balance);
        }

        [Fact]
        public void GetWalletById_ShouldReturnNull_WhenWalletDoesNotExist()
        {
            // Act
            var result = _walletService.GetWalletById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateWallet_ShouldAddNewWallet()
        {
            // Arrange
            var newWallet = new WalletDto
            {
                Balance = 2000,
                UserDocument = "555555555",
                UserName = "Pedro Gomez",
                Currency = Currency.USD
            };

            // Act
            var createdWallet = _walletService.CreateWallet(newWallet);
            var retrievedWallet = _walletService.GetWalletByUserDocument("555555555");

            // Assert
            Assert.NotNull(createdWallet);
            Assert.NotNull(retrievedWallet);
            Assert.Equal(2000, retrievedWallet.Balance);
        }

        [Fact]
        public void GetWalletByUserDocument_ShouldReturnCorrectWallet()
        {
            // Act
            var result = _walletService.GetWalletByUserDocument("123456789");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Juan Peralta", result.UserName);
        }
    }
}
