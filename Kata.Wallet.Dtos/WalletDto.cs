using Kata.Wallet.Domain;

using System.Text.Json.Serialization;

namespace Kata.Wallet.Dtos;

public class WalletDto
{
    [JsonIgnore]
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public string? UserDocument { get; set; }
    public string? UserName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Currency Currency { get; set; }
}
