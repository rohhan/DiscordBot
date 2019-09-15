using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data
{
    public class ManageUsers : IManageUsers
    {
        private DiscordBotDbContext _context;

        public ManageUsers(DiscordBotDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewUser(SocketGuildUser socketGuildUser)
        {
            // Check to see if user already exists
            var userAlreadyExists = _context.Users.Any(u => u.UserDiscordId == socketGuildUser.Id);

            var user = Map(socketGuildUser);

            // Add user to db if it doesn't exist
            if (userAlreadyExists)
            {
                return false;
            }

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            // Save Guild User Relationship
            var guild = await _context.Guilds.FirstOrDefaultAsync(g => g.GuildDiscordId == socketGuildUser.Guild.Id);

            if (guild == null)
            {
                throw new Exception();
            }

            var relationship = new GuildUser()
            {
                Guild = guild,
                User = user,
                ActionType = GuildUserActionEnum.Joined,
                ActionDate = socketGuildUser.JoinedAt
            };

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();

            return true;
        }

        public Task RemoveUser(SocketGuildUser socketGuildUser)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserActivity(SocketCommandContext socketCommandContext, SocketUserMessage socketUserMessage)
        {
            var user = socketCommandContext.User;

            var guild = socketCommandContext.Guild;

            var userInGuild = _context.GuildUsers.FirstOrDefault(gu => gu.Guild.GuildDiscordId == guild.Id && gu.User.UserDiscordId == user.Id);

            if (userInGuild == null) { return; }

            userInGuild.DateLastActive = DateTimeOffset.Now;

            await _context.SaveChangesAsync();
        }

        private User Map(SocketGuildUser socketGuildUser)
        {
            return new User()
            {
                UserDiscordId = socketGuildUser.Id,
                DiscriminatorValue = socketGuildUser.DiscriminatorValue,
                Username = socketGuildUser.Username,
                IsBot = socketGuildUser.IsBot
            };
        }
    }
}
