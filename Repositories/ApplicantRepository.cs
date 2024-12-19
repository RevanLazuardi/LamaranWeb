using Dapper;
using LamaranWeb.Models;
using Microsoft.Data.SqlClient;

namespace LamaranWeb.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly string _connectionString;

        public ApplicantRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Applicant>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Applicant>(
                "SELECT Id, NamaLengkap, Email, NoHp, CV AS CVName, CreatedAt FROM TblPelamar"
            );
        }

        public async Task AddAsync(Applicant applicant)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "INSERT INTO TblPelamar (NamaLengkap, Email, NoHp, CV, CreatedAt) VALUES (@NamaLengkap, @Email, @NoHp, @CVName, @CreatedAt)";
            await connection.ExecuteAsync(query, applicant);
        }

        public async Task UpdateAsync(Applicant applicant)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "UPDATE TblPelamar SET NamaLengkap = @NamaLengkap, Email = @Email, NoHp = @NoHp, CV = @CVName WHERE Id = @Id";
            await connection.ExecuteAsync(query, applicant);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM TblPelamar WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<Applicant> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Applicant>(
                "SELECT Id, NamaLengkap, Email, NoHp, CV AS CVName, CreatedAt FROM TblPelamar WHERE Id = @Id",
                new { Id = id }
            );
        }
    }
}
