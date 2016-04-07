using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Core.Event.RendererEvents
{
    public class EventRenderTargetResizedArgs : EventArgs
    {
        private int _width;
        private int _height;
        private int _prevWidth;
        private int _prevHeight;

        public EventRenderTargetResizedArgs(int width, int height, int prevWidth, int prevHeight)
        {
            this.Width = width;
            this.Height = height;
            this.PrevWidth = width;
            this.PrevHeight = height;
        }

        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
            }
        }

        public int PrevWidth
        {
            get
            {
                return _prevWidth;
            }

            set
            {
                _prevWidth = value;
            }
        }

        public int PrevHeight
        {
            get
            {
                return _prevHeight;
            }

            set
            {
                _prevHeight = value;
            }
        }
    }
}
