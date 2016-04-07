using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Core.Resource.Texture
{
    public class TextureHelper
    {
        public static void ApplyTextureProperties(TextureProperties properties)
        {
            /*glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, properties.getMagFilter() == EnumTextureFilter.LINEAR ? (properties.hasMipMap() ? GL_LINEAR_MIPMAP_LINEAR : GL_LINEAR) : (properties.hasMipMap() ? GL_LINEAR_MIPMAP_NEAREST : GL_NEAREST));
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, properties.getMinFilter() == EnumTextureFilter.LINEAR ? (properties.hasMipMap() ? GL_LINEAR_MIPMAP_LINEAR : GL_LINEAR) : (properties.hasMipMap() ? GL_LINEAR_MIPMAP_NEAREST : GL_NEAREST));

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, properties.getWrapS() == EnumTextureWrap.CLAMP_TO_EDGE ? GL_CLAMP_TO_EDGE : GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, properties.getWrapT() == EnumTextureWrap.CLAMP_TO_EDGE ? GL_CLAMP_TO_EDGE : GL_REPEAT);
            */
        }

        public static void GenerateMipMap(int width, int height, bool hasAlpha, byte[] buffer)
        {
           // GLU.gluBuild2DMipmaps(GL11.GL_TEXTURE_2D, hasAlpha ? 4 : 3, width, height, hasAlpha ? GL11.GL_RGBA : GL11.GL_RGB, GL11.GL_UNSIGNED_BYTE, buffer);
           // System.out.println("mip maps are generating");
        }

        public static void InitTextureSwizzle(int[] mask, byte bpp)
        {
            /*
            if (mask != null)
            {
                int length = mask.length;
                if (length > 0)
                {
                    glTexParameteri(GL_TEXTURE_2D, GL33.GL_TEXTURE_SWIZZLE_R, mask[0]);
                    if (length > 1 && bpp > 1)
                    {
                        glTexParameteri(GL_TEXTURE_2D, GL33.GL_TEXTURE_SWIZZLE_G, mask[1]);
                        if (length > 2)
                        {
                            glTexParameteri(GL_TEXTURE_2D, GL33.GL_TEXTURE_SWIZZLE_B, mask[2]);
                            if (length > 3 && bpp > 3)
                            {
                                glTexParameteri(GL_TEXTURE_2D, GL33.GL_TEXTURE_SWIZZLE_A, mask[3]);
                            }
                        }
                    }
                }
            }
            */
        }
    }
}
