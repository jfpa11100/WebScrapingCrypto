using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;

class Program
{
    public static void Main()
    {
        WebScraper.Get_crypto_data("https://finance.yahoo.com/crypto/?count=200&offset=0");
    }
}
class WebScraper
{
    public static async Task<string> Call_url(string fullUrl)
    {
        HttpClient client = new();
        var response = await client.GetStringAsync(fullUrl);
        return response;
    }
    public static List<List<string>> Parse_html(string html)
    {
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(html);


        var parsed_data1 = htmlDoc.DocumentNode.Descendants("tr")
                .Where(node => node.GetAttributeValue("class", "").Contains("simpTblRow Bgc($hoverBgColor):h BdB Bdbc($seperatorColor) Bdbc($tableBorderBlue):h H(32px) Bgc($lv2BgColor) ")).
                ToList();

        var parsed_data2 = htmlDoc.DocumentNode
        .SelectNodes("//tr[@class='simpTblRow Bgc($hoverBgColor):h BdB Bdbc($seperatorColor) Bdbc($tableBorderBlue):h H(32px) Bgc($lv1BgColor) ']").ToList();

        List<List<string>> crypto_data = new();
        List<string> elementCrypto = new();
        for (int i=0;i<100;i++)
        {
            //name
            elementCrypto.Add(parsed_data1[i].SelectSingleNode("//td[@class='Va(m) Ta(start) Px(10px) Fz(s)']").InnerText);

            //Price
            elementCrypto.Add(parsed_data1[i].SelectSingleNode("//td[@class='Va(m) Ta(end) Pstart(20px) Fw(600) Fz(s)']").FirstChild.InnerText);

            //%change
            elementCrypto.Add(parsed_data1[i].SelectSingleNode("//td[@aria-label='% Change']").FirstChild.FirstChild.InnerText);
            //Price+change
            

            crypto_data.Add(elementCrypto);
            elementCrypto.Clear();
            //--------------------------------------------------------------------------------------------------------------------------------------------
            elementCrypto.Add(parsed_data2[i].SelectSingleNode("//td[@class='Va(m) Ta(start) Px(10px) Fz(s)']").InnerText);

            //Price
            elementCrypto.Add(parsed_data2[i].SelectSingleNode("//td[@class='Va(m) Ta(end) Pstart(20px) Fw(600) Fz(s)']").FirstChild.InnerText);

            //%change
            elementCrypto.Add(parsed_data2[i].SelectSingleNode("//td[@aria-label='% Change']").FirstChild.FirstChild.InnerText);
            //Price+change


            crypto_data.Add(elementCrypto);
            elementCrypto.Clear();
        }
        return crypto_data;
    }
    public static List<List<string>> Get_crypto_data(string url)
    {
        string response = Call_url(url).Result;
        List<List<string>> crypto_data = Parse_html(response);
        return crypto_data;
    }
}