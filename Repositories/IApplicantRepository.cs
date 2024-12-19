using LamaranWeb.Models;

namespace LamaranWeb.Repositories
{
    public interface IApplicantRepository
    {
        Task<IEnumerable<Applicant>> GetAllAsync();
        Task AddAsync(Applicant applicant);
        Task UpdateAsync(Applicant applicant);
        Task DeleteAsync(int id);
        Task<Applicant> GetByIdAsync(int id);
    }
}
