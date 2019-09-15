using Discord.Commands;
using DiscordBot.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private IInitializeGuilds _guildRepo;

        public AdminModule(IInitializeGuilds guildRepo)
        {
            _guildRepo = guildRepo;
        }

        [Command("Init")]
        public async Task InitializeGuildAsync()
        {
            var guild = Context.Guild;

            var successfullyAddedGuild = await _guildRepo.AddNewGuild(guild);

            if (successfullyAddedGuild)
            {
                await _guildRepo.SaveGuildUsers(guild);
            }
        }

        [Command("Update users")]
        public async Task UpdateUsersAsync()
        {
            var guild = Context.Guild;

            await _guildRepo.SaveGuildUsers(guild);
        }
    }
}
