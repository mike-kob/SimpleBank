using System;

namespace Bank_server.Models
{
    interface ICard
    {
        long CardNum { get; set; }
        string Pin { get; set; }
        DateTime DateCreated { get; set; }
        User CardUser { get; set; }
        decimal Balance { get; }
    }
}
