namespace BlockchainProviderService.Application.Services;

//define strategy interface
public interface IBlockchainService
{
    public Task SendTransactionAsync();

    public Task CheckBalanceAsync();
}