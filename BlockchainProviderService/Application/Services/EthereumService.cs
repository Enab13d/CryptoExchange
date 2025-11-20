using BlockchainProviderService.Infrastracture.Configuration;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;

namespace BlockchainProviderService.Application.Services;

//implement concrete strategies
public class EthereumProviderService(IOptions<BlockchainProviderOptions> options, ILogger<EthereumProviderService> logger) : IBlockchainService
{
    private readonly IOptions<BlockchainProviderOptions> _options = options;
    private readonly ILogger<EthereumProviderService> _logger = logger;
    public async Task CheckBalanceAsync()
    {
        //connect to node
        string rpcURL = $"{_options.Value.Ethereum.NodeProviderURL}/{_options.Value.Ethereum.NodeAPIKey}";
        string walletAddress = "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae";
        Web3 web3 = new(rpcURL);
        HexBigInteger balance = await web3.Eth.GetBalance.SendRequestAsync(walletAddress);
        _logger.LogInformation("Balance in Wei: {balance}", balance.Value);
        decimal etherAmount = Web3.Convert.FromWei(balance.Value);
        _logger.LogInformation("Balance in Ether: {balance}", balance.Value);
        Account account = new("asdasd");
        Web3 x = new(account);


    }

    public Task SendTransactionAsync()
    {
        //create wallet
        EthECKey ecKey = EthECKey.GenerateKey();
        string privateKey = ecKey.GetPrivateKey();
        string walletAddress = ecKey.GetPublicAddress();


        throw new NotImplementedException();
    }


}