using System;
using Kean;
using Kean.Extension;

namespace Kean.Password
{
	public class PhonemeGenerator :
	Generator
	{
		public override string Next ()
		{
			Collection.List<Element> result;
			bool hasDigit;
			bool hasUppercase;
			do
			{
				result = new Collection.List<Element>();
				hasDigit = false;
				hasUppercase = false;
				Element last = null;
				Element secondLast = null;
				int length = 0;
				while (length < this.Length)
				{
					Element next;
					do
						next = Element.Phonemes[this.Generate(Element.Phonemes.Length)];
					while (last.IsNull() ? next.NotFirst : last.Digit ||
					       (next.Vowel ? last.Vowel &&
					       (last.Dipthong || next.Dipthong || secondLast.NotNull() && secondLast.Vowel || this.Generate(10) > 7) :
						!last.Vowel) ||
					       length + next.Length > this.Length);

					if (this.WithUppercase && this.Generate(10) < 2)
					{
						next = next.ToUpper();
						hasUppercase = true;
					}

					secondLast = last;
					result.Add(last = next);
					length += next.Length;
					if (length < this.Length)
					{
						if (this.WithDigits && this.Generate(10) < 3)
						{
							next = Element.CreateDigit(this.Generate(10));
							result.Add(next);
							length += next.Length;
							hasDigit = true;
						}
					}
				}
			}
			while (this.WithDigits != hasDigit || this.WithUppercase != hasUppercase);
			return result.Map(element => element.Symbol).Join();
		}
	}
}

