using ErciyesSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Common.Models.RequestModels
{
    public class LoginUserCommand : IRequest<LoginUserViewModel>
    {
        //email ve password alındığında geriye loginuserviewmodel döner
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public LoginUserCommand(string emailAddress, string password)
        {
            EmailAddress = emailAddress;
            Password = password;
        }

        //normalde kullanılmalı, ancak bu olunca swagger'da şablon olmuyor,
        //bundan dolayı şimdilik yorum satırı yapıldı
        //public LoginUserCommand()
        //{

        //}
    }
}
