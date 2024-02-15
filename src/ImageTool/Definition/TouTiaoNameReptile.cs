using System.Collections.Concurrent;
using System.Text;
using System.Web;

namespace ImageTool.Definition
{
    /// <summary>
    /// 头条爬虫
    /// </summary>
    public class TouTiaoNameReptile
    {
        public static string AnalyseInput(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Position = 0;
                var fileContent = fs.Read(bytes, 0, (int)fs.Length);

                var fileString = System.Text.UTF8Encoding.UTF8.GetString(bytes);
                return fileString;
            }
        }

        public static async Task WriteRecombination(string input, string outPutPath)
        {
            //var inputArray = input.Split('|').Select(n => n.Trim()).OrderBy(n => n.Length).ToList();
            var inputArray = input.Split(Environment.NewLine).Select(n => n.Trim()).OrderBy(n => n.Length).ToList();

            //StringBuilder[] writeArray = new StringBuilder[10];
            //StringBuilder sbWrite = new StringBuilder();
            List<string> resultArray = new List<string>();

            //ConcurrentBag<string> tempArray = new ConcurrentBag<string>();
            //Parallel.ForEach(array, new ParallelOptions { MaxDegreeOfParallelism = 4 }, async item =>
            //{
            //    //if (item.Contains(" ") || item.Length < 4)
            //    //{
            //    //    continue;
            //    //}
            //    var petName = $"{item.ToLower()} girl";
            //    //var petName = $"{item.ToLower()} gril";
            //    if (await SreachByPetName(petName))
            //    {
            //        //sbWrite.Append($"{petName}{Environment.NewLine}");
            //        tempArray.Add(petName);
            //    }
            //});
            //var sbWrite = string.Join(Environment.NewLine, tempArray.OrderBy(n => n.Length).Select(n => n));

            int schedule = 1;

            foreach (var item in inputArray)
            {
                if (item.Contains(" ") || item.Length < 4 || item.Length > 25)
                {
                    continue;
                }
                var petName = $"{item.ToLower()} girl";
                Console.WriteLine($"{schedule.ToString()}:{petName}");
                if (await SreachByPetName(petName))
                {
                    resultArray.Add(petName);
                }
                schedule++;
            }
            var writeArray = string.Join(Environment.NewLine, resultArray.OrderBy(n => n.Length).ThenBy(n => n));
            var bytes = UTF8Encoding.UTF8.GetBytes(writeArray);
            using (FileStream fs = new FileStream(outPutPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Position = 0;
                fs.Write(bytes);
            }
        }

        public static async Task ParallelWriteRecombination(string input, string outPutPath)
        {
            //var inputArray = input.Split('|').Select(n => n.Trim()).OrderBy(n => n.Length).ToList();
            var inputArray = input.Split(Environment.NewLine).Select(n => n.Trim()).OrderBy(n => n.Length).ToList();

            ConcurrentBag<string> resultArray = new ConcurrentBag<string>();

            Parallel.ForEach(inputArray, new ParallelOptions { MaxDegreeOfParallelism = 1 }, async item =>
            {
                if (!item.Contains(" ") && item.Length >= 4 && item.Length <= 25)
                {
                    var petName = $"{item.ToLower()} girl";
                    Console.WriteLine(petName);
                    if (await SreachByPetName(petName))
                    {
                        resultArray.Add(petName);
                    }
                    await Task.Delay(500);
                }
            });

            var writeArray = string.Join(Environment.NewLine, resultArray.OrderBy(n => n.Length).ThenBy(n => n));
            var bytes = UTF8Encoding.UTF8.GetBytes(writeArray);
            using (FileStream fs = new FileStream(outPutPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Position = 0;
                fs.Write(bytes);
            }
        }

        public static async Task<bool> SreachByPetName(string petName)
        {
            using (var client = HttpClientFactory.Create())
            {
                var queryName = HttpUtility.UrlEncode(petName);
                Uri uri = new Uri($"https://so.toutiao.com/search?dvpf=pc&source=input&keyword={queryName}&pd=user&action_type=search_subtab_switch&page_num=0&from=media&cur_tab_title=media");
                //client.BaseAddress = uri;

                client.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0");
                client.DefaultRequestHeaders.Add("Cookie", @$"msToken=8zD9SvjCySWC06cvDJDouqV9YU_FzcCisDn1nICB6rWRAXTin-h1EwfVBDbp3dzxgD3nipwbrTrPoABpc7hzEIIpwbjuMzhYG_HhjfgW; tt_webid=7326490851436217871; _tea_utm_cache_4916=undefined; _S_WIN_WH=2056_1141; _S_DPR=1.3958333730697632; _S_IPAD=0; s_v_web_id=verify_lrnc3mtn_fqo6lNru_dbXx_4UEY_BTol_Pdy6Lx0a5Gly; ttwid=1%7CF8u838scL1zRBdGM36qKyGoffLVy21tOLVbhDLqaZI4%7C1705833506%7Ccd8471e36c3fed37cb0fd698d13223d21a01f97fcbb6a424ba0a591e29828b5c; __ac_nonce=065ad25500067b18d1e43; __ac_signature=_02B4Z6wo00f01ZS7dFAAAIDBYtJ9dSIyy-2Um3DAAACEeb; __ac_referer=https://so.toutiao.com/search?dvpf=pc&source=input&keyword={petName}&pd=user&action_type=search_subtab_switch&page_num=0&from=media&cur_tab_title=media");
                var result = await client.GetAsync(uri);
                if (!result.IsSuccessStatusCode)
                {
                    return false;
                }
                var responseContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                if (responseContent.Contains("抱歉，未找到"))
                {
                    return false;
                }

                return true;
            }
        }

        public static async Task<List<string>> GetAllPWorld()
        {
            var result = new List<string>();
            var pageFlag = 1;
            using (var client = HttpClientFactory.Create())
            {
                while (true)
                {
                    var url = new Uri($"https://koolearn.com/dict/zimu_p_{pageFlag}.html");
                    client.DefaultRequestHeaders.Add("Cookie", "__jsluid_s=28fd3e0c8303f1942ca3688967a66392; _csrf=f29ea31c74f2155184274a6aa8f670b10e7faae2666fe0676022d4e23ec5ca3ba%3A2%3A%7Bi%3A0%3Bs%3A5%3A%22_csrf%22%3Bi%3A1%3Bs%3A32%3A%22FV2XiPAsb5hq1m6fDhbgqcGglF6ijMY2%22%3B%7D; php-webapp-dict=63df0281ff3858097dbd2665fdaecb90; koolearnad=true");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0");
                    var request = await client.GetAsync(url);
                    if (!request.IsSuccessStatusCode)
                    {
                        break;
                    }

                    var pageContent = await request.Content.ReadAsStringAsync();

                    //var start = pageContent.IndexOf("<body>");
                    //var end = pageContent.IndexOf("</body>");
                    //var html1 = pageContent.Substring(start, end - start);

                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(pageContent);
                    var pageWordNodes = doc.DocumentNode.SelectNodes("//a[@class='word']");
                    if (pageWordNodes != null)
                    {
                        foreach (var node in pageWordNodes)
                        {
                            result.Add(node.InnerText);
                        }
                        pageFlag++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public static void WriteWordToFile(IEnumerable<string> words, string savePath)
        {
            words = words.OrderBy(n => n.Length).Select(n => n.ToLower()).ToList();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var word in words)
            {
                stringBuilder.Append($"{word}{Environment.NewLine}");
            }

            using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var contentAray = UTF8Encoding.UTF8.GetBytes(stringBuilder.ToString());
                fs.Position = 0;
                fs.Write(contentAray, 0, contentAray.Length);
            }
        }
    }
}
