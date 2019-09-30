using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data
{
    public interface IWriteWaifus
    {
        Task AddNewWaifu(Waifu waifuToAdd);
    }
}
