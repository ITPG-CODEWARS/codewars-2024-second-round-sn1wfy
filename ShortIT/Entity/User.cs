using System.ComponentModel.DataAnnotations;
using ShortIT.ViewModel;

namespace ShortIT.Entity
{
	public class User
	{
		[Key]
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool IsAdmin { get; set; }

		public User()
		{
			Id = Guid.NewGuid();
		}

		/// <summary>
		/// Constructor for register
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public User(MainUserVM model)
		{
			Id = Guid.NewGuid();
			Username = model.Username;
			Password = model.Password;
			IsAdmin = false;
		}

	}
}
