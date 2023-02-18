namespace LabChaos.Methods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PlayerRoles;
    using Exiled.API.Features;
    using Exiled.API.Enums;
    using Exiled.API.Features.Items;
    using UnityEngine;
    using MEC;

    public class ChaosEvents
    {
        public List<Action> events = new List<Action>();
        System.Random rand = new System.Random();

        public ChaosEvents()
        {
            events.Add(StartWarhead);
            events.Add(GiveMicroToRandHum);
            events.Add(GiveDClassCom15);
            events.Add(ClearEveryoneInv);
            events.Add(TurnSpectatorsToZombie);
            events.Add(ShrinkPlayers);
            events.Add(HumTeamSwapping);
            events.Add(GiveCoinToEveryone);
            events.Add(SwapTwoPlayersInv);
            events.Add(GiveBlackCardToRandHum);
            events.Add(TeleportRandHumToPocketDim);
            events.Add(TurnOnFF);
            events.Add(GiveColaEffectToEveryone);
            events.Add(GiveRandomEffectToEveryone);
            events.Add(TeleportRandHumToIntercom);
            events.Add(KillHalfPlayers);
            events.Add(RespawnPlayerAsRandScp);
            events.Add(RespawnHalfAsMTFHalfAsCI);
            events.Add(GiveTripleCOMToRandHum);
            events.Add(GiveJailBirdToRandHum);
            events.Add(GivePartDisruptorToRandHum);
            events.Add(Give1576ToEveryone);
        }

        private void BroadcastToAllPlayers(string message)
        {
            foreach (Player p in Player.List)
                p.Broadcast(new Broadcast(message), true);
        }

        private void RemoveRandomItem(Player player)
        {
            player.RemoveItem(player.Items.ElementAt(rand.Next(player.Items.Count())));
        }

        private void StartWarhead()
        {
            Warhead.Start();
            BroadcastToAllPlayers("Boom boom time!");
        }

        private void GiveMicroToRandHum()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));

            if (InventorySystem.Inventory.MaxSlots - player.Items.Count() == 0)
                RemoveRandomItem(player);

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
                    p.ClearInventory();
            }

            BroadcastToAllPlayers("Where's your items, lads?");
        }

        private void TurnSpectatorsToZombie()
        {
            var spectators = Player.Get(RoleTypeId.Spectator);

            Vector3 spawnPos;
            if (AlphaWarheadController.Detonated)
                spawnPos = Room.Get(RoomType.Surface).Transform.TransformPoint(Vector3.up);
            else
                spawnPos = Room.Get(RoomType.Hcz049).Transform.TransformPoint(Vector3.up);

            foreach (Player p in spectators)
            {
                p.Role.Set(RoleTypeId.Scp0492, SpawnReason.Revived);
                p.Position = spawnPos;
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
                p.Scale = new Vector3(0.5f, 0.5f, 0.5f);

            BroadcastToAllPlayers("It's shrinkin' time!");
        }

        private void HumTeamSwapping()
        {
            List<Player> dClass = new List<Player>();
            foreach (Player p in Player.Get(RoleTypeId.ClassD))
                dClass.Add(p);

            List<Player> scientists = new List<Player>();
            foreach (Player p in Player.Get(RoleTypeId.Scientist))
                scientists.Add(p);

            List<Player> ntf = new List<Player>();
            foreach (Player p in Player.Get(Team.FoundationForces))
                ntf.Add(p);

            List<Player> ci = new List<Player>();
            foreach (Player p in Player.Get(Team.ChaosInsurgency))
                ci.Add(p);

            foreach (Player p in scientists)
                p.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.None);

            foreach (Player p in dClass)
                p.Role.Set(RoleTypeId.Scientist, RoleSpawnFlags.None);

            foreach (Player p in ntf)
                p.Role.Set(RoleTypeId.ChaosRifleman, RoleSpawnFlags.None);

            foreach (Player p in ci)
                p.Role.Set(RoleTypeId.NtfSergeant, RoleSpawnFlags.None);

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
                p1TempItems.Add(item);

            foreach (Item item in p2.Items)
                p2TempItems.Add(item);

            p2.ClearInventory(false);
            p2.AddItem(p1TempItems);

            p1.ClearInventory(false);
            p1.AddItem(p2TempItems);

            BroadcastToAllPlayers("Two players have swapped their inventories! But who are they?...");
        }

        private void GiveBlackCardToRandHum()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));

            if (InventorySystem.Inventory.MaxSlots - player.Items.Count() == 0)
                RemoveRandomItem(player);

            player.AddItem(ItemType.KeycardO5);

            BroadcastToAllPlayers("Someone got a V.I.P. access!");
        }

        private void TeleportRandHumToPocketDim()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));
            player.Position = new Vector3(0f, -1998.5f, 0f);
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
            Timing.RunCoroutine(FFTimer());

            BroadcastToAllPlayers("Friendly fire is on for 100 seconds!");
        }

        private IEnumerator<float> FFTimer()
        {
            yield return Timing.WaitForSeconds(100f);
            Server.FriendlyFire = false;
        }

        private void GiveColaEffectToEveryone()
        {
            var players = Player.List.Where(p => p.IsAlive);

            foreach(Player p in players)
                p.EnableEffect(EffectType.Scp207, 60);

            BroadcastToAllPlayers("I'm fast as fuck boiiii");
        }

        private void GiveRandomEffectToEveryone()
        {
            var players = Player.List.Where(p => p.IsAlive);
            
            foreach(Player p in players)
                p.ApplyRandomEffect(EffectCategory.None, 30f, true);

            BroadcastToAllPlayers("Everyone got a random effect for 30 seconds!");
        }

        private void TeleportRandHumToIntercom()
        {
            var humans = Player.List.Where(p => p.IsHuman);
            Player player = humans.ElementAt(rand.Next(humans.Count()));

            Vector3 intercomPos = Intercom.Transform.TransformPoint(Vector3.forward * 3f);
            player.Position = intercomPos;

            BroadcastToAllPlayers("Someone is locked up in the intercom!");
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

            BroadcastToAllPlayers("Dead players has been respawned and divided into two opposing teams...");
        }

        private void GiveTripleCOMToRandHum()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));

            if (InventorySystem.Inventory.MaxSlots - player.Items.Count() == 0)
                RemoveRandomItem(player);

            player.AddItem(ItemType.GunCom45, 1);
            player.AddAmmo(AmmoType.Nato9, 45);

            BroadcastToAllPlayers("Someone got a triple gun!");
        }

        private void GiveJailBirdToRandHum()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));

            if (InventorySystem.Inventory.MaxSlots - player.Items.Count() == 0)
                RemoveRandomItem(player);

            player.AddItem(ItemType.Jailbird, 1);

            BroadcastToAllPlayers("It's BONKing time!");
        }

        private void GivePartDisruptorToRandHum()
        {
            var players = Player.List.Where(p => p.IsHuman);
            Player player = players.ElementAt(rand.Next(players.Count()));

            if (InventorySystem.Inventory.MaxSlots - player.Items.Count() == 0)
                RemoveRandomItem(player);

            player.AddItem(ItemType.ParticleDisruptor, 1);

            BroadcastToAllPlayers("Interesting gun, innit?");
        }

        private void Give1576ToEveryone()
        {
            var players = Player.List.Where(p => p.IsHuman);
            foreach (Player p in players)
            {
                if (InventorySystem.Inventory.MaxSlots - p.Items.Count() == 0)
                    RemoveRandomItem(p);

                p.AddItem(ItemType.SCP1576, 1);
            }

            BroadcastToAllPlayers("Now you can talk with the dead!");
        }
    }
}
