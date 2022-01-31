using System;
using System.IO;

namespace TaxEz.Models
{
    public class User
    {
        public string Username { get; set; }
        public string State { get; set; }
        public string UserDataFile
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Username}.txt");
            }
        }
        public string UserIncomeFile
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Username}_Income.txt");
            }
        }
    }
}
