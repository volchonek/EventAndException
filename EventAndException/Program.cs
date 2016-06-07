using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAndException
{
	class Program
	{
		/// <summary>
		/// Реализация событий и исключений
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			Person person = new Person();
			for (int i = 0; i < 2; i++)
			{
				Console.WriteLine("_______________________________________________________________");
				Console.WriteLine("Введите имя персонажа");
				person.Name = Console.ReadLine();
				try
				{

					Console.WriteLine("_______________________________________________________________");
					Console.WriteLine("Введите возраст персонажа");
					person.Age = int.Parse(Console.ReadLine());
					Console.WriteLine(" ");
					Console.WriteLine("Были введены следующие параметры");
					Console.WriteLine("Имя - {0}, Возраст - {1}", person.Name, person.Age);
				}
				catch (FormatException fe)
				{
					Console.WriteLine("{0}", fe.Message);
					Console.WriteLine("{0}", fe.Source);
					Console.WriteLine("{0}", fe.StackTrace);
					Console.WriteLine("{0}", fe.TargetSite);
				}
				catch (MyException me)
				{
					Console.WriteLine("{0}", me.Message);
					Console.WriteLine("{0}", me.Source);
					Console.WriteLine("{0}", me.StackTrace);
					Console.WriteLine("{0}", me.TargetSite);
				}
				// Делегаты и события	
				Console.WriteLine(" ");
// Вешаем обработку события при изменении переменной Age за пределы промежутка 0 - 150 ( срабатывает к сожалению только с изменениемзначения параметра Age)
				person.OnError += (sender, ErrorAge) => Console.WriteLine(ErrorAge.Message);
				
				// Вешаем обработку события при изменении имени		
				person.OnChanging += (sender, changingArgs) => Console.WriteLine("Old Name: {0}\tNew Name: {1}", changingArgs.NewName, changingArgs.OldName);
				person.OnChanged += (sender, changedArgs) => Console.WriteLine("OnChanged to {0}", changedArgs.NewName);

				// Вешаем обработку события при изменении возраста	
				person.OnChangingAge += (sender, changingArgsAge) => Console.WriteLine("Old Age: {0}\tNew Age: {1}", changingArgsAge.NewAge, changingArgsAge.OldAge);
				person.OnChangedAge += (sender, changedArgsAge) => Console.WriteLine("OnChanged to {0}", changedArgsAge.NewAge);
				Console.ReadKey();
			}
		}
	}

	/// <summary>
	/// Класс описывающий дополнительные параметры добавляемые к параметрам общего класса для вывода ошибки.
	/// В данном слущае дополнительным параметром является поля id отвечающее за вывод номера строки.
	/// </summary>
	class MyException : ApplicationException
	{
		public MyException(string s, int id)
			: base(s)
		{
			this.Id = id;
		}
		public int Id { get; private set; }
	}

	/// <summary>
	/// класс опписывающий объект Person с параметрами Name и Age
	/// </summary>
	class Person
	{
		private string name;
		private int age;

		/// <summary>
		///  вешаем обработку события при изменения поля Name вклассе Person
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.OnChanging != null)
				{
					this.OnChanging(this, new PersonChangingArgs(new Args { NewName = this.name, OldName = value }));
				}

				this.name = value;

				if (this.OnChanged != null)
				{
					this.OnChanged(this, new PersonChangedArgs(this.Name));
				}
			}
		}
		/// <summary>
		/// вешаем обработку события при изменения поля Age вклассе Person
		/// </summary>
		public int Age
		{
			get
			{
				return this.age;
			}
			set
			{
					if (this.OnChangingAge != null)
					{
						this.OnChangingAge(this, new PersonChangingArgsAge(new ArgsAge { NewAge = this.age, OldAge = value }));
					}

					this.age = value;

					if (this.OnChangedAge != null)
					{
						this.OnChangedAge(this, new PersonChangedArgsAge(this.Age));
					}
					if (value < 0 || value > 150)
						{ 
							if (this.OnError != null)
							{
								this.OnError(this, new OnErrorEventArgs((string.Format("Error!Error!Error!Error!Error!Error!Error!Error!Error!Error!"))));
							}
						}
			}
		}
		public Person()
		{
			this.Name = Name;
			this.Age = Age;
		}
		// Передача ссылки через EventHandler для поля Name
		public event EventHandler<PersonChangedArgs> OnChanged;
		public event EventHandler<PersonChangingArgs> OnChanging;
		// Передача ссылки через EventHandler для поля Age
		public event EventHandler<PersonChangedArgsAge> OnChangedAge;
		public event EventHandler<PersonChangingArgsAge> OnChangingAge;
		// Передача ссылки через EventHandler для поля Age при инициализации события
		public event EventHandler<OnErrorEventArgs> OnError;
	}

	/// <summary>
	/// Отвечает за параметр string по полю Name при передачи данных в классе Person
	/// </summary>
	class PersonChangedArgs : EventArgs
	{
		public PersonChangedArgs(string newName)
		{
			this.NewName = newName;
		}
		public string NewName { get; private set; }
	}

	/// <summary>
	/// Отвечает за параметры string для NewName и OldName по полю Name при передачи данных в классе Person
	/// </summary>
	class PersonChangingArgs : EventArgs
	{
		public PersonChangingArgs(Args args)
		{
			this.NewName = args.NewName;
			this.OldName = args.OldName;
		}
		public string NewName { get; private set; }
		public string OldName { get; private set; }
	}

	/// <summary>
	/// Формирует начальные значения NewName и OldName для класса Person
	/// </summary>
	class Args
	{
		public string NewName { get; set; }
		public string OldName { get; set; }
	}

	/// <summary>
	/// Отвечает за передачу данных через EventHandler поля Age
	/// </summary>
	class PersonChangedArgsAge : EventArgs
	{
		public PersonChangedArgsAge(int newAge)
		{
			this.NewAge = newAge;
		}
		public int NewAge { get; private set; }
	}

	/// <summary>
	///  Отвечает за передачу данных через EventHandler полей Age и Name
	/// </summary>
	class PersonChangingArgsAge : EventArgs
	{
		public PersonChangingArgsAge(ArgsAge args)
		{
			this.NewAge = args.NewAge;
			this.OldAge = args.OldAge;
		}
		public int NewAge { get; private set; }
		public int OldAge { get; private set; }
	}
	/// <summary>
	///  Формирует начальные значения NewAge и OldAge для класса Person
	/// </summary>
	class ArgsAge
	{
		public int NewAge { get; set; }
		public int OldAge { get; set; }
	}
	/// <summary>
	/// Класс, отвечает за передачу параметрапо ссылке типа string
	/// </summary>
	class OnErrorEventArgs : EventArgs
	{
		public string Message { get; private set;}
		public OnErrorEventArgs(string message)
		{
			this.Message = message;
		}
	}
}
