using Swagger_Demo.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger_Demo.Examples
{
    public class UserListExample : IExamplesProvider<IEnumerable<StoredUser>>
    {
        public IEnumerable<StoredUser> GetExamples()
        {
            return new List<StoredUser>
            {
                new StoredUser
                {
                    Id = "a1",
                    Name = "Alice",
                    Email = "alice@example.com"
                },
                new StoredUser
                {
                    Id = "b2",
                    Name = "Bob",
                    Email = "bob@example.com"
                }
            };
        }
    }
}
