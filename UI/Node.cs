using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace UI
{
    public class Node
    {
        public String label { get; set; }
        public Boolean IsChanged { get; set; }
        /// <summary>
        /// the background image of the node
        /// </summary>
        private static Bitmap _nodeBg = new Bitmap(30,25);

        private static Size _freespace = new Size(_nodeBg.Width, _nodeBg.Height);
        static Node()
        {
            var g = Graphics.FromImage(_nodeBg);
        }
    }
}
