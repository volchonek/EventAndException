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
		/// реализация исключений с помощью стандартнаго класса Exception и интерфейса
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			Person person = new Person();
			ICatchExceptionInterface exception = new MyException();
			int s;
			do {
				person.Name = Console.ReadLine();
				try
				{
					//реализация исключения с помощью интерфейса ICatchExeption()
					bool TrueOrFalse = int.TryParse(person.Name, out s);
					if (TrueOrFalse) { exception.ICatchExeption(); }
					//реализация исключения через Exception
					person.Age = int.Parse(Console.ReadLine());
					Console.WriteLine("Имя - {0}, Возраст - {1}", person.Name, person.Age);
				}

				catch(Exception e)
				{
					Console.WriteLine("{0}", e.Message);
					Console.WriteLine("{0}", e.Source);
					Console.WriteLine("{0}", e.StackTrace);
					Console.WriteLine("{0}", e.TargetSite);
				}
				Console.WriteLine(" ");
				Console.Write("Для завершения работы программы введите - Выйти => ");
			} while (Console.ReadLine() != "Выйти");
		}
	}
	/// <summary>
	/// класс описывающий интерфейс для вывода ошибки
	/// </summary>
	class MyException : ICatchExceptionInterface
	{
		public void ICatchExeption()
		{
			throw new Exception("Имя должно состоять из букв");
		}
	}
	/// <summary>
	///		интерфейсICatchExceptionInterface
	/// </summary>
	interface ICatchExceptionInterface
	{
		void ICatchExeption();		
	}

	/// <summary>
	/// класс опписывающий объект Person с параметрами Name и Age
	/// </summary>
	class Person 
	{
		public string Name{ get;set;}
		public int Age{ get; set;}
				
		public Person ()
		{
			this.Name = Name;
			this.Age = Age;
		}
	}
}
