using System;
using NUnit.Framework;

namespace Kean.Html.Dom.Test
{
	[TestFixture]
	public class Save :
		Kean.Test.Fixture<Save>
	{
		protected override void Run()
		{
			this.Run(
				this.Paragraph,
				this.Heading,
				this.Anchor,
				this.Abbreviation,
				this.Address,
				this.Aside,
				this.Audio,
				this.Emphasized,
				this.Strong,
				this.Definition,
				this.Code,
				this.KeyBoardInput,
				this.Sample,
				this.Variable,
				this.DescriptionList,
				this.Division,
				this.Italic,
				this.InlineFrame,
				this.Form,
				this.Mark,
				this.Meter,
				this.NoScript,
				this.OrderedList,
				this.Select,
				this.PreformattedText,
				this.Progress,
				this.Small,
				this.Quote,
				this.Caption,
				this.Cite,
				this.Object,
				this.TextArea,
				this.Base,
				this.BidirectionalOverride
			);
		}
		protected void Verify(Document document)
		{
			document.Save(Uri.Locator.FromPlatformPath(System.Environment.CurrentDirectory + "/Html/" + this.CurrentTestStep + ".html"));
			VerifyAsResource(System.Environment.CurrentDirectory + "/Html/" + this.CurrentTestStep + ".html", "/Html/Correct/" + this.CurrentTestStep + ".html", "Failed saving: " + this.CurrentTestStep + ".html");
		}
		[Test]
		public void Paragraph()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph("this is paragraph"));
			this.Verify(document);
		}
		[Test]
		public void Heading()
		{
			Document document = new Document();
			document.Body.Add(new Heading1("this is heading 1"));
			document.Body.Add(new Heading2("this is heading 2"));
			document.Body.Add(new Heading3("this is heading 3"));
			document.Body.Add(new Heading4("this is heading 4"));
			document.Body.Add(new Heading5("this is heading 5"));
			document.Body.Add(new Heading6("this is heading 6"));
			this.Verify(document);
		}
		[Test]
		public void Anchor()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph("here is a ", new Anchor("link") { Destination = "http://imint.se", Style = "color : red" }));
			this.Verify(document);
		}
		[Test]
		public void Abbreviation()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph(new Abbreviation("WHO") { Title = "World Health Organization" }, "was found in 1998"));
			this.Verify(document);
		}
		[Test]
		public void Address()
		{
			Document document = new Document();
			document.Body.Add(new Address("IMINT image Intelligence AB ", new LineBreak(), "Dag Hammarskjolds vag 10C ", new LineBreak(), "751 83 Uppsala, Sweden "));
			this.Verify(document);
		}
		[Test]
		public void Aside()
		{
			Document document = new Document();
			document.Body.Add(new Aside(new Heading4("Epcot Center"), new Paragraph("the Epcot Center is a theme park in Disney WORLD, Florida.")));
			this.Verify(document);
		}
		[Test]
		public void Audio()
		{
			Document document = new Document();
			document.Body.Add(new Audio(new MediaSource() { Source = "sample.ogg", Type = "audio/ogg" }, new MediaSource() { Source = "sample.mp3", Type = "audio/mpeg" }, "No supported format available.") { Controls = true });
			this.Verify(document);
		}
		[Test]
		public void Emphasized()
		{
			Document document = new Document();
			document.Body.Add(new Emphasized("Emphasized text"));
			this.Verify(document);
		}
		[Test]
		public void Strong()
		{
			Document document = new Document();
			document.Body.Add(new Strong("Strong text"));
			this.Verify(document);
		}
		[Test]
		public void Definition()
		{
			Document document = new Document();
			document.Body.Add(new Definition("Definition term"));
			this.Verify(document);
		}
		[Test]
		public void Code()
		{
			Document document = new Document();
			document.Body.Add(new Code("a piece of computer code"));
			this.Verify(document);
		}
		[Test]
		public void KeyBoardInput()
		{
			Document document = new Document();
			document.Body.Add(new KeyBoardInput("keyboardinput sample"));
			this.Verify(document);
		}
		[Test]
		public void Sample()
		{
			Document document = new Document();
			document.Body.Add(new Sample("sample output from computer code"));
			this.Verify(document);
		}
		[Test]
		public void Variable()
		{
			Document document = new Document();
			document.Body.Add(new Variable("variable"));
			this.Verify(document);
		}
		[Test]
		public void DescriptionList()
		{
			Document document = new Document();
			document.Body.Add(new DefinitionList(new DefinitionTerm("Coffee"), new DefinitionData("black hot drink"), new DefinitionTerm("Milk"), new DefinitionData("white cold drink")));
			this.Verify(document);
		}
		[Test]
		public void Division()
		{
			Document document = new Document();
			document.Body.Add(new Division(new Paragraph("this is paragraph"), new Heading3("this is heading")) { Style = "color : purple" });
			this.Verify(document);
		}
		[Test]
		public void Italic()
		{
			Document document = new Document();
			document.Body.Add(new Italic("this text is in italic"));
			this.Verify(document);
		}
		[Test]
		public void InlineFrame()
		{
			Document document = new Document();
			document.Body.Add(new InlineFrame(new Paragraph("Inline Frame")) { Source = "http://www.w3schools.com" });
			this.Verify(document);
		}
		[Test]
		public void Form()
		{
			Document document = new Document();
			document.Body.Add(new Form(new FieldSet(new Legend("Form"), new Label("Firse Name :") { For = "fname" }, new Input() { Type = "text", Identifier = "fname" }, new LineBreak(), "Last Name: ", new Input() { Type = "text", Name = "lname" }, new LineBreak(), "Encryption: ", new KeyGenerator() { Name = "security" }, new LineBreak(), new Input() { Type = "submit", Value = "submit" })));
			this.Verify(document);
		}
		[Test]
		public void Mark()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph("do not forget to buy ", new Mark("milk"), " today"));
			this.Verify(document);
		}
		[Test]
		public void Meter()
		{
			Document document = new Document();
			document.Body.Add(new Meter("2 out of 10") { CurrentValue = "2", Minimum = "0", Maximum = "10" });
			this.Verify(document);
		}
		[Test]
		public void NoScript()
		{
			Document document = new Document();
			document.Body.Add(new NoScript("this is no script text"));
			this.Verify(document);
		}
		[Test]
		public void OrderedList()
		{
			Document document = new Document();
			document.Body.Add(new OrderedList(new ListItem("coffee"), new ListItem("Tea"), new ListItem("milk")) { StartValue = "50", Reversed = true });
			this.Verify(document);
		}
		[Test]
		public void Select()
		{
			Document document = new Document();
			document.Body.Add(new Select(new OptionGroup(new Option("Volvo"), new Option("saab")) { LabelForOption = "Swedish Cars" }));
			this.Verify(document);
		}
		[Test]
		public void PreformattedText()
		{
			Document document = new Document();
			document.Body.Add(new PreformattedText("this is the     preformated text"));
			this.Verify(document);
		}
		[Test]
		public void Progress()
		{
			Document document = new Document();
			document.Body.Add(new Progress() { Value = "32", Maximum = "150" });
			this.Verify(document);
		}
		[Test]
		public void Small()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph(new Small("this is small text")));
			this.Verify(document);
		}
		[Test]
		public void Quote()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph("WWF's goal is to ", new Quote("biuld a future where people live in harmony with nature"), "we hope they succeed"));
			this.Verify(document);
		}
		[Test]
		public void Caption()
		{
			Document document = new Document();
			document.Body.Add(new Table(new Caption("Monthly savings"), new TableRow(new TableHeading("Month"), new TableHeading("saving")), new TableRow(new TableData("january"), new TableData("100"))));
			this.Verify(document);
		}
		[Test]
		public void Cite()
		{
			Document document = new Document();
			document.Body.Add(new Paragraph(new Cite("The Scream"), "by Edward Munch"));
			this.Verify(document);
		}
		[Test]
		public void Object()
		{
			Document document = new Document();
			document.Body.Add(new Object() { Width = "200", Height = "200", Data = "http://www.w3schools.com" });
			this.Verify(document);
		}
		[Test]
		public void TextArea()
		{
			Document document = new Document();
			document.Body.Add(new TextArea("At w3schools.com you will learn how to make a website. We offer free tutorials in all web development technologies.") { Row = "2", Column = "5" });
			this.Verify(document);
		}
		[Test]
		public void Base()
		{
			Document document = new Document();
			document.Head.Add(new Base() { Destination = "http://www.w3schools.com/images/", Target = "_blank" });
			this.Verify(document);
		}
		[Test]
		public void BidirectionalOverride()
		{
			this.Verify(new Document(new Head("", new BidirectionalOverride("This text will go right-to-left.") { Direction = "rtl" }), new Body()));
		}
	}
}

