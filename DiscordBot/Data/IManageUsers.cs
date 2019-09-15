using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data
{
    public interface IManageUsers
    {
        Task UpdateUserActivity(SocketCommandContext socketCommandContext, SocketUserMessage socketUserMessage);

        Task<bool> AddNewUser(SocketGuildUser socketGuildUser);

        Task RemoveUser(SocketGuildUser socketGuildUser);
    }
}
