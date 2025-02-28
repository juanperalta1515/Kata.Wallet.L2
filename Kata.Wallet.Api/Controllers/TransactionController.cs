using Kata.Wallet.Dtos;
using Kata.Wallet.Services;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IWalletService _walletService;

    public TransactionController(ITransactionService transactionService, IWalletService walletService)
    {
        _transactionService = transactionService;
        _walletService = walletService;
    }

    [HttpPost]
    public IActionResult CreateTransaction([FromBody] TransactionDto transactionDto)
    {
        try
        {
            var transaction = _transactionService.CreateTransaction(transactionDto);
            return CreatedAtAction(nameof(GetTransactionsById), new { walletId = transaction.SourceWalletId }, transaction);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{walletId}")]
    public IActionResult GetTransactionsById(int walletId)
    {
        var wallet = _walletService.GetWalletById(walletId);
        if (wallet == null)
            return NotFound("La cuenta especificada no existe.");

        var transactions = _transactionService.GetTransactions(walletId);
        return transactions.Any() ? Ok(transactions) : NotFound("No hay transacciones para esta cuenta.");
    }
}
