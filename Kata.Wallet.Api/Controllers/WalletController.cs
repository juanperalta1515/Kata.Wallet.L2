using Kata.Wallet.Services;
using Kata.Wallet.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/wallets")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ITransactionService _transactionService;

    
    public WalletController(IWalletService walletService, ITransactionService transactionService)
    {
        _walletService = walletService;
        _transactionService = transactionService;
    }

    [HttpPost]
    public IActionResult CreateWallet([FromBody] WalletDto walletDto)
    {
        if (walletDto.Balance < 0)
        {
            return BadRequest("El saldo inicial no puede ser negativo.");
        }

        var existingWallet = _walletService.GetWalletByUserDocument(walletDto.UserDocument);
        if (existingWallet != null)
        {
            return Conflict("El usuario ya tiene una cuenta.");
        }

        var newWallet = _walletService.CreateWallet(walletDto);
        return CreatedAtAction(nameof(GetWalletById), new { id = newWallet.UserName }, newWallet);
    }
    [HttpGet]
    public IActionResult GetWallets([FromQuery] string? currency, [FromQuery] string? userDocument)
    {
        var wallets = _walletService.GetWallets(currency, userDocument);

        if (!wallets.Any())
        {
            return NotFound("No se encontraron cuentas con los filtros especificados.");
        }

        return Ok(wallets);
    }
    [HttpGet("{id}")]
    public IActionResult GetWalletById(int id)
    {
        var wallet = _walletService.GetWalletById(id);
        if (wallet == null)
        {
            return NotFound("Cuenta no encontrada.");
        }
        return Ok(wallet);
    }
}