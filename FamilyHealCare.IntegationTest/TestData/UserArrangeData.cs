using Business;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealCare.IntegationTest.TestData
{
    class UserArrangeData
    {
        public static async Task InitUserDataAsync(BaseRepository<User> userRepository)
        {
            if (!userRepository.GetAll().Result.Any())
            {
                var userList = new List<User>()
                {
                    new User
                    {
                        Id = 1,
                        FullName = "Alexis Nico"
                    },
                };

                foreach (User user in userList)
                {
                    await userRepository.Create(user);
                }
            }
        }
    }
}
