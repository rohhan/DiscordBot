using Discord.Commands;
using DiscordBot.Data.Guilds;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private IManageGuilds _guildRepo;

        public AdminModule(IManageGuilds guildRepo)
        {
            _guildRepo = guildRepo;
        }

        [Command("Init")]
        public async Task InitializeGuildAsync()
        {
            var guild = Context.Guild;

            var successfullyAddedGuild = await _guildRepo.AddNewGuild(guild);

            await ReplyAsync("Successfully initialized new guild!");

            if (successfullyAddedGuild)
            {
                var numberOfNewUsers = await _guildRepo.UpdateGuildUsers(guild);
                await ReplyAsync($"Successfully added {numberOfNewUsers} new users to the guild!");
            }
        }

        [Command("Update users")]
        public async Task UpdateUsersAsync()
        {
            var guild = Context.Guild;

            var numberOfNewUsers = await _guildRepo.UpdateGuildUsers(guild);

            await ReplyAsync($"Successfully added {numberOfNewUsers} new users to the guild!");
        }
    }
}
