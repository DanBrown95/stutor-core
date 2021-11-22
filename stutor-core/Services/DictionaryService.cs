using stutor_core.Repositories.Interfaces;
using stutor_core.Services.Interfaces;

namespace stutor_core.Services
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IDictionaryRepository _repo;

        public DictionaryService(IDictionaryRepository repo)
        {
            _repo = repo;
        }
        public string GetRandomWord()
        {
            return _repo.GetRandomWord();
        }
    }
}
