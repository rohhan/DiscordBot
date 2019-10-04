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
        Task<GuildUser> CreateGuildUserRelationship(SocketGuildUser socketGuildUser, GuildUserActionEnum actionType, DateTimeOffset? actionTime);

        Task<GuildUser> CreateGuildUserRelationship(SocketUser socketUser, SocketGuild socketGuild, GuildUserActionEnum actionType, DateTimeOffset? actionTime);
    }
}
