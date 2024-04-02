namespace WebApplication1.IService
{
    public interface IExchangeService
    {
        decimal GetExchangeRate(DateTime date, string currencyPair);
        decimal ExchangeCurrency(decimal amount, decimal exchangeRate);
    }

}
