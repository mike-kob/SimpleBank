using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bank_server.Models
{
    [Table("User", Schema = "dbo")]
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
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTime DateBirth { get; set; }
        [Required]
        public DateTime Created { get; set; }
        //not sure if this property is necessary
        [Required]
        public int NumOfCards { get; set; }
    }
}
