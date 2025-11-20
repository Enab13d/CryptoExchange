using SharedContracts.Constants;
namespace BlockchainProviderService.Application.Mappers;

public interface ICurrencyMapper
{
    public int CryptoToConversionId(Crypto crypto);
    public int FiatToConverstionId(Currency fiat);
}