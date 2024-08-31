using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace NameFilter;

class Program 
	{
		private static List<string> testCases = new List<string>
			{
				"Вася Пупкин",
				"сеееекккксссс",
				"Вася Набегатор",
				"Яна Цист",
				"пеТя нЕфОр"
			};
		
		private static List<string> bannedPatterns = new List<string>
		{
			// TODO: fill this
			"гитлер",
			"нацист"
		};

		private static Regex bannedRegex = new Regex(string.Join("|", bannedPatterns), RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static Regex latinRegex = new Regex(@"^[a-zA-Z\s]+$", RegexOptions.Compiled);
		private static Regex cyrillicRegex = new Regex(@"^[а-яА-ЯёЁ\s]+$", RegexOptions.Compiled);
		private static Regex moreThanThreeRepeatedChars = new Regex(@"(\w)\1{3,}", RegexOptions.Compiled);
		private static Regex enNameScheme = new Regex(@"^[A-Z][a-z]+ [A-Z][a-z]+$", RegexOptions.Compiled);
		private static Regex ruNameScheme = new Regex(@"^[А-Я][а-я]+ [А-я][а-я]+$", RegexOptions.Compiled);
		private static readonly Regex AllowedCharactersRegex = new Regex(@"[^a-zA-Zа-яА-Я]", RegexOptions.Compiled);

		
		public static bool CheckName(string name) 
		{			
			string normalized = NormalizeName(name);

			return (!bannedRegex.IsMatch(normalized) 
					&& IsSingleAlphabet(normalized)
					&& (enNameScheme.IsMatch(name) || ruNameScheme.IsMatch(name))
					&& !moreThanThreeRepeatedChars.IsMatch(name));
		}
		
		public static string NormalizeName(string name) 
		{
			// В нижний регистр
			name = name.ToLower();
			
			name = AllowedCharactersRegex.Replace(name, "");
			
			// Удаление повторяющихся букв
			name = Regex.Replace(name, @"(\w)\1+", "$1");
			
			return name;
		}
		public static bool IsSingleAlphabet(string name)
		{
			return latinRegex.IsMatch(name) || cyrillicRegex.IsMatch(name);
		}
		
		public static void Main(string[] args) 
		{
			foreach (var test in testCases) 
			{
				Console.WriteLine($"{test} : {CheckName(test)}");
			}
		}
	}
