namespace LabChaos.Methods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using Exiled.API.Enums;
    using Exiled.API.Features.Items;

    public class ChaosEvents
    {
        public List<Action> events = new List<Action>();
        Random rand = new Random();

        public ChaosEvents()
        {
            events.Add(StartWarhead); //0
            events.Add(GiveMicroToRandHum); //1
            events.Add(GiveDClassCom15); //2
            events.Add(ClearEveryoneInv); //3
            //events.Add(TurnSpectatorsToZombie); //4
            events.Add(Contain106); //5
            events.Add(ShrinkPlayers); //6
            //events.Add(SwapDClassAndSec); //7
            events.Add(GiveCoinToEveryone); //8
            events.Add(SwapTwoPlayersInv); //9
            events.Add(GiveBlackCardToRandHum); //10
            events.Add(TeleportRandPlayerToPocketDim); //11
            events.Add(TurnOnFF); //12
            events.Add(GiveColaEffectToEveryone); //13
            events.Add(GiveRandomEffectToEveryone); //14
            //events.Add(TeleportRandHumToIntercom); //15
            events.Add(KillHalfPlayers); //16
            events.Add(RespawnPlayerAsRandScp); //17
            events.Add(ExpandPlayers); //18
        }

        private void BroadcastAllPlayers(string message)
        {
            foreach (Player p in Player.List)
            {
                p.Broadcast(new Broadcast(message), true);
            }
        }

        private void StartWarhead()
        {
            Warhead.Start();
            BroadcastAllPlayers("Boom boom time");
        }

        private void GiveMicroToRandHum()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));
            player.AddItem(ItemType.MicroHID, 1);

            BroadcastAllPlayers("Given MicroHid to a random human!");
        }

        private void GiveDClassCom15()
        {
            var players = Player.List.Where(p => p.Role == RoleType.ClassD);

            foreach (Player p in players)
            {
                p.AddItem(ItemType.GunCOM15, 1);
                p.AddAmmo(AmmoType.Nato9, 30);
            }

            BroadcastAllPlayers("D bois revolution!");
        }

        private void ClearEveryoneInv()
        {
            foreach (Player p in Player.List)
            {
                if (p.IsHuman)
                {
                    p.ClearInventory();
                }
            }

            BroadcastAllPlayers("Where's your items, lads?");
        }

        private void TurnSpectatorsToZombie()
        {
            var spectators = Player.List.Where(p => p.Role == RoleType.Spectator);

            foreach (Player p in spectators)
            {
                p.SetRole(RoleType.Scp0492);
                p.Position = Room.Get(RoomType.Hcz049).Position;
            }

            BroadcastAllPlayers("Walking dead!");
        }

        private void Contain106()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));
            player.Position = Scp106Container.Position;
            var scp106 = Player.List.Where(p => p.Role == RoleType.Scp106);

            if (scp106 != null)
            {
                foreach (Player p in scp106)
                {
                    p.Role.As<Scp106Role>().Contain(player);
                }
            }

            BroadcastAllPlayers("Someone going to scream tonight... It's RE:containment time!");
        }

        private void ShrinkPlayers()
        {
            var players = Player.List.Where(p => p.IsAlive);

            foreach(Player p in players)
            {
                p.Scale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
            }

            BroadcastAllPlayers("It's shrinkin' time!");
        }

        private void SwapDClassAndSec()
        {
            var dClass = Player.Get(RoleType.ClassD);
            var guards = Player.Get(RoleType.FacilityGuard);

            foreach(Player p in guards) //TODO: fix
            {
                p.SetRole(RoleType.ClassD, SpawnReason.None, true);
            }
            foreach (Player p in dClass)
            {
                p.SetRole(RoleType.FacilityGuard, SpawnReason.None, true);
            }

            BroadcastAllPlayers("We do a little amount of team swapping...");
        }

        private void GiveCoinToEveryone()
        {
            var players = Player.List.Where(p => p.IsHuman);
            foreach (Player p in players)
            {
                int emptySlots = InventorySystem.Inventory.MaxSlots - p.Items.Count();
                if (emptySlots == 0)
                {
                    p.RemoveItem(p.Items.ElementAt(rand.Next(p.Items.Count())));
                    emptySlots++;
                }
                p.AddItem(ItemType.Coin, emptySlots);
            }

            BroadcastAllPlayers("Here comes the money!");
        }

        private void SwapTwoPlayersInv()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player p1 = players.ElementAt(rand.Next(players.Count()));

            var playersEx = players.Where(p => !p.Equals(p1) && p.IsHuman);
            Player p2 = playersEx.ElementAt(rand.Next(playersEx.Count()));
            
            List<Item> p1TempItems = new List<Item>();
            List<Item> p2TempItems = new List<Item>();

            foreach (Item item in p1.Items)
            {
                p1TempItems.Add(item);
            }

            foreach (Item item in p2.Items)
            {
                p2TempItems.Add(item);
            }

            p2.ClearInventory(false);
            p2.AddItem(p1TempItems);

            p1.ClearInventory(false);
            p1.AddItem(p2TempItems);

            BroadcastAllPlayers("Two players has swapped their inventories! But who are they?...");
        }

        private void GiveBlackCardToRandHum()
        {
            Player player = Player.List.ElementAt(rand.Next(Player.List.Count()));
            player.AddItem(ItemType.KeycardO5);

            BroadcastAllPlayers("Someone got a V.I.P access!");
        }

        private void TeleportRandPlayerToPocketDim()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));
            player.Position = new UnityEngine.Vector3(0f, -1998.5f, 0f);

            foreach (Player p in Player.List)
            {
                if (p.Equals(player))
                {
                    player.Broadcast(new Broadcast("Welcome to Brazil!"));
                    continue;
                }

                p.Broadcast(new Broadcast("Someone got sent to Brazil!"), true);
            }
        }

        private void TurnOnFF()
        {
            Server.FriendlyFire = true;

            BroadcastAllPlayers("Friendly fire is on!");
        }

        private void GiveColaEffectToEveryone()
        {
            var players = Player.List.Where(p => p.IsAlive);

            foreach(Player p in players)
            {
                p.EnableEffect(EffectType.Scp207, 60);
            }

            BroadcastAllPlayers("I'm fast as fuck boiiii");
        }

        private void GiveRandomEffectToEveryone()
        {
            var players = Player.List.Where(p => p.IsAlive);
            
            foreach(Player p in players)
            {
                p.ApplyRandomEffect(30f, true);
            }

            BroadcastAllPlayers("Everyone got a random effect for 30 seconds!");
        }

        private void TeleportRandHumToIntercom()
        {
            var humans = Player.List.Where(p => p.IsHuman);
            Player player = humans.ElementAt(rand.Next(humans.Count()));

            player.Position = Room.Get(RoomType.EzIntercom).Position;
            player.Position = new UnityEngine.Vector3(-18.5f, -1007.6f, 85.5f);

            BroadcastAllPlayers("Someone is locked in the intercom!");
        }

        private void KillHalfPlayers()
        {
            var players = Player.List.Where(p => p.IsAlive);
            List<Player> playerL = players.ToList();

            for (int i = 0; i < players.Count()/2; i++)
            {
                int k = rand.Next(playerL.Count());
                playerL[k].Kill(DamageType.Unknown);
                playerL.Remove(playerL[k]);
            }

            BroadcastAllPlayers("Thanos has snaped his fingers!");
        }

        private void RespawnPlayerAsRandScp()
        {
            RoleType[] scps = new RoleType[] { RoleType.Scp049, RoleType.Scp096, RoleType.Scp106, RoleType.Scp173, RoleType.Scp93953, RoleType.Scp93989 };

            var players = Player.List.Where(p => p.IsHuman || p.Role == RoleType.Spectator);
            Player player = players.ElementAt(rand.Next(players.Count()));
            player.SetRole(scps[rand.Next(scps.Count())]);

            BroadcastAllPlayers("A random scp has been freed...");
        }

        private void ExpandPlayers()
        {
            var players = Player.List.Where(p => p.IsAlive);

            foreach (Player p in players)
            {
                p.Scale = new UnityEngine.Vector3(1.3f, 1.3f, 1.3f);
            }

            BroadcastAllPlayers("It's growin' time!");
        }
    }

}
