// 
//  Element.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License

using System;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Kean.Extension;
using Generic = System.Collections.Generic;

namespace Kean.Html.Dom
{
	public abstract class Element :
		Node,
		System.Collections.Generic.IEnumerable<Node>
	{
		Collection.IList<Node> childNodes = new Collection.List<Node>();

		public bool Empty { get { return this.childNodes.Count == 0; } }

		protected abstract string TagName { get; }
		protected bool NoLineBreaks { get; set; }
		protected bool RequiresEndTag { get; set; }
		public string Identifier { get; set; }
		public string Direction { get; set; }
		public string Class { get; set; }
		public string Title { get; set; }
		public string Style { get; set; }
		public string Language { get; set; }
		public bool SpellCheck { get; set; }
		public bool ContentEditable { get; set; }

		#region Event Attributes
		#region Window Event Attributes
		public string OnAfterPrint { get; set; }
		public string OnBeforePrint { get; set; }
		public string OnBeforeUnload { get; set; }
		public string OnErrorWindow { get; set; }
		public string OnHasChange { get; set; }
		public string OnLoad { get; set; }
		public string OnMessage { get; set; }
		public string OnOffline { get; set; }
		public string OnOnline { get; set; }
		public string OnPageHide { get; set; }
		public string OnPageShow { get; set; }
		public string OnPopState { get; set; }
		public string OnRedo { get; set; }
		public string OnResize { get; set; }
		public string OnStorage { get; set; }
		public string OnUndo { get; set; }
		public string OnUnload { get; set; }
		#endregion
		#region Form Events
		public string OnBlur { get; set; }
		public string OnChange { get; set; }
		public string OnContextMenu { get; set; }
		public string OnFocus { get; set; }
		public string OnFormChange { get; set; }
		public string OnFormInput { get; set; }
		public string OnInput { get; set; }
		public string OnInvalid { get; set; }
		public string OnSelect { get; set; }
		public string OnSubmit { get; set; }
		#endregion
		#region Keyboard Events
		public string OnKeyDown { get; set; }
		public string OnKeyPress { get; set; }
		public string OnKeyUp { get; set; }
		#endregion
		#region Mouse Events
		public string OnClick { get; set; }
		public string OnDoubleClick { get; set; }
		public string OnDrag { get; set; }
		public string OnDragEnd { get; set; }
		public string OnDragEnter { get; set; }
		public string OnDragLeave { get; set; }
		public string OnDragOver { get; set; }
		public string OnDragStart { get; set; }
		public string OnDrop { get; set; }
		public string OnMouseDown { get; set; }
		public string OnMouseMove { get; set; }
		public string OnMouseOut { get; set; }
		public string OnMouseOver { get; set; }
		public string OnMouseUp { get; set; }
		public string OnMouseWheel { get; set; }
		public string OnScroll { get; set; }
		#endregion
		#region Media Events
		public string OnAbort { get; set; }
		public string OnCanPlay { get; set; }
		public string OnCanPlayThrough { get; set; }
		public string OnDurationChange { get; set; }
		public string OnEmptied { get; set; }
		public string OnEnded { get; set; }
		public string OnErrorMedia { get; set; }
		public string OnLoadedData { get; set; }
		public string OnLoadedMetaData { get; set; }
		public string OnLoadStart { get; set; }
		public string OnPause { get; set; }
		public string OnPlay { get; set; }
		public string OnPlaying { get; set; }
		public string OnProgress { get; set; }
		public string OnRateChange { get; set; }
		public string OnReadyStateChange { get; set; }
		public string OnSeeked { get; set; }
		public string OnSeeking { get; set; }
		public string OnStalled { get; set; }
		public string OnSuspend { get; set; }
		public string OnTimeUpdate { get; set; }
		public string OnVolumeChange { get; set; }
		public string OnWaiting { get; set; }
		#endregion
		#endregion
		protected Element()
		{}
		protected Element(Generic.IEnumerable<Node> nodes)
		{
			this.childNodes.Add(nodes);
		}
		public void Add(Node node)
		{
			this.childNodes.Add(node);
		}
		public void Add(params Node[] nodes)
		{
			this.childNodes.Add(nodes);
		}
		string FormatEventAttributes()
		{
			return
				this.FormatAttribute("onafterprint", this.OnAfterPrint) +
				this.FormatAttribute("onbeforeprint", this.OnBeforePrint) +
				this.FormatAttribute("onbeforeload", this.OnBeforeUnload) +
				this.FormatAttribute("onhaschange", this.OnHasChange) +
				this.FormatAttribute("onerror", this.OnErrorWindow) + // TODO: ?
				this.FormatAttribute("onload", this.OnLoad) +
				this.FormatAttribute("onmessage", this.OnMessage) +
				this.FormatAttribute("onoffline", this.OnOffline) +
				this.FormatAttribute("ononline", this.OnOnline) +
				this.FormatAttribute("onpagehide", this.OnPageHide) +
				this.FormatAttribute("onpageshow", this.OnPageShow) +
				this.FormatAttribute("onpopstate", this.OnPopState) +
				this.FormatAttribute("onredo", this.OnRedo) +
				this.FormatAttribute("onresize", this.OnResize) +
				this.FormatAttribute("onstorage", this.OnStorage) +
				this.FormatAttribute("onundo", this.OnUndo) +
				this.FormatAttribute("onunload", this.OnUnload) +
				this.FormatAttribute("onblur", this.OnBlur) +
				this.FormatAttribute("onchange", this.OnChange) +
				this.FormatAttribute("oncontextmenu", this.OnContextMenu) +
				this.FormatAttribute("onfocus", this.OnFocus) +
				this.FormatAttribute("onformchange", this.OnFormChange) +
				this.FormatAttribute("onforminput", this.OnFormInput) +
				this.FormatAttribute("oninput", this.OnInput) +
				this.FormatAttribute("oninvalid", this.OnInvalid) +
				this.FormatAttribute("onselect", this.OnSelect) +
				this.FormatAttribute("onsubmit", this.OnSubmit) +
				this.FormatAttribute("onkeydown", this.OnKeyDown) +
				this.FormatAttribute("onkeypress", this.OnKeyPress) +
				this.FormatAttribute("onkeyup", this.OnKeyUp) +
				this.FormatAttribute("onclick", this.OnClick) +
				this.FormatAttribute("ondblclick", this.OnDoubleClick) + //TODO: ?
				this.FormatAttribute("ondrag", this.OnDrag) +
				this.FormatAttribute("ondragend", this.OnDragEnd) +
				this.FormatAttribute("ondragenter", this.OnDragEnter) +
				this.FormatAttribute("ondragleave", this.OnDragLeave) +
				this.FormatAttribute("ondragover", this.OnDragOver) +
				this.FormatAttribute("ondragstart", this.OnDragStart) +
				this.FormatAttribute("ondrop", this.OnDrop) +
				this.FormatAttribute("onmousedown", this.OnMouseDown) +
				this.FormatAttribute("onmousemove", this.OnMouseMove) +
				this.FormatAttribute("onmouseout", this.OnMouseOut) +
				this.FormatAttribute("onmouseover", this.OnMouseOver) +
				this.FormatAttribute("onmouseup", this.OnMouseUp) +
				this.FormatAttribute("onmousewheel", this.OnMouseWheel) +
				this.FormatAttribute("onscroll", this.OnScroll) +
				this.FormatAttribute("onabort", this.OnAbort) +
				this.FormatAttribute("oncanplay", this.OnCanPlay) +
				this.FormatAttribute("oncanplaythrough", this.OnCanPlayThrough) +
				this.FormatAttribute("ondurationchange", this.OnDurationChange) +
				this.FormatAttribute("onemptied", this.OnEmptied) +
				this.FormatAttribute("onended", this.OnEnded) +
				this.FormatAttribute("onerror", this.OnErrorMedia) + //TODO: ?
				this.FormatAttribute("onloadeddata", this.OnLoadedData) +
				this.FormatAttribute("onloadedmetadata", this.OnLoadedMetaData) +
				this.FormatAttribute("onloadstart", this.OnLoadStart) +
				this.FormatAttribute("onpause", this.OnPause) +
				this.FormatAttribute("onplay", this.OnPlay) +
				this.FormatAttribute("onplaying", this.OnPlaying) +
				this.FormatAttribute("onprogress", this.OnProgress) +
				this.FormatAttribute("onratechange", this.OnRateChange) +
				this.FormatAttribute("onreadystatechange", this.OnReadyStateChange) +
				this.FormatAttribute("onseeked", this.OnSeeked) +
				this.FormatAttribute("onseeking", this.OnSeeking) +
				this.FormatAttribute("onstalled", this.OnStalled) +
				this.FormatAttribute("onsuspend", this.OnSuspend) +
				this.FormatAttribute("ontimeupdate", this.OnTimeUpdate) +
				this.FormatAttribute("onvolumechange", this.OnVolumeChange) +
				this.FormatAttribute("onwaiting", this.OnWaiting);

		}
		protected virtual string FormatAttributes()
		{
			return
				this.FormatAttribute("id", this.Identifier) +
				this.FormatAttribute("dir", this.Direction) +
				this.FormatAttribute("title", this.Title) +
				this.FormatAttribute("style", this.Style) +
				this.FormatAttribute("class", this.Class) +
				this.FormatAttribute("lang", this.Language) +
				this.FormatAttribute("spellcheck", this.SpellCheck) +
				this.FormatAttribute("contenteditable", this.ContentEditable) +
				this.FormatEventAttributes();
		}
		protected string FormatAttribute<T>(string name, T value, T @default)
		{
			return value.SameOrEquals(@default) ? "" : " " + name + "=\"" + value.AsString() + "\"";
		}
		protected string FormatAttribute<T>(string name, T value) where T : class
		{
			return this.FormatAttribute(name, value, null);
		}
		protected string FormatAttribute(string name, bool value)
		{
			return value ? " " + name : "";
		}
		public override string Format(int indent)
		{
			string indentation = indent > 0 ? new string('\t', indent) : "";
			string result;
			result = indentation + "<" + this.TagName + this.FormatAttributes() + ">" + (this.NoLineBreaks || indent < 0 ? "" : "\n");
			if (!this.Empty || this.RequiresEndTag)
				result = result + this.Fold((node, accumulator) => accumulator + node.Format(this.NoLineBreaks || indent < 0 ? -1 : indent + 1), "") +
					(this.NoLineBreaks ? "" : indentation) + "</" + this.TagName + ">";
			if (indent >= 0)
				result += "\n";
			return result;
		}
		System.Collections.Generic.IEnumerator<Node> System.Collections.Generic.IEnumerable<Node>.GetEnumerator()
		{
			return this.childNodes.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.childNodes.GetEnumerator();
		}
	}
}
