namespace LabChaos.Methods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PlayerRoles;
    using Exiled.API.Features;
    using Exiled.API.Enums;
    using Exiled.API.Features.Items;

    public class ChaosEvents
    {
        public List<Action> events = new List<Action>();
        Random rand = new Random();

        public ChaosEvents()
        {
            events.Add(StartWarhead);
            events.Add(GiveMicroToRandHum);
            events.Add(GiveDClassCom15);
            events.Add(ClearEveryoneInv);
            events.Add(TurnSpectatorsToZombie);
            //events.Add(Contain106);
            events.Add(ShrinkPlayers);
            events.Add(SwapDClassAndSec);
            events.Add(GiveCoinToEveryone);
            events.Add(SwapTwoPlayersInv);
            events.Add(GiveBlackCardToRandHum);
            events.Add(TeleportRandPlayerToPocketDim);
            events.Add(TurnOnFF);
            events.Add(GiveColaEffectToEveryone);
            events.Add(GiveRandomEffectToEveryone);
            events.Add(TeleportRandHumToIntercom);
            events.Add(KillHalfPlayers);
            events.Add(RespawnPlayerAsRandScp);
            events.Add(RespawnHalfAsMTFHalfAsCI);
            //add: give triple glock, give jailbird, give lasergun, give 1576, tp to 106 room
        }

        private void BroadcastToAllPlayers(string message)
        {
            foreach (Player p in Player.List)
            {
                p.Broadcast(new Broadcast(message), true);
            }
        }

        private void RemoveRandomItem(Player player)
        {
            player.RemoveItem(player.Items.ElementAt(rand.Next(player.Items.Count())));
        }

        private void StartWarhead()
        {
            Warhead.Start();
            BroadcastToAllPlayers("Boom boom time");
        }

        private void GiveMicroToRandHum()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));

            if (InventorySystem.Inventory.MaxSlots - player.Items.Count() == 0)
            {
                RemoveRandomItem(player);
            }

            player.AddItem(ItemType.MicroHID, 1);

            BroadcastToAllPlayers("Given MicroHid to a random human!");
        }

        private void GiveDClassCom15()
        {
            var players = Player.Get(RoleTypeId.ClassD);

            foreach (Player p in players)
            {
                p.AddItem(ItemType.GunCOM15, 1);
                p.AddAmmo(AmmoType.Nato9, 30);
            }

            BroadcastToAllPlayers("D bois revolution!");
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

            BroadcastToAllPlayers("Where's your items, lads?");
        }

        private void TurnSpectatorsToZombie()
        {
            var spectators = Player.Get(RoleTypeId.Spectator);

            foreach (Player p in spectators)
            {
                p.Position = new UnityEngine.Vector3(165.9f, 993.8f, -57f);
                p.Role.Set(RoleTypeId.Scp0492, SpawnReason.Revived);
            }

            BroadcastToAllPlayers("Walking dead!");
        }

        //private void Contain106()
        //{
        //    var players = Player.List.Where(p => p.IsHuman);
        //    Player player = players.ElementAt(rand.Next(players.Count()));
        //    player.Position = Scp106Container.Position;
        //    var scp106 = Player.Get(RoleTypeId.Scp106);

        //    if (scp106 != null)
        //    {
        //        foreach (Player p in scp106)
        //        {
        //            p.Role.As<Scp106Role>().Contain(player);
        //        }
        //    }

        //    BroadcastToAllPlayers("Someone going to scream tonight... It's RE:containment time!");
        //}

        private void ShrinkPlayers()
        {
            var players = Player.List.Where(p => p.IsAlive);

            foreach(Player p in players)
            {
                p.Scale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
            }

            BroadcastToAllPlayers("It's shrinkin' time!");
        }

        private void SwapDClassAndSec()
        {
            List<Player> dClass = new List<Player>();
            foreach (Player p in Player.Get(RoleTypeId.ClassD))
                dClass.Add(p);

            List<Player> guards = new List<Player>();
            foreach (Player p in Player.Get(RoleTypeId.FacilityGuard))
                guards.Add(p);

            foreach (Player p in guards)
                p.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.None);

            foreach (Player p in dClass)
                p.Role.Set(RoleTypeId.FacilityGuard, RoleSpawnFlags.None);

            BroadcastToAllPlayers("We do a little amount of team swapping...");
        }

        private void GiveCoinToEveryone()
        {
            var players = Player.List.Where(p => p.IsHuman);
            foreach (Player p in players)
            {
                int emptySlots = InventorySystem.Inventory.MaxSlots - p.Items.Count();
                if (emptySlots == 0)
                {
                    RemoveRandomItem(p);
                    emptySlots++;
                }
                p.AddItem(ItemType.Coin, emptySlots);
            }

            BroadcastToAllPlayers("Here comes the money!");
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

            BroadcastToAllPlayers("Two players has swapped their inventories! But who are they?...");
        }

        private void GiveBlackCardToRandHum()
        {
            Player player = Player.List.ElementAt(rand.Next(Player.List.Count()));

            if (InventorySystem.Inventory.MaxSlots - player.Items.Count() == 0)
            {
                RemoveRandomItem(player);
            }

            player.AddItem(ItemType.KeycardO5);

            BroadcastToAllPlayers("Someone got a V.I.P access!");
        }

        private void TeleportRandPlayerToPocketDim()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));
            player.Position = new UnityEngine.Vector3(0f, -1998.5f, 0f);
            player.EnableEffect(EffectType.Corroding);

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

            BroadcastToAllPlayers("Friendly fire is on!");
        }

        private void GiveColaEffectToEveryone()
        {
            var players = Player.List.Where(p => p.IsAlive);

            foreach(Player p in players)
            {
                p.EnableEffect(EffectType.Scp207, 60);
            }

            BroadcastToAllPlayers("I'm fast as fuck boiiii");
        }

        private void GiveRandomEffectToEveryone()
        {
            var players = Player.List.Where(p => p.IsAlive);
            
            foreach(Player p in players)
            {
                p.ApplyRandomEffect(EffectCategory.None, 30f, true);
            }

            BroadcastToAllPlayers("Everyone got a random effect for 30 seconds!");
        }

        private void TeleportRandHumToIntercom()
        {
            var humans = Player.List.Where(p => p.IsHuman);
            Player player = humans.ElementAt(rand.Next(humans.Count()));

            UnityEngine.Vector3 intercom = Room.Get(RoomType.EzIntercom).Position;
            player.Position = new UnityEngine.Vector3(intercom.x, intercom.y - 6f, intercom.z);

            BroadcastToAllPlayers("Someone is locked in the intercom!");
        }

        private void KillHalfPlayers()
        {
            var players = Player.List.Where(p => p.IsAlive);
            float halfPlayersCount = players.Count() / 2f;

            Log.Info(halfPlayersCount);
            for (int i = 0; i < halfPlayersCount; i++)
            {
                Log.Info(halfPlayersCount);
                int k = rand.Next(players.Count());
                players.ElementAt(k).Kill(DamageType.Unknown);
            }

            BroadcastToAllPlayers("Thanos has snaped his fingers!");
        }

        private void RespawnPlayerAsRandScp()
        {
            RoleTypeId[] scps = new RoleTypeId[] { RoleTypeId.Scp049, RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939};

            var players = Player.List.Where(p => p.IsHuman || p.Role == RoleTypeId.Spectator);
            Player player = players.ElementAt(rand.Next(players.Count()));
            player.Role.Set(scps[rand.Next(scps.Count())]);

            BroadcastToAllPlayers("A random scp has been freed...");
        }

        private void RespawnHalfAsMTFHalfAsCI()
        {
            List<Player> spectators = new List<Player>();
            foreach (Player p in Player.Get(RoleTypeId.Spectator))
                spectators.Add(p);

            for (int i = 0; i < spectators.Count; i++)
            {
                if (i % 2 == 0)
                {
                    spectators.ElementAt(i).Role.Set(RoleTypeId.NtfSergeant);
                    continue;
                }
                spectators.ElementAt(i).Role.Set(RoleTypeId.ChaosRifleman);
            }

            BroadcastToAllPlayers("Dead players has been respawned and divided in two opposing teams...");
        }

    }
}
