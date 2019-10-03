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
            // Add user to db if it doesn't already exist
            var userAlreadyExists = _context.Users.Any(u => u.UserDiscordId == socketGuildUser.Id);

            if (userAlreadyExists)
            {
                return false;
            }

            var user = Map(socketGuildUser);

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            // Save Guild User Relationship
            var relationship = await CreateGuildUserRelationship(socketGuildUser, GuildUserActionEnum.Joined, socketGuildUser.JoinedAt);

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task RemoveUserFromGuild(SocketGuildUser socketGuildUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserDiscordId == socketGuildUser.Id);

            // Add retroactive history if necessary
            if (user == null)
            {
                await AddNewUser(socketGuildUser);
            }

            // We add a new guild relationship to reflect that the user left
            // We don't remove the user from the master list of users
            var relationship = await CreateGuildUserRelationship(socketGuildUser, GuildUserActionEnum.Left, DateTimeOffset.Now);

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();
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

        private async Task<GuildUser> CreateGuildUserRelationship(SocketGuildUser socketGuildUser, GuildUserActionEnum actionType, DateTimeOffset? actionTime)
        {
            var guild = await _context.Guilds.FirstOrDefaultAsync(g => g.GuildDiscordId == socketGuildUser.Guild.Id);

            if (guild == null)
            {
                throw new Exception();
            }

            var relationship = new GuildUser()
            {
                Guild = guild,
                User = Map(socketGuildUser),
                ActionType = actionType,
                ActionDate = actionTime
            };

            return relationship;
        }
    }
}
