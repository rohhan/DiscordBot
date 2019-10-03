using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data.Users
{
    public interface IAddUsers
    {
        Task<bool> AddNewUser(SocketGuildUser socketGuildUser);
    }
}
