using Kata.Wallet.Dtos;
using Kata.Wallet.Services;
using Kata.Wallet.Domain;

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
        if (transactionDto.Amount <= 0)
        {
            return BadRequest("El monto debe ser mayor a 0.");
        }

        if (transactionDto.SourceWalletId == transactionDto.DestinationWalletId)
        {
            return BadRequest("No se puede transferir a la misma cuenta.");
        }

        var sourceWallet = _walletService.GetWalletById(transactionDto.SourceWalletId);
        var destinationWallet = _walletService.GetWalletById(transactionDto.DestinationWalletId);

        if (sourceWallet == null || destinationWallet == null)
        {
            return NotFound("Una o ambas cuentas no existen.");
        }

        if (sourceWallet.Currency != destinationWallet.Currency)
        {
            return BadRequest("Las cuentas deben tener la misma moneda para realizar la transferencia.");
        }

        if (sourceWallet.Balance < transactionDto.Amount)
        {
            return BadRequest("Saldo insuficiente en la cuenta de origen.");
        }

        var transaction = _transactionService.CreateTransaction(transactionDto);
        return CreatedAtAction(nameof(GetTransactionsById), new { id = transaction.Id }, transaction);
    }


    [HttpGet("{walletId}")]
    public IActionResult GetTransactionsById(int walletId)
    {
        var wallet = _walletService.GetWalletById(walletId);
        if (wallet == null)
        {
            return NotFound("La cuenta especificada no existe.");
        }

        var transactions = _transactionService.GetTransactions(walletId);

        if (!transactions.Any())
        {
            return NotFound("No hay transacciones para esta cuenta.");
        }

        return Ok(transactions);
    }

}