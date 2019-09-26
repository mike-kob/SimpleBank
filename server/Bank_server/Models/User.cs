using System;

namespace Bank_server.Models
{
    public class User
    {
        public User(int id, string firstName, string lastName, DateTime dateBirth, DateTime created, int numOfCards)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateBirth = dateBirth;
            Created = created;
            NumOfCards = numOfCards;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime Created { get; set; }
        //not sure if this property is necessary
        public int NumOfCards { get; set; }
    }
}
