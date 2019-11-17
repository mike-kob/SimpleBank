using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BankServer.Data;
using BankServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AddCardUtil
{
    class Program
    {
        private static DbContextOptionsBuilder<BankServerContext> _optionsBuilder;
        
        static void Main(string[] args)
        {
            _optionsBuilder = new DbContextOptionsBuilder<BankServerContext>();
            _optionsBuilder.UseSqlServer("Server=MICHAEL-PC\\SQLEXPRESS;Database=bankDB_3;Trusted_Connection=True;MultipleActiveResultSets=true");

            using (var context = new BankServerContext(_optionsBuilder.Options))
            {
                User user = GetUser(context);
                if (user == null) 
                    return;

                Card card = GetCard(user, context);

                context.Card.Add(card);
                context.SaveChanges();
            }
        }

        private static User GetUser(BankServerContext context)
        {
            Console.Write("UserID: ");
            try
            {
                int userId = Int32.Parse(Console.ReadLine());
                User user = context.User.FirstOrDefault(user => user.UserId == userId);
                Console.WriteLine(user.FirstName + " " + user.LastName + " " + user.DateBirth);
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("User not found.");
                return null;
            }
        }

        private static Card GetCard(User user, BankServerContext context)
        { 
            try
            {
                Console.Write("New card number: ");
                string cardNum = Console.ReadLine();
                if (!cardNum.All(char.IsDigit) || cardNum.Length < 4)
                {
                    Console.WriteLine("Invalid card number. Must be only numbers and no less than 4 digits.");
                    return null;
                }
                if(context.Card.FirstOrDefault(card => card.CardNum == cardNum) != null)
                {
                    Console.WriteLine("Card already exists.");
                    return null;
                }
                Console.Write("Enter balance: ");
                int balance = Int32.Parse(Console.ReadLine());

                Console.Write("Enter pin: ");
                string pin = Console.ReadLine();
                if (!pin.All(char.IsDigit) || pin.Length != 4)
                {
                    Console.WriteLine("Invalid pin. Must be only numbers and 4 digits.");
                    return null;
                }

                Console.Write("Enter card type (checking, credit, deposit): ");
                string type = Console.ReadLine();
                switch (type)
                {
                    case "checking":
                        CheckingCard card = new CheckingCard();
                        card.CardNum = cardNum;
                        card.CardUser = user;
                        card.UserId = user.UserId;
                        card.DateCreated = DateTime.Now;
                        card.Balance = balance;
                        card.Pin = ComputeSha256Hash(pin);
                        return card;
                    case "credit":
                        CreditCard crCard = new CreditCard();
                        crCard.CardNum = cardNum;
                        crCard.CardUser = user;
                        crCard.UserId = user.UserId;
                        crCard.DateCreated = DateTime.Now;
                        crCard.OwnMoney = balance;
                        Console.Write("Enter credit limit: ");
                        int limit = Int32.Parse(Console.ReadLine());
                        crCard.Limit = limit;
                        crCard.Pin = ComputeSha256Hash(pin);
                        return crCard;
                    case "deposit":
                        DepositCard dpCard = new DepositCard();
                        dpCard.CardNum = cardNum;
                        dpCard.CardUser = user;
                        dpCard.UserId = user.UserId;
                        dpCard.DateCreated = DateTime.Now;
                        dpCard.Balance = balance;
                        dpCard.Pin = ComputeSha256Hash(pin);
                        return dpCard;
                    default:
                        Console.Write("Not found");
                        return null;
                }



            }
            catch (Exception e)
            {
                Console.WriteLine("Error.");
                return null;
            }
        }


        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
