using System.ComponentModel.DataAnnotations.Schema;


namespace TestAPI.Model
{
    public class Person : Microsoft.AspNetCore.Mvc.ActionResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FavouriteColour { get; set; }
        public int NumberChildren { get; set; }
        public int Counter { get; private set; }

        public Person() { }

        public Person(int id, string username, string firstName, string lastName, string favouriteColour, int numberChildren, int counter) {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            FavouriteColour = favouriteColour;
            NumberChildren = numberChildren;
            Counter = counter;
            }

        public int IncrementCounter() {
            return Counter++;
        }

        public void Copy(Person person) {
            Username = person.Username;
            FirstName = person.FirstName;
            LastName = person.LastName;
            FavouriteColour = person.FavouriteColour;
            NumberChildren = person.NumberChildren;
            Counter = person.Counter;
        }
    }
}
