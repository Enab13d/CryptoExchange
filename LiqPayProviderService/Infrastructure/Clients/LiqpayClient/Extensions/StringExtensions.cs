using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace LiqPayProviderService.Infrastructure.Clients.LiqpayClient.Extensions;

public static class StringExtensions
{
    public static string ToBase64(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Minify JSON first
        string minifiedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(input));

        var bytes = Encoding.UTF8.GetBytes(minifiedJson);
        return Convert.ToBase64String(bytes);
    }



}