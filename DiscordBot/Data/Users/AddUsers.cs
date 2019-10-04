using Discord.WebSocket;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data.Users
{
    public class AddUsers : IAddUsers
    {
        private DiscordBotDbContext _context;
        private IManageGuildUserRelationships _relationshipRepo;

        public AddUsers(DiscordBotDbContext context, IManageGuildUserRelationships relationshipRepo)
        {
            _context = context;
            _relationshipRepo = relationshipRepo;
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
            var relationship = await _relationshipRepo.CreateGuildUserRelationship(socketGuildUser, GuildUserActionEnum.Joined, socketGuildUser.JoinedAt);

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddNewUser(SocketUser socketUser, SocketGuild socketGuild)
        {
            // Add user to db if it doesn't already exist
            var userAlreadyExists = _context.Users.Any(u => u.UserDiscordId == socketUser.Id);

            if (userAlreadyExists)
            {
                return false;
            }

            var user = Map(socketUser);

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            // Save Guild User Relationship
            // Todo: Make a note that the time is inaccurate because 
            // we don't know when a user joined the guild once they have already left
            var relationship = await _relationshipRepo.CreateGuildUserRelationship(socketUser, socketGuild, GuildUserActionEnum.Joined, DateTimeOffset.Now);

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();

            return true;
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

        private User Map(SocketUser socketUser)
        {
            return new User()
            {
                UserDiscordId = socketUser.Id,
                DiscriminatorValue = socketUser.DiscriminatorValue,
                Username = socketUser.Username,
                IsBot = socketUser.IsBot
            };
        }
    }
}
