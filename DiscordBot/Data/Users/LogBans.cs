using Discord.WebSocket;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserDiscordId == socketUser.Id);

            // Add retroactive history if necessary
            if (user == null)
            {
                await _addUsersRepo.AddNewUser(socketUser, socketGuild);
            }

            // We add a new guild relationship to reflect that the user left
            // We don't remove the user from the master list of users
            var relationship = await _relationshipRepo.CreateGuildUserRelationship(socketUser, socketGuild, GuildUserActionEnum.Banned, DateTimeOffset.Now);

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();
        }
    }
}
