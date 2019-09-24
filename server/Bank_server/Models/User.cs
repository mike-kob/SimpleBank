using System;

namespace Bank_server.Models
{
    public class User
    {
        public User(int id, string firstName, string lastName, DateTime dateBirth, int numOfCards)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateBirth = dateBirth;
            NumOfCards = numOfCards;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public int NumOfCards { get; set; }
    }
}
