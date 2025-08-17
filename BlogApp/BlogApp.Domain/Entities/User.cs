using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Entities
{
	public class User
	{
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }

        public User()
        {
            
        }
        public User(string username,string firstname,string lastname,string email,int roleId)
        {
			if (string.IsNullOrWhiteSpace(username))
				throw new ArgumentException("Kullanıcı Adı boş olamaz", nameof(username));
			if (string.IsNullOrWhiteSpace(firstname))
				throw new ArgumentException("İsim boş olamaz", nameof(firstname));
			if (string.IsNullOrWhiteSpace(lastname))
				throw new ArgumentException("Soyisim  boş olamaz", nameof(lastname));
			if (roleId > 2)
				throw new ArgumentException("RoleId 2 den büyük olamaz", nameof(roleId));

           UserName = username;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            RoleId = roleId;


		}
    }
}
