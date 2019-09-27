using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.CommMessage.Controls
{
    /// <summary>
    /// draw icon prop
    /// </summary>
    public class SymbolParams
    {
        /// <summary>
        /// fillcolor
        /// </summary>
        public Color FillColorParm { get; set; }
        /// <summary>
        /// toumingdu
        /// </summary>
        public double TransparentParm { get; set; }
        /// <summary>
        /// line color
        /// </summary>
        public Color LineColorParm { get; set; } 
        /// <summary>
        /// line width
        /// </summary>
        public double LineWidthParm { get; set; }
        /// <summary>
        /// point color
        /// </summary>
        public Color MarkColorParm { get; set; } 
        /// <summary>
        /// point size
        /// </summary>
        public int MarkSizeParm { get; set; }
    }
}
