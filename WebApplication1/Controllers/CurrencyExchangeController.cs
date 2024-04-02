using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication1.IService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public CurrencyExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet]
        public IActionResult ExchangeCurrency(DateTime date, decimal amount, string currencyPair)
        {
            // Lấy tỷ giá từ ExchangeService
            decimal exchangeRate = _exchangeService.GetExchangeRate(date, currencyPair);

            if (exchangeRate == 0)
            {
                return BadRequest("Không tìm thấy tỷ giá cho cặp tiền tệ đã cho.");
            }

            // Thực hiện trao đổi tiền tệ
            decimal exchangedAmount = _exchangeService.ExchangeCurrency(amount, exchangeRate);

            // Trả về kết quả
            return Ok(new { originalAmount = amount, exchangedAmount = exchangedAmount });
        }
    }
}
