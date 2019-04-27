using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CeasarCrypt.Models;

namespace CeasarCrypt.Functional
{
    public static class Crypt
    {
        private static WordDatabase database = new WordDatabase();

        private static string lowerAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private static string upperAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private static string fullAlphabet = lowerAlphabet + upperAlphabet + " ";
        private static string vowels = "аеёиоуюэя";
        private static string oneLetter = "акубовияс";
        private static Random rnd = new Random();

        public static string Encryption(int key, string text)
        {
            return Shift(key, text);
        }

        public static string Decryption(int key, string text)
        {
            return Shift(-key, text);
        }

        public static int[] GetKeys(string text)
        {
            string rusText = "";
            foreach (var item in text)
            {
                if (fullAlphabet.Contains(item))
                {
                    rusText += item;
                }
            }

            rusText = rusText.ToLower();

            string[] words = rusText.Split(' ');
            Key[] keys = new Key[33];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = new Key() { Number = i, Priority = 0 };
                foreach (var word in words.Select(x => Shift(-i, x)))
                {
                    if (word.StartsWith("ъ") || word.StartsWith("ы") || word.StartsWith("ь"))
                    {
                        keys[i].Priority -= 100;
                        break;
                    }

                    if (word.Length == 1)
                    {
                        if (word.Contains(oneLetter))
                            keys[i].Priority++;
                        else
                            keys[i].Priority -= 5;
                    }
                    else if (word.Length > 4 && rnd.Next(3) == 0 && database.Contains(word))
                    {
                        keys[i].Priority += 10;
                    }
                }
            }



            return keys.OrderByDescending(x => x.Priority).Select(x => x.Number).ToArray();
        }

        public static string Shift(int shift, string s)
        {
            var result = new StringBuilder();
            shift %= 33;
            if (shift < 0)
            {
                shift += 33;
            }


            for (int i = 0; i < s.Length; i++)
            {
                int index = lowerAlphabet.IndexOf(s[i]);
                if (index > -1)
                {
                    index += shift;
                    if (index > 32)
                        index -= 33;
                    result.Append(lowerAlphabet[index]);
                }
                else
                {
                    index = upperAlphabet.IndexOf(s[i]);

                    if (index > -1)
                    {
                        index += shift;
                        if (index > 32)
                            index -= 33;
                        result.Append(upperAlphabet[index]);
                    }
                    else
                    {
                        result.Append(s[i]);
                    }
                }
            }

            return result.ToString();
        }
    }
}