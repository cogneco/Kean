//
//  Framebuffer.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using Error = Kean.Error;

namespace Kean.Draw.OpenGL.Exception
{
    public class Framebuffer : Exception
    {
        internal Framebuffer()
            : this(OpenTK.Graphics.OpenGL.FramebufferErrorCode.FramebufferUndefined) { }
        internal Framebuffer(OpenTK.Graphics.OpenGL.FramebufferErrorCode error)
            : base(Error.Level.Recoverable, "OpenGL Framebuffer Error", Framebuffer.GetMessage(error))
        { }
        static string GetMessage(OpenTK.Graphics.OpenGL.FramebufferErrorCode error)
        {
            string result = null;
            switch (error)
            {
                case OpenTK.Graphics.OpenGL.FramebufferErrorCode.FramebufferCompleteExt:
                    result = "No framebuffer error.";
                    break;
                case OpenTK.Graphics.OpenGL.FramebufferErrorCode.FramebufferIncompleteAttachmentExt:
                    result = "One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.";
                    break;
                case OpenTK.Graphics.OpenGL.FramebufferErrorCode.FramebufferIncompleteMissingAttachmentExt:
                    result = "There are no attachments.";
                    break;
                case OpenTK.Graphics.OpenGL.FramebufferErrorCode.FramebufferIncompleteDrawBufferExt:
                    result = "An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.";
                    break;
                case OpenTK.Graphics.OpenGL.FramebufferErrorCode.FramebufferIncompleteReadBufferExt:
                    result = "The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.";
                    break;
                case OpenTK.Graphics.OpenGL.FramebufferErrorCode.FramebufferUnsupportedExt:
                    result = "This particular FBO configuration is not supported by the implementation.";
                    break;
                default:
                    result = "Unknown framebuffer error.";
                    break;
            }
            return result;
        }
        internal static void Check()
        {
            OpenTK.Graphics.OpenGL.FramebufferErrorCode error = OpenTK.Graphics.OpenGL.GL.Ext.CheckFramebufferStatus(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt);
            if (error != OpenTK.Graphics.OpenGL.FramebufferErrorCode.FramebufferCompleteExt)
                new Framebuffer(error).Throw();
        }
    }
}
