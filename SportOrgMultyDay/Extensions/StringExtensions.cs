namespace SportOrgMultyDay.Extensions
{
    public static class StringExtensions
    {
        // Мягкие разрывы строк: \v (Shift+Enter, вставка из Word/Excel/сайтов), \f, NEL, LS, PS
        private static readonly char[] SoftLineBreaks = [(char)0x0B, (char)0x0C, (char)0x85, (char)0x2028, (char)0x2029];

        /// <summary>
        /// Приводит все виды переносов строк к "\n". RichTextBox отображает как перенос не только \r\n,
        /// но и "мягкие" разрывы, которые не делят текст по '\n' и склеивают строки списка в одну.
        /// </summary>
        public static string NormalizeLineBreaks(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text ?? string.Empty;

            text = text.Replace("\r\n", "\n").Replace('\r', '\n');
            foreach (char softBreak in SoftLineBreaks)
                text = text.Replace(softBreak, '\n');
            return text;
        }
    }
}
