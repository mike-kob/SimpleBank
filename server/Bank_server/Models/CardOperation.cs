using System;

namespace Bank_server.Models
{
    public class CardOperation
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; }
        public ICard Sender { get; set; }
        public ICard Receiver { get; set; }
        public decimal Sum { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool Success { get;set; }
    }
}
