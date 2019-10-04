using Discord.WebSocket;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data.Users
{
    public class LogBans : ILogBans
    {
        private DiscordBotDbContext _context;
        private IAddUsers _addUsersRepo;
        private IManageGuildUserRelationships _relationshipRepo;

        public LogBans(DiscordBotDbContext context, IAddUsers addUsersRepo, IManageGuildUserRelationships relationshipRepo)
        {
            _context = context;
            _addUsersRepo = addUsersRepo;
            _relationshipRepo = relationshipRepo;
        }

        public async Task LogUserBannedFromGuild(SocketUser socketUser, SocketGuild socketGuild)
        {
            await AddUserIfMissing(socketUser, socketGuild);
            await AddUserJoiningGuildRelationshipIfMissing(socketUser, socketGuild);

            // We add a new guild relationship to reflect that the user left
            // We don't remove the user from the master list of users
            await _relationshipRepo.CreateGuildUserRelationship(socketUser, socketGuild, GuildUserActionEnum.Banned, DateTimeOffset.Now);
        }

        /// <summary>
        /// Adds retroactive user history if it is missing
        /// </summary>
        /// <param name="socketGuildUser"></param>
        /// <returns></returns>
        private async Task AddUserIfMissing(SocketUser socketUser, SocketGuild socketGuild)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserDiscordId == socketUser.Id);

            if (user == null)
            {
                await _addUsersRepo.AddNewUser(socketUser, socketGuild);
            }
        }

        private async Task AddUserJoiningGuildRelationshipIfMissing(SocketUser socketUser, SocketGuild socketGuild)
        {
            var mostRecentGuildUserRelationship = _context.GuildUsers
                .OrderByDescending(gu => gu.ActionDate)
                .FirstOrDefault(gu => gu.Guild.GuildDiscordId == socketGuild.Id && gu.User.UserDiscordId == socketUser.Id);

            if (mostRecentGuildUserRelationship == null || mostRecentGuildUserRelationship.ActionType != GuildUserActionEnum.Joined)
            {
                // Todo: Make a note that the time is inaccurate because 
                // we don't know when a user joined the guild once they have already left
                await _relationshipRepo.CreateGuildUserRelationship(socketUser, socketGuild, GuildUserActionEnum.Joined, DateTimeOffset.Now);
            }
        }
    }
}
