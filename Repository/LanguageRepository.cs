using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class LanguageRepository
    {
        private const string FilePath = "../../../Resources/Data/languages.csv";

        private readonly Serializer<Language> serializer;

        private List<Language> languages;


        public LanguageRepository()
        {
            serializer = new Serializer<Language>();
            languages = serializer.FromCSV(FilePath);
        }

        public List<Language> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }
        public Language GetById(int id)
        {
            languages=serializer.FromCSV(FilePath);
            return languages.Find(i => i.Id == id);
        }
    }
}
