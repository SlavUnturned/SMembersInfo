global using static SMembersInfo.Faction;

namespace SMembersInfo
{
    public class Faction
    {
        [XmlAttribute]
        public string 
            Name;

        public List<Rank> Ranks = new ();

        public class Rank
        {
            [XmlAttribute]
            public string
                Group, DisplayName;

            public override string ToString() => DisplayName;
        }

        public virtual Rank IsMember(UP up)
        {
            var groups = R.Permissions.GetGroups(up, false);
            return Ranks.FirstOrDefault(x => groups.Any(y => y.Id == x.Group));
        }

        public class Member
        {
            public Member(int id, UnturnedPlayer player, Rank rank)
            {
                Id = id;
                Player = player;
                Rank = rank;
            }

            public int Id { get; }
            public UP Player { get; }
            public Rank Rank { get; }
        }

        public virtual IEnumerable<Member> FindMembers()
        {
            var counter = 0;
            foreach(var up in UPList)
            {
                var rank = IsMember(up);
                if (rank is null)
                    continue;
                yield return new (++counter, up, rank);
            }
        }

        public virtual string Format(out IEnumerable<Member> members, out int count)
        {
            members = FindMembers();
            count = members.Count();
            return TranslateFactionFormat(Name, count, count == 0 ? 0 : (count * 100d) / Provider.clients.Count, 1);
        }
        public virtual IEnumerable<string> FormatMembers(IEnumerable<Member> members) => members.Select(x => TranslateMemberFormat(x.Id, x.Player.CharacterName, x.Rank));
    }

    public partial class Config : IRocketPluginConfiguration
    {
        public List<Faction> Factions = new();
        public void LoadDefaults()
        {
            Factions.Add(new()
            {
                Name = "Police",
                Ranks = new()
                {
                    new() { Group = "police1", DisplayName = "Officer" }
                }
            });
        }
    }
}