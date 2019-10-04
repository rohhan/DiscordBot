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
    public class LogLeaves : ILogLeaves
    {
        private DiscordBotDbContext _context;
        private IAddUsers _addUsersRepo;
        private IManageGuildUserRelationships _relationshipRepo;

        public LogLeaves(DiscordBotDbContext context, IAddUsers addUsersRepo, IManageGuildUserRelationships relationshipRepo)
        {
            _context = context;
            _addUsersRepo = addUsersRepo;
            _relationshipRepo = relationshipRepo;
        }

        public async Task LogUserLeftGuild(SocketGuildUser socketGuildUser)
        {
            await AddUserIfMissing(socketGuildUser);
            await AddUserJoiningGuildRelationshipIfMissing(socketGuildUser);

            // We add a new guild relationship to reflect that the user left
            // We don't remove the user from the master list of users
            await _relationshipRepo.CreateGuildUserRelationship(socketGuildUser, GuildUserActionEnum.Left, DateTimeOffset.Now);
        }

        /// <summary>
        /// Adds retroactive user history if it is missing
        /// </summary>
        /// <param name="socketGuildUser"></param>
        /// <returns></returns>
        private async Task AddUserIfMissing(SocketGuildUser socketGuildUser)
        {
            var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserDiscordId == socketGuildUser.Id);

            if (user == null)
            {
                await _addUsersRepo.AddNewUser(socketGuildUser);
            }
        }

        private async Task AddUserJoiningGuildRelationshipIfMissing(SocketGuildUser socketGuildUser)
        {
            var mostRecentGuildUserRelationship = _context.GuildUsers
                .OrderByDescending(gu => gu.ActionDate)
                .FirstOrDefault(gu => gu.Guild.GuildDiscordId == socketGuildUser.Guild.Id && gu.User.UserDiscordId == socketGuildUser.Id);

            if (mostRecentGuildUserRelationship == null || mostRecentGuildUserRelationship.ActionType != GuildUserActionEnum.Joined)
            {
                await _relationshipRepo.CreateGuildUserRelationship(socketGuildUser, GuildUserActionEnum.Joined, socketGuildUser.JoinedAt);
            }
        }
    }
}
