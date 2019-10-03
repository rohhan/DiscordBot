using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordBot.Data.Guilds
{
    public interface IManageGuilds
    {
        Task<bool> AddNewGuild(SocketGuild guild);

        Task<int> UpdateGuildUsers(SocketGuild guild);
    }
}
