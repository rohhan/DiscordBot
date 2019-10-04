using Discord.Commands;
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
    public class ManageUserActivity : IManageUserActivity
    {
        private DiscordBotDbContext _context;
        private IAddUsers _addUsersRepo;
        private IManageGuildUserRelationships _relationshipRepo;

        public ManageUserActivity(DiscordBotDbContext context,
            IAddUsers addUsersRepo,
            IManageGuildUserRelationships relationshipRepo)
        {
            _context = context;
            _addUsersRepo = addUsersRepo;
            _relationshipRepo = relationshipRepo;
        }

        public async Task UpdateUserActivity(SocketCommandContext socketCommandContext, SocketUserMessage socketUserMessage)
        {
            var socketGuildUser = socketCommandContext.Guild.GetUser(socketCommandContext.User.Id);
            
            if (socketGuildUser == null)
            {
                throw new Exception();
            }

            GuildUser userInGuild;
            userInGuild = _context.GuildUsers
                .OrderByDescending(gu => gu.ActionDate)
                .FirstOrDefault(gu => gu.Guild.GuildDiscordId == socketGuildUser.Guild.Id && gu.User.UserDiscordId == socketGuildUser.Id);

            if (userInGuild == null) 
            {
                await AddUserIfMissing(socketGuildUser);
                await AddUserJoiningGuildRelationshipIfMissing(socketGuildUser);

                userInGuild = _context.GuildUsers
                    .OrderByDescending(gu => gu.ActionDate)
                    .FirstOrDefault(gu => gu.Guild.GuildDiscordId == socketGuildUser.Guild.Id && gu.User.UserDiscordId == socketGuildUser.Id);
            }

            userInGuild.DateLastActive = DateTimeOffset.Now;

            await _context.SaveChangesAsync();
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
