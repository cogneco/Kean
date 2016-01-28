using System;

namespace Kean.Password
{
	public class Element
	{
		public readonly string Symbol;
		public readonly int Length;
		public readonly bool Dipthong;
		public readonly bool Vowel;
		public readonly bool Digit;
		public readonly bool NotFirst;
		public readonly bool Uppercase;
		Element(string symbol, bool vowel, bool notFirst)
		{
			this.Symbol = symbol;
			this.Length = symbol.Length;
			this.Dipthong = this.Length > 1;
			this.Vowel = vowel;
			this.NotFirst = notFirst;
		}
		Element(int number)
		{
			this.Symbol = number.ToString();
			this.Length = this.Symbol.Length;
			this.Digit = true;
		}
		Element(Element original)
		{
			this.Symbol = original.Symbol.Substring(0, 1).ToUpper();
			if (original.Length > 1)
				this.Symbol += original.Symbol.Substring(1);
			this.Length = this.Symbol.Length;
			this.Dipthong = original.Dipthong;
			this.Vowel = original.Vowel;
			this.Digit = original.Digit;
			this.NotFirst = original.NotFirst;
			this.Uppercase = original.Uppercase;
		}
		public Element ToUpper ()
		{
			return new Element(this);
		}
		public static Element CreateDigit (int number)
		{
			return new Element(number);
		}
		public static Element CreateConsonant (string symbol)
		{
			return new Element(symbol, false, false);
		}
		public static Element CreateConsonant (string symbol, bool notFirst)
		{
			return new Element(symbol, false, notFirst);
		}
		public static Element CreateVowel (string symbol)
		{
			return new Element(symbol, true, false);
		}
		public readonly static Element[] Phonemes = new Element[] {
			Element.CreateVowel("a"),
			Element.CreateVowel("ae"),
			Element.CreateVowel("ah"),
			Element.CreateVowel("ai"),
			Element.CreateConsonant("b"),
			Element.CreateConsonant("c"),
			Element.CreateConsonant("ch"),
			Element.CreateConsonant("d"),
			Element.CreateVowel("e"),
			Element.CreateVowel("ee"),
			Element.CreateVowel("ei"),
			Element.CreateConsonant("f"),
			Element.CreateConsonant("g"),
			Element.CreateConsonant("gh", true),
			Element.CreateConsonant("h"),
			Element.CreateVowel("i"),
			Element.CreateVowel("ie"),
			Element.CreateConsonant("j"),
			Element.CreateConsonant("k"),
			Element.CreateConsonant("l"),
			Element.CreateConsonant("m"),
			Element.CreateConsonant("n"),
			Element.CreateConsonant("ng", true),
			Element.CreateVowel("o"),
			Element.CreateVowel("oh"),
			Element.CreateVowel("oo"),
			Element.CreateConsonant("p"),
			Element.CreateConsonant("ph"),
			Element.CreateConsonant("qu"),
			Element.CreateConsonant("r"),
			Element.CreateConsonant("s"),
			Element.CreateConsonant("sh"),
			Element.CreateConsonant("t"),
			Element.CreateConsonant("th"),
			Element.CreateVowel("u"),
			Element.CreateConsonant("v"),
			Element.CreateConsonant("w"),
			Element.CreateConsonant("x"),
			Element.CreateConsonant("y"),
			Element.CreateConsonant("z"),
		};
	}
}

