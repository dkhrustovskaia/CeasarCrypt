using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CeasarCrypt.Models
{
    public class WordDatabase : DbContext
    {
        public DbSet<Word> Words { get; set; }

        public WordDatabase() : base("WordDatabase") { }

        public bool Contains(string word)
        {
            return Words.FirstOrDefault(realWord => realWord.Value == word) != null;
        }
    }
}