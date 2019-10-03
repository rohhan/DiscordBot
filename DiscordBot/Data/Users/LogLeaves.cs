using Discord.WebSocket;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserDiscordId == socketGuildUser.Id);

            // Add retroactive history if necessary
            if (user == null)
            {
                await _addUsersRepo.AddNewUser(socketGuildUser);
            }

            // We add a new guild relationship to reflect that the user left
            // We don't remove the user from the master list of users
            var relationship = await _relationshipRepo.CreateGuildUserRelationship(socketGuildUser, GuildUserActionEnum.Left, DateTimeOffset.Now);

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();
        }
    }
}
