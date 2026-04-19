using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SportOrgMultyDay.Processing
{
    internal sealed class ProtocolHtmlCompressorSettings
    {
        public bool ClearPersonsSensitiveData { get; set; } = true;
        public bool ClearOrganizationsSensitiveData { get; set; } = true;
    }

    internal class ProtocolHtmlCompressor
    {
        public string CompressProtocolHtml(
            string htmlContent,
            string pakoJsContent,
            ProtocolHtmlCompressorSettings? settings = null)
        {
            settings ??= new ProtocolHtmlCompressorSettings();

            if (string.IsNullOrWhiteSpace(htmlContent))
                throw new ArgumentException("Входной HTML пустой.");

            if (string.IsNullOrWhiteSpace(pakoJsContent))
                throw new ArgumentException("Содержимое pako.min.js пустое.");

            int varIndex = htmlContent.IndexOf("var race =", StringComparison.Ordinal);
            if (varIndex < 0)
                throw new Exception("В HTML не найден блок 'var race ='.");

            int jsonStart = htmlContent.IndexOf('{', varIndex);
            if (jsonStart < 0)
                throw new Exception("Не найдено начало JSON после 'var race ='.");

            int jsonEnd = FindMatchingBrace(htmlContent, jsonStart);
            if (jsonEnd < 0)
                throw new Exception("Не найден конец JSON блока race.");

            int statementEnd = htmlContent.IndexOf(';', jsonEnd);
            if (statementEnd < 0)
                throw new Exception("Не найден конец выражения 'var race = ...;'.");

            string raceJson = htmlContent.Substring(jsonStart, jsonEnd - jsonStart + 1);

            JObject race;
            try
            {
                race = JObject.Parse(raceJson);
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось распарсить JSON блока race: " + ex.Message, ex);
            }

            ApplyFilters(race, settings);

            string filteredJson = JsonConvert.SerializeObject(race, Formatting.None);
            string compressedBase64 = CompressStringToBase64(filteredJson);

            // 1. Вставляем pako ВНУТРЬ того же script-блока, но раньше var race =
            string htmlWithPakoInjected = InjectPakoIntoRaceScriptBlock(htmlContent, varIndex, pakoJsContent);

            // После вставки pako позиции сдвинулись, ищем var race заново
            int newVarIndex = htmlWithPakoInjected.IndexOf("var race =", StringComparison.Ordinal);
            if (newVarIndex < 0)
                throw new Exception("После вставки pako блок 'var race =' не найден.");

            int newJsonStart = htmlWithPakoInjected.IndexOf('{', newVarIndex);
            if (newJsonStart < 0)
                throw new Exception("После вставки pako не найдено начало JSON.");

            int newJsonEnd = FindMatchingBrace(htmlWithPakoInjected, newJsonStart);
            if (newJsonEnd < 0)
                throw new Exception("После вставки pako не найден конец JSON блока race.");

            int newStatementEnd = htmlWithPakoInjected.IndexOf(';', newJsonEnd);
            if (newStatementEnd < 0)
                throw new Exception("После вставки pako не найден конец выражения 'var race = ...;'.");

            string raceAssignmentReplacement = BuildRaceAssignmentScript(compressedBase64);

            return htmlWithPakoInjected.Substring(0, newVarIndex)
                 + raceAssignmentReplacement
                 + htmlWithPakoInjected.Substring(newStatementEnd + 1);
        }

        public string CompressProtocolHtml(
            string htmlContent,
            string pakoJsContent,
            bool clearPersonsSensitiveData,
            bool clearOrganizationsSensitiveData)
        {
            return CompressProtocolHtml(
                htmlContent,
                pakoJsContent,
                new ProtocolHtmlCompressorSettings
                {
                    ClearPersonsSensitiveData = clearPersonsSensitiveData,
                    ClearOrganizationsSensitiveData = clearOrganizationsSensitiveData
                });
        }

        public string CompressProtocolHtmlWithLocalPakoFile(
            string htmlContent,
            string pakoFilePath,
            ProtocolHtmlCompressorSettings? settings = null)
        {
            if (string.IsNullOrWhiteSpace(pakoFilePath))
                throw new ArgumentException("Не указан путь к pako.min.js.");

            if (!File.Exists(pakoFilePath))
                throw new FileNotFoundException("Файл pako.min.js не найден.", pakoFilePath);

            string pakoJsContent = File.ReadAllText(pakoFilePath, Encoding.UTF8);
            return CompressProtocolHtml(htmlContent, pakoJsContent, settings);
        }

        private static string InjectPakoIntoRaceScriptBlock(string htmlContent, int varIndex, string pakoJsContent)
        {
            int scriptStart = htmlContent.LastIndexOf("<script", varIndex, StringComparison.OrdinalIgnoreCase);
            if (scriptStart < 0)
                throw new Exception("Не найден открывающий <script> для блока race.");

            int scriptStartEnd = htmlContent.IndexOf('>', scriptStart);
            if (scriptStartEnd < 0 || scriptStartEnd > varIndex)
                throw new Exception("Не найден конец открывающего тега <script> для блока race.");

            string safePakoJs = pakoJsContent.Replace("</script>", "<\\/script>");

            string injection = "\n" + safePakoJs + "\n";

            return htmlContent.Insert(scriptStartEnd + 1, injection);
        }

        private static void ApplyFilters(JObject race, ProtocolHtmlCompressorSettings settings)
        {
            if (settings.ClearPersonsSensitiveData && race["persons"] is JArray persons)
            {
                foreach (JToken token in persons)
                {
                    if (token is not JObject person)
                        continue;

                    ClearField(person, "middle_name");
                    //ClearField(person, "birth_date");
                    ClearField(person, "birthday");
                    ClearField(person, "birthDay");
                    ClearField(person, "date_of_birth");
                    ClearField(person, "full_birth_date");
                }
            }

            if (settings.ClearOrganizationsSensitiveData && race["organizations"] is JArray organizations)
            {
                foreach (JToken token in organizations)
                {
                    if (token is not JObject organization)
                        continue;

                    ClearField(organization, "contact");
                    ClearField(organization, "phone");
                    ClearField(organization, "email");
                    ClearField(organization, "site");
                    ClearField(organization, "url");
                    ClearField(organization, "telegram");
                    ClearField(organization, "vk");
                }
            }
        }

        private static void ClearField(JObject obj, string fieldName)
        {
            JProperty? prop = obj.Property(fieldName);
            if (prop != null)
                obj[fieldName] = "";
        }

        private static string BuildRaceAssignmentScript(string compressedBase64)
        {
            string escapedBase64 = JavaScriptStringEncode(compressedBase64);

            return $@"
var raceCompressed = ""{escapedBase64}"";

function base64ToUint8Array(base64) {{
    var binary = atob(base64);
    var len = binary.length;
    var bytes = new Uint8Array(len);

    for (var i = 0; i < len; i++) {{
        bytes[i] = binary.charCodeAt(i);
    }}

    return bytes;
}}

function unpackRace(base64) {{
    var compressedBytes = base64ToUint8Array(base64);
    var jsonBytes = window.pako.ungzip(compressedBytes);
    var jsonText = new TextDecoder(""utf-8"").decode(jsonBytes);
    return JSON.parse(jsonText);
}}

var race = unpackRace(raceCompressed);";
        }

        private static string CompressStringToBase64(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            byte[] inputBytes = Encoding.UTF8.GetBytes(text);

            using MemoryStream output = new();
            using (GZipStream gzip = new(output, CompressionLevel.Optimal, leaveOpen: true))
            {
                gzip.Write(inputBytes, 0, inputBytes.Length);
            }

            return Convert.ToBase64String(output.ToArray());
        }

        private static int FindMatchingBrace(string text, int startIndex)
        {
            int depth = 0;
            bool inString = false;
            bool escape = false;

            for (int i = startIndex; i < text.Length; i++)
            {
                char c = text[i];

                if (escape)
                {
                    escape = false;
                    continue;
                }

                if (c == '\\')
                {
                    if (inString)
                        escape = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                    continue;
                }

                if (inString)
                    continue;

                if (c == '{')
                {
                    depth++;
                }
                else if (c == '}')
                {
                    depth--;
                    if (depth == 0)
                        return i;
                }
            }

            return -1;
        }

        private static string JavaScriptStringEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }
    }
}