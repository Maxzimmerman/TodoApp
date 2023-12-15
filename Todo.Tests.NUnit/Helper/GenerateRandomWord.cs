using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Tests.NUnit.Helper
{
    public static class GenerateRandomWord
    {
        private static string _word;

        public static string Word
        {
            get { return _word; }
            set { _word = value; }
        }

        public static void GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            var randomString = new System.Text.StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                randomString.Append(chars[index]);
            }

            Word = randomString.ToString();
        }
    }
}
