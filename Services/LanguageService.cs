using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class LanguageService
    {
        private ILanguageRepository languageRepository;
        public LanguageService(ILanguageRepository languageRepository) 
        {
            this.languageRepository = languageRepository;
        }
        public void GetAll(List<LanguageDTO> languages)
        {
            languages.Clear();
            foreach (Language language in languageRepository.GetAll()) languages.Add(new LanguageDTO(language));
        }

        public LanguageDTO GetByIdDTO(int id)
        {
            Language language = languageRepository.GetById(id);
            return new LanguageDTO(language);
        }
        public Language GetById(int id) 
        { 
            return languageRepository.GetById(id);
        }
    }
}
