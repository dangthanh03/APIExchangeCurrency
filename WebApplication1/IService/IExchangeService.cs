namespace WebApplication1.IService
{
    public interface IExchangeService
    {
        Task<List<string>> GetCurrencyPairs();
        decimal GetExchangeRate(DateTime date, string currencyPair);
        decimal ExchangeCurrency(decimal amount, decimal exchangeRate);
    }

}
