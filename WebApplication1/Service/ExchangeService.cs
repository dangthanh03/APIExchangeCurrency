using System.Xml;
using WebApplication1.IService;

namespace WebApplication1.Service
{
    public class ExchangeService : IExchangeService
    {
        private readonly string _vietcombankApiUrl = "https://portal.vietcombank.com.vn/Usercontrols/TVPortal.TyGia/pXML.aspx?b=10";

        public decimal GetExchangeRate(DateTime date, string currencyPair)
        {
            // Tạo yêu cầu HTTP để lấy dữ liệu từ API của Vietcombank
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(_vietcombankApiUrl);

            // Lấy danh sách các nút Exrate từ XML
            XmlNodeList exrateList = xmlDoc.GetElementsByTagName("Exrate");

            // Lặp qua từng Exrate để tìm tỷ giá phù hợp
            foreach (XmlNode exrate in exrateList)
            {
                string exrateCurrencyCode = exrate.Attributes["CurrencyCode"].Value;
                if (exrateCurrencyCode == currencyPair)
                {
                    // Lấy tỷ giá mua chuyển đổi
                    string exrateValue = exrate.Attributes["Transfer"].Value;
                    decimal exchangeRate = Convert.ToDecimal(exrateValue.Replace(",", "")); // Xóa dấu phẩy trong giá trị để chuyển sang decimal
                    return exchangeRate;
                }
            }

            // Nếu không tìm thấy tỷ giá, trả về 0 hoặc giá trị mặc định khác
            return 0;
        }
        public async Task<List<string>> GetCurrencyPairs()
        {
            List<string> currencyPairs = new List<string>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Gửi yêu cầu HTTP GET để lấy dữ liệu từ API của Vietcombank
                    HttpResponseMessage response = await httpClient.GetAsync(_vietcombankApiUrl);
                    response.EnsureSuccessStatusCode();

                    // Đọc nội dung từ phản hồi
                    string xmlContent = await response.Content.ReadAsStringAsync();

                    // Phân tích dữ liệu XML
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlContent);

                    // Lấy danh sách các nút Exrate từ XML
                    XmlNodeList exrateList = xmlDoc.GetElementsByTagName("Exrate");

                    // Lặp qua từng Exrate để trích xuất currencyPair
                    foreach (XmlNode exrate in exrateList)
                    {
                        string currencyCode = exrate.Attributes["CurrencyCode"].Value;
                        currencyPairs.Add(currencyCode);
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Xử lý lỗi khi gửi yêu cầu HTTP
                    throw new Exception("Error sending HTTP request: " + ex.Message, ex);
                }
                catch (XmlException ex)
                {
                    // Xử lý lỗi khi phân tích dữ liệu XML
                    throw new Exception("Error parsing XML response: " + ex.Message, ex);
                }
            }

            return currencyPairs;
        }
        public decimal ExchangeCurrency(decimal amount, decimal exchangeRate)
        {
            return amount * exchangeRate;
        }
    }

}
