using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Core.Resource.Texture
{
    public class TextureProperties
    {
        private bool hasMipMap = false;
        private TextureMinFilter minFilter = TextureMinFilter.Nearest;
        private TextureMagFilter magFilter = TextureMagFilter.Nearest;
        private PixelType pixelType = PixelType.UnsignedByte;
        private TextureWrapMode wrapS = TextureWrapMode.Repeat;
        private TextureWrapMode wrapT = TextureWrapMode.Repeat;
        private PixelFormat colorModel = PixelFormat.Rgba;
        private PixelInternalFormat colorModelInternal = PixelInternalFormat.Rgba;

        public bool HasMipMap
        {
            get
            {
                return hasMipMap;
            }

            set
            {
                hasMipMap = value;
            }
        }

        public TextureMinFilter MinFilter
        {
            get
            {
                return minFilter;
            }

            set
            {
                minFilter = value;
            }
        }

        public TextureMagFilter MagFilter
        {
            get
            {
                return magFilter;
            }

            set
            {
                magFilter = value;
            }
        }

        public PixelType PixelType
        {
            get
            {
                return pixelType;
            }

            set
            {
                pixelType = value;
            }
        }

        public TextureWrapMode WrapS
        {
            get
            {
                return wrapS;
            }

            set
            {
                wrapS = value;
            }
        }

        public TextureWrapMode WrapT
        {
            get
            {
                return wrapT;
            }

            set
            {
                wrapT = value;
            }
        }

        public PixelFormat ColorModel
        {
            get
            {
                return colorModel;
            }

            set
            {
                colorModel = value;
            }
        }

        public PixelInternalFormat ColorModelInternal
        {
            get
            {
                return colorModelInternal;
            }

            set
            {
                colorModelInternal = value;
            }
        }
    }
}
