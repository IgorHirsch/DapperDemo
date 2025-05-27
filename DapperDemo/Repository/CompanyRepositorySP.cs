using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Repository
{
    public class CompanyRepositorySP : ICompanyRepository
    {
        private IDbConnection db;

        public CompanyRepositorySP(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Add(Company company)
        {

            //var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
            //            + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            //var id = db.Query<int>(sql, company).Single();
            //company.CompanyId = id;

            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", 0, DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            this.db.Execute("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
            company.CompanyId = parameters.Get<int>("CompanyId");
            return company;

        }

        public Company Find(int id)
        {
            return db.Query<Company>("usp_GetCompany", new { CompanyId = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public List<Company> GetAll()
        {

            return db.Query<Company>("usp_GetALLCompany", commandType: CommandType.StoredProcedure).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            db.Execute(sql, new { id });
        }

        public Company Update(Company company)
        {
            //var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, " +
            //    "State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            //db.Execute(sql, company);

            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            this.db.Execute("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);


            return company;

        }
    }
}
