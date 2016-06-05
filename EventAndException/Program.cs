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
			do {
				Console.WriteLine("_______________________________________________________________");
				Console.WriteLine("Введите имя персонажа");
				person.Name = Console.ReadLine();
				try
				{
					Console.WriteLine("_______________________________________________________________");
					Console.WriteLine("Введите возраст персонажа");
					person.Age = int.Parse(Console.ReadLine());
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
				Console.WriteLine(" ");
				//Делегаты и события				
				person.OnChanging+= (sender, changingArgs) =>
				Console.WriteLine("Old Name: {0}\tNewName: {1}", changingArgs.NewName, changingArgs.OldName);
				person.OnChanged += (sender, changedArgs) => Console.WriteLine("OnChanged to {0}", changedArgs.NewName);
				Console.WriteLine("Для завершения работы программы введите - Выйти");
				Console.WriteLine("Для очистки экрана введите < Очистить >");
				Console.WriteLine("Для проверки функции - собитие - нажмите любую клавишу и следуйте инструкциям на экране");
				if (Console.ReadLine() == "Очистить")
				{
					Console.Clear();
				}
			} while (Console.ReadLine() != "Выйти");
		}	
	}
	/// <summary>
	/// Класс описывающий дополнительные параметры добавляемые к параметрам общего класса для вывода ошибки.
	/// В данном слущае дополнительным параметром является поля id отвечающее за вывод номера строки.
	/// </summary>
	class MyException :ApplicationException
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
		//private int age;
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
		public int Age
		{
			get;
			set;	
		}				
		public Person ()
		{
			this.Name = Name;
			this.Age = Age;
		}

		public event EventHandler<PersonChangedArgs> OnChanged;
		public event EventHandler<PersonChangingArgs> OnChanging;
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
}
