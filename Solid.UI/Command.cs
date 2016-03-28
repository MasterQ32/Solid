namespace Solid.UI
{
	using System;

	public abstract class Command
	{
		public abstract void Execute();
	}

	public sealed class ActionCommand : Command
	{
		private readonly Action command;

		public ActionCommand(Action command)
		{
			if (command == null)
				throw new ArgumentNullException(nameof(command));
			this.command = command;
		}

		public override void Execute() => this.command();
	}
}