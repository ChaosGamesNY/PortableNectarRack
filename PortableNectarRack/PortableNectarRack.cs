using System;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Actors;
using Sims3.SimIFace;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.UI;
using System.Collections.Generic;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.Gameplay.Utilities;

namespace Sims3.Gameplay.Objects.Decorations.MonoChaos
{
	public class PortableNectarRack : NectarRackExpensive, IHasObjectInventoryInteractions
	{
		//
		// Static Fields
		//
		public static int kMaxBottlesInRack = 1000;

		//
		// Properties
		//
		public override int MaxBottlesInRack {
			get {
				return PortableNectarRack.kMaxBottlesInRack;
			}
		}

		//
		// Constructors
		//

		//
		// Methods
		//

		//
		// Nested Types
		//
		public class Open : ImmediateInteraction<Sim, NectarRack>
		{
			private sealed class Definition : ImmediateInteractionDefinition<Sim, NectarRack, Open>
			{
				protected override bool Test(Sim a, NectarRack target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return true;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			protected override bool Run()
			{
				HudModel.OpenObjectInventoryForOwner(Target);
				return true;
			}
			protected override bool RunFromInventory()
			{
				HudModel.OpenObjectInventoryForOwner(Target);
				return true;
			}
		}

		public class TakeAllNectar : ImmediateInteraction<Sim, NectarRack>
		{
			private sealed class Definition : ImmediateInteractionDefinition<Sim, NectarRack, TakeAllNectar>
			{
				protected override bool Test(Sim a, NectarRack target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return true;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			protected override bool Run()
			{
				List<NectarBottle> list = Target.Inventory.FindAll<NectarBottle>(checkInUse: true);
				foreach (NectarBottle item in list)
				{
					Actor.Inventory.TryToMove(item);
				}
				return true;
			}
			protected override bool RunFromInventory()
			{
				List<NectarBottle> list = Target.Inventory.FindAll<NectarBottle>(checkInUse: true);
				foreach (NectarBottle item in list)
				{
					Actor.Inventory.TryToMove(item);
				}
				return true;
			}

			public override void Cleanup()
			{
				base.Cleanup();
			}
		}

		public class StockWithNectar : ImmediateInteraction<Sim, NectarRack>
		{
			private sealed class Definition : ImmediateInteractionDefinition<Sim, NectarRack, StockWithNectar>
			{
				private string GreyTooltipCallback()
				{
					return LocalizeString("RackFullToolTip");
				}

				protected override bool Test(Sim a, NectarRack target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (target.Inventory.IsFull())
					{
						greyedOutTooltipCallback = GreyTooltipCallback;
						return false;
					}
					return true;
				}
			}

			private const string sLocalizationKey = "Gameplay/Objects/Decorations/NectarRack/StockWithNectar";

			public static readonly InteractionDefinition Singleton = new Definition();

			private static string LocalizeString(string name, params object[] parameters)
			{
				return Localization.LocalizeString("Gameplay/Objects/Decorations/NectarRack/StockWithNectar:" + name, parameters);
			}

			protected override bool Run()
			{
				List<NectarBottle> list = Actor.Inventory.FindAll<NectarBottle>(checkInUse: true);
				foreach (NectarBottle item in list)
				{
					Target.Inventory.TryToMove(item);
				}
				return true;
			}
			protected override bool RunFromInventory()
			{
				List<NectarBottle> list = Actor.Inventory.FindAll<NectarBottle>(checkInUse: true);
				foreach (NectarBottle item in list)
				{
					Target.Inventory.TryToMove(item);
				}
				return true;
			}

			public override void Cleanup()
			{
				base.Cleanup();
			}
		}

		public override void OnStartup()
		{
			AddComponent<ItemComponentKeepChildrenParented>(new object[1] { ItemComponent.SimInventoryItem });
			base.AddInteraction (Open.Singleton);
			base.AddInteraction (TakeAllNectar.Singleton);
			base.AddInteraction (StockWithNectar.Singleton);
			base.AddInventoryInteraction (Open.Singleton);
			base.AddInventoryInteraction (TakeAllNectar.Singleton);
			base.AddInventoryInteraction (StockWithNectar.Singleton);
			base.OnStartup();
		}

	}
}

