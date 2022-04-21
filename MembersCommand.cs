using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMembersInfo;

public class MembersCommand : IRocketCommand
{
    public string Name { get; } = "members";

    public AllowedCaller AllowedCaller { get; } = AllowedCaller.Both;

    public string Help { get; } = "Shows members of faction(s).";

    public string Syntax => $"/{Name} [name/*]";

    public List<string> Aliases { get; } = new();

    List<string> permissions;
    public List<string> Permissions => permissions ??= new() { Name };

    public async void Execute(IRocketPlayer caller, string[] command)
    {
        var arg = (command.ElementAtOrDefault(0) ?? "").Trim().ToLower();
        if (string.IsNullOrWhiteSpace(arg) || arg == "*" || arg == "all")
        {
            var factions = conf.Factions.Select(faction => faction.Format(out _, out _));
            UnturnedChat.Say(caller, TranslateFormatAll());
            foreach (var factionFormatted in factions)
                UnturnedChat.Say(caller, factionFormatted);
            return;
        }

        var faction = conf.Factions.FirstOrDefault(x => x.Name.Equals(arg, StringComparison.OrdinalIgnoreCase));
        if (faction is null)
        {
            UnturnedChat.Say(caller, Syntax);
            return;
        }

        UnturnedChat.Say(caller, TranslateFormat(faction.Format(out var members, out var count)));
        foreach (var memberFormatted in faction.FormatMembers(members))
            UnturnedChat.Say(caller, memberFormatted);
    }
}
