using System;
using System.Net;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.VisualBasic.Logging;

namespace SportOrgMultyDay.Processing.Parsing.Things
{
    public static class ProtocolParser
    {
        public static string GetBaseFromProtocolUrl(string url, out string log)
        {
            log = "Импорт протокола по ссылке...\n";
            try
            {
                log += $"Получение данных с сайта [{url}]\n";
                var task = Task.Run(() => GetHttpResponse(url));
                string protocolRaw = task.Result;
                return ParseRawProtocol(protocolRaw, ref log);
            }
            catch (Exception ex)
            {
                log += $"\n  ERROR: {ex.Message}";
            }
            return null;
        }
        public static string GetBaseFromProtocolFile(string input, out string log)
        {
            log = "Импорт протокола из файла...\n";

            //return ParseRawProtocol(protocolRaw);
            return "";
        }

        public static string ParseRawProtocol(string rawProtocol, ref string msgLog)
        {
            msgLog += "  Парсинг протокола...\n";
            // Создаем объект HtmlDocument и загружаем HTML-код
            HtmlAgilityPack.HtmlDocument doc = new();
            doc.LoadHtml(rawProtocol);

            // Ищем скрипты в документе
            var scriptNodes = doc.DocumentNode.SelectNodes("//script");

            string htmlText = "";
            int startIndex = -1;
            foreach (var scriptNode in scriptNodes )
            {
                htmlText = scriptNode.InnerText;
                startIndex = htmlText.IndexOf("var race =");
                if (startIndex >= 0)
                    break;
            }

            // Находим индекс начала строки "var race ="

            if (startIndex != -1)
            {
                // Находим индекс первой открывающей скобки после "var race ="
                int openBracketIndex = htmlText.IndexOf("{", startIndex);

                if (openBracketIndex != -1)
                {
                    // Считаем открытые и закрытые скобки
                    int bracketCount = 1; // Начинаем с 1, так как первая скобка уже найдена

                    // Начиная с открывающей скобки, ищем закрывающую скобку
                    for (int i = openBracketIndex + 1; i < htmlText.Length; i++)
                    {
                        if (htmlText[i] == '{')
                            bracketCount++;
                        else if (htmlText[i] == '}')
                        {
                            bracketCount--;
                            if (bracketCount == 0)
                            {
                                string jsonString = htmlText.Substring(openBracketIndex, i - openBracketIndex + 1);
                                string startWith = jsonString[..(jsonString.Length > 30 ? 30 : jsonString.Length)];
                                msgLog += $"  База найдена. Длинна базы - {jsonString.Length}. Начало - {startWith}\n";
                                return jsonString;
                            }
                        }
                    }
                }
                else
                    msgLog += "\n  ⚠Ошибка - Не найдена открывающая скобка '{' после 'var race ='.";
            }
            else
            {
                msgLog += "\n  ⚠Ошибка - Строка 'var race =' не найдена в HTML-документе.";
            }
            return null;
        }

        private async static Task<string> GetHttpResponse(string url)
        {

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(5);
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string htmlCode = await response.Content.ReadAsStringAsync();
                    return htmlCode;
                }
                else
                    throw Logger.LogError("902hd84hgaso", $"⚠Ошибка при запросе на Url: [{url}] Ответ: [{response.StatusCode}]", true);
            }
        }
    }
}
