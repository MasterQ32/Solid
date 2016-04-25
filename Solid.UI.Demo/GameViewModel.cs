namespace Solid.UI.Demo
{
	using System;
	using System.Collections.ObjectModel;
	using System.ComponentModel;

	public sealed class GameViewModel
	{
		private string displayText = "";
		private ObservableCollection<object> pressedNumbers;

		public event PropertyChangedEventHandler PropertyChanged;

		public GameViewModel()
		{
			this.Input0 = new KeypadCommand(this, 0);
			this.Input1 = new KeypadCommand(this, 1);
			this.Input2 = new KeypadCommand(this, 2);
			this.Input3 = new KeypadCommand(this, 3);
			this.Input4 = new KeypadCommand(this, 4);
			this.Input5 = new KeypadCommand(this, 5);
			this.Input6 = new KeypadCommand(this, 6);

			this.LowerRow = new
			{
				Input7 = new KeypadCommand(this, 7),
				Input8 = new KeypadCommand(this, 8),
				Input9 = new KeypadCommand(this, 9),
			};

			this.Clear = new ActionCommand(() => this.pressedNumbers.Clear());
			this.Check = new ActionCommand(() =>
			{
				this.pressedNumbers[1] = new PressedNumber(50);
			});

			this.pressedNumbers = new ObservableCollection<object>();
			this.pressedNumbers.Add(new PressedNumber(10));
			this.pressedNumbers.Add(new PressedNumber(20));
			this.pressedNumbers.Add(new PressedNumber(30));
			this.pressedNumbers.CollectionChanged += PressedNumbers_CollectionChanged;
		}

		private void PressedNumbers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Console.WriteLine("Changed: {0}", e.Action);
		}

		private void InputNumber(int number)
		{
			Console.WriteLine("Input: {0}", number);

			this.PressedNumbers.Add(new PressedNumber(number));
			this.DisplayText += number.ToString();
		}

		public string DisplayText
		{
			get { return this.displayText; }
			private set
			{
				this.displayText = value;
				this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayText)));
			}
		}

		public Command Input0 { get; private set; }
		public Command Input1 { get; private set; }
		public Command Input2 { get; private set; }
		public Command Input3 { get; private set; }
		public Command Input4 { get; private set; }
		public Command Input5 { get; private set; }
		public Command Input6 { get; private set; }

		public object LowerRow { get; private set; }

		public ObservableCollection<object> PressedNumbers => this.pressedNumbers;

		public Command Clear { get; private set; }

		public Command Check { get; private set; }

		private class KeypadCommand : Command
		{
			private readonly GameViewModel vm;
			private int value;

			public KeypadCommand(GameViewModel gameViewModel, int value)
			{
				this.vm = gameViewModel;
				this.value = value;
			}

			public override void Execute() => this.vm.InputNumber(this.value);
		}
	}

	public sealed class PressedNumber
	{
		public PressedNumber(int number)
		{
			this.Text = number.ToString();
		}

		public string Text { get; private set; }
	}
}