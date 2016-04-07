using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Core.Resource.Texture
{
    public class Texture
    {
        private int id;
        private int width;
        private int height;
        private int attachment;
        private byte bytesPerPixel;
        private TextureProperties textureProperties;

        public static Texture generate(int width, int height, TextureProperties properties)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            TextureHelper.ApplyTextureProperties(properties);
           // GL.TexImage2D(TextureTarget.Texture2D, 0, properties.ColorModelInternal, width, height, 0, properties.ColorModel, properties.PixelType, null);
            

            GL.BindTexture(TextureTarget.Texture2D, 0);
           // Texture tex = new Texture(texture, width, height, properties);

            return null;
        }
    }
}
