using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDataBase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postgresql database.");

                    var connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var command = new NpgsqlCommand() {Connection = connection})
                        {
                            command.CommandText = "DROP TABLE IF EXISTS Coupon";
                            command.ExecuteNonQuery();

                            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                        ProductName VARCHAR(24) NOT NULL,
                                                        Description TEXT,
                                                        Amount INT)";
                            command.ExecuteNonQuery();

                            command.CommandText =
                                "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                            command.ExecuteNonQuery();

                            command.CommandText =
                                "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                            command.ExecuteNonQuery();

                            logger.LogInformation("Migrated postresql database.");
                        }
                    }
                }
                catch (NpgsqlException e)
                {
                    logger.LogError(e, "An error ocurred while migrating the postgresql database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDataBase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }
    }
}