namespace Kata.Wallet.Dtos;

public class TransactionDto
{
    public int Id { get; set; }
    public int SourceWalletId { get; set; }
    public int DestinationWalletId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}
