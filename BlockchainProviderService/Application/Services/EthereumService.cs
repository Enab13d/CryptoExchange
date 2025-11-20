using BlockchainProviderService.Infrastracture.Configuration;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using SharedContracts.Constants;

namespace BlockchainProviderService.Application.Services;

//implement concrete strategies
public class EthereumProviderService(IOptions<BlockchainProviderOptions> options, ILogger<EthereumProviderService> logger, ICryptoMarketDataClient client) : IBlockchainService
{
    private readonly IOptions<BlockchainProviderOptions> _options = options;
    private readonly ILogger<EthereumProviderService> _logger = logger;

    private readonly ICryptoMarketDataClient _client = client;
    private async Task<decimal> CheckBalanceAsync()
    {
        //connect to node
        string rpcURL = $"{_options.Value.Ethereum.NodeProviderURL}/{_options.Value.Ethereum.NodeAPIKey}";
        string walletAddress = _options.Value.Ethereum.ExchangeWalletAddress;
        Web3 web3 = new(rpcURL);
        HexBigInteger balance = await web3.Eth.GetBalance.SendRequestAsync(walletAddress);
        _logger.LogInformation("Balance in Wei: {balance}", balance.Value);
        decimal etherAmount = Web3.Convert.FromWei(balance.Value);
        //balance of our exchange
        _logger.LogInformation("Balance in Ether: {balance}", etherAmount);
        return etherAmount;

    }

    public void RegisterWallet()
    {
        //create wallet
        EthECKey ecKey = EthECKey.GenerateKey();
        string privateKey = ecKey.GetPrivateKey();
        string walletAddress = ecKey.GetPublicAddress();
        _logger.LogInformation("Private key: {privateKey}", privateKey);
        _logger.LogInformation("Wallet address: {publicKey}", walletAddress);
    }
    //send crypto to client
    public async Task SendCryptoAsync(string walletAddress, Currency fiat, Crypto crypto, decimal amount)
    {

        //check the course
        decimal requestedCrypto = await _client.ConvertFiatToCryptoAsync(fiat, crypto, amount);
        // take 1% margin
        requestedCrypto *= 0.99m;
        //check exchange balance
        decimal existingCrypto = await CheckBalanceAsync();
        if (existingCrypto < requestedCrypto)
        {
            throw new InvalidOperationException("Insufficient funds in exchange wallet.");
        }
        //connect to node, create client and provide transaction
        string rpcURL = $"{_options.Value.Ethereum.NodeProviderURL}/{_options.Value.Ethereum.NodeAPIKey}";
        Account account = new(_options.Value.Ethereum.ExchangeWalletPrivateKey, Chain.Sepolia);
        Web3 web3 = new(account, rpcURL);
        var weiAmount = Web3.Convert.ToWei(requestedCrypto);
        var txHash = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(walletAddress, requestedCrypto);
        // txHash.Status;




    }


}