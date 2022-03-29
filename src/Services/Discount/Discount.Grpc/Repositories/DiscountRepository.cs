using System.Threading.Tasks;
using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");

            await using var connection = new NpgsqlConnection(connectionString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE PRODUCTNAME = @ProductName", new {ProductName = productName});

            if (coupon is null)
            {
                coupon = new Coupon()
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No discount Desc"
                };
            }

            return coupon;
        }

        public async Task<bool> Create(Coupon coupon)
        {
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");

            await using var connection = new NpgsqlConnection(connectionString);

            var affected = await connection.ExecuteAsync
            ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new {coupon.ProductName, coupon.Description, coupon.Amount});

            return affected == 0;
        }

        public async Task<bool> Update(Coupon coupon)
        {
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");

            await using var connection = new NpgsqlConnection(connectionString);

            var affected = await connection.ExecuteAsync
            ("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE id = @Id",
                new {coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id});

            return affected == 0;        }

        public async Task<bool> Delete(string productName)
        {
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");

            await using var connection = new NpgsqlConnection(connectionString);

            var affected = await connection.ExecuteAsync
            ("DELETE FROM Coupon ProductName = @ProductName",
                new {productName});

            return affected == 0;
        }
    }
}