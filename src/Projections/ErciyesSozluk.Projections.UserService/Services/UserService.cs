using Dapper;
using ErciyesSozluk.Common.Events.User;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Projections.UserService.Services
{
    public class UserService
    {
        private readonly string connectionString;

        public UserService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("SqlServer");
        }

        public async Task<Guid> CreateEmailConfirmation(UserEmailChangedEvent @event)
        {
            var guid = Guid.NewGuid();

            //TODO: yani kayıt olan kullanıcının oldemail'i boş gelidiği için burayı düzenle

            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("INSERT INTO emailconfirmation (Id, CreateDate, OldEmailAddress, NewEmailAddress) VALUES (@Id, GETDATE(), @OldEmailAddress, @NewEmailAddress)",
                    new
                    {
                        Id = guid,
                        OldEmailAddress = @event.OldEmailAddress ?? "newuser",
                        NewEmailAddress = @event.NewEmailAddress
                    });

            return guid;
        }
    }
}
