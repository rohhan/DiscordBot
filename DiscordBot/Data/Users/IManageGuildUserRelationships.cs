using Discord.WebSocket;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data.Users
{
    public interface IManageGuildUserRelationships
    {
        Task<bool> CreateGuildUserRelationship(SocketGuildUser socketGuildUser, GuildUserActionEnum actionType, DateTimeOffset? actionTime);

        Task<bool> CreateGuildUserRelationship(SocketUser socketUser, SocketGuild socketGuild, GuildUserActionEnum actionType, DateTimeOffset? actionTime);
    }
}
